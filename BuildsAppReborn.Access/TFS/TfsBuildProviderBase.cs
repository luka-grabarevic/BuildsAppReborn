using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BuildsAppReborn.Access.Models;
using BuildsAppReborn.Access.Models.Internal;
using BuildsAppReborn.Contracts;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Svg;

namespace BuildsAppReborn.Access
{
    internal abstract class TfsBuildProviderBase<TBuild, TBuildDefinition, TUser, TSourceVersion, TArtifact, TTestRun, TPullRequest> : TfsBuildProviderBase, IBuildProvider
        where TBuild : TfsBuild, new()
        where TBuildDefinition : TfsBuildDefinition, new()
        where TUser : TfsUser, new()
        where TSourceVersion : TfsSourceVersion, new()
        where TArtifact : TfsArtifact, new()
        where TTestRun : TfsTestRun, new()
        where TPullRequest : TfsPullRequest, new()
    {
        public virtual async Task<DataResponse<IEnumerable<IBuildDefinition>>> GetBuildDefinitionsAsync(BuildMonitorSettings settings)
        {
            var projectUrl = settings.GetValueStrict<String>(ProjectUrlSettingKey).TrimEnd('/');
            if (!String.IsNullOrWhiteSpace(projectUrl))
            {
                var requestUrl = $"{projectUrl}/_apis/build/definitions?api-version={ApiVersion}";

                var requestResponse = await GetRequestResponseAsync(requestUrl, settings).ConfigureAwait(false);
                requestResponse.ThrowIfUnsuccessful();
                
                var result = await requestResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var data = JsonConvert.DeserializeObject<List<TBuildDefinition>>(JObject.Parse(result)["value"].ToString());
                foreach (var buildDefinition in data)
                {
                    buildDefinition.BuildSettingsId = settings.UniqueId;
                }

                return new DataResponse<IEnumerable<IBuildDefinition>> {Data = data, StatusCode = requestResponse.StatusCode};
            }

            throw new Exception("Error while processing method!");
        }

        public virtual async Task<DataResponse<IEnumerable<IBuild>>> GetBuildsAsync(IEnumerable<IBuildDefinition> buildDefinitions, BuildMonitorSettings settings)
        {
            var buildDefinitionsList = buildDefinitions.ToList();
            if (!buildDefinitionsList.Any())
            {
                return new DataResponse<IEnumerable<IBuild>> {Data = Enumerable.Empty<TBuild>(), StatusCode = HttpStatusCode.NoContent};
            }

            var projectUrl = settings.GetValueStrict<String>(ProjectUrlSettingKey).TrimEnd('/');
            var maxBuilds = settings.GetDefaultValueIfNotExists<Int32?>(MaxBuildsPerDefinitionSettingsKey) ?? 5;

            // use fallback value when no value was defined via settings

            if (!String.IsNullOrWhiteSpace(projectUrl))
            {
                var buildDefinitionsCommaList = String.Join(",", buildDefinitionsList.Select(a => a.Id));
                var requestUrl = $"{projectUrl}/_apis/build/builds?api-version={ApiVersion}&definitions={buildDefinitionsCommaList}&maxBuildsPerDefinition={maxBuilds}";
                var requestResponse = await GetRequestResponseAsync(requestUrl, settings).ConfigureAwait(false);
                requestResponse.ThrowIfUnsuccessful();

                var result = await requestResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var data = JsonConvert.DeserializeObject<List<TBuild>>(JObject.Parse(result)["value"].ToString());
                await ResolveAdditionalBuildDataAsync(settings, data, projectUrl).ConfigureAwait(false);

                return new DataResponse<IEnumerable<IBuild>> {Data = data, StatusCode = requestResponse.StatusCode};
            }

            throw new Exception("Error while processing method!");
        }

        public virtual async Task<DataResponse<IEnumerable<IBuild>>> GetBuildsByPullRequestsAsync(BuildMonitorSettings settings)
        {
            var projectUrl = settings.GetValueStrict<String>(ProjectUrlSettingKey).TrimEnd('/');

            var requestUrl = $"{projectUrl}/_apis/git/pullrequests?api-version={ApiVersion}";
            var requestResponse = await GetRequestResponseAsync(requestUrl, settings).ConfigureAwait(false);
            requestResponse.ThrowIfUnsuccessful();

            var dict = new Dictionary<IPullRequest, IEnumerable<TBuild>>();

            var result = await requestResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var data = JsonConvert.DeserializeObject<List<TPullRequest>>(JObject.Parse(result)["value"].ToString());

            foreach (var pullRequest in data)
            {
                var buildsResponse = await GetBuildsOfPullRequestAsync(pullRequest, settings).ConfigureAwait(false);
                if (buildsResponse.IsSuccessStatusCode)
                {
                    dict.Add(pullRequest, buildsResponse.Data);
                }
                else
                {
                    throw new Exception("Error while processing method!");
                }
            }

            // sets the relation of the PR to the build object
            foreach (var keyValuePair in dict)
            {
                foreach (var build in keyValuePair.Value)
                {
                    build.PullRequest = keyValuePair.Key;
                }
            }

            return new DataResponse<IEnumerable<IBuild>> {Data = dict.Values.SelectMany(a => a).ToList(), StatusCode = requestResponse.StatusCode};
        }

        protected abstract String ApiVersion { get; }

        private static async Task<Byte[]> GetImageDataAsync(BuildMonitorSettings settings, IUser user)
        {
            try
            {
                var response = await GetRequestResponseAsync(user.ImageUrl, settings).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    if (response.Content.Headers.ContentType.MediaType == "image/png")
                    {
                        return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                    }

                    if (response.Content.Headers.ContentType.MediaType == "image/svg+xml")
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await response.Content.CopyToAsync(memoryStream).ConfigureAwait(false);
                            memoryStream.Position = 0;
                            var svgDoc = SvgDocument.Open<SvgDocument>(memoryStream);

                            var converter = new ImageConverter();
                            return (Byte[]) converter.ConvertTo(svgDoc.Draw(128, 128), typeof(Byte[]));
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }

        private static async Task<HttpResponseMessage> GetRequestResponseAsync(String requestUrl, BuildMonitorSettings settings)
        {
            var credentials = settings.GetValueStrict<ICredentials>(ProjectCredentialsSettingKey);
            var accessToken = settings.GetDefaultValueIfNotExists<String>(PersonalAccessTokenSettingsKey);

            if (!String.IsNullOrWhiteSpace(accessToken))
            {
                return await HttpRequestHelper.GetRequestResponseAsync(requestUrl, accessToken).ConfigureAwait(false);
            }

            return await HttpRequestHelper.GetRequestResponseAsync(requestUrl, credentials).ConfigureAwait(false);
        }

        private static async Task<TInnerSourceVersion> GetSourceVersionAsync<TInnerSourceVersion>(BuildMonitorSettings settings, String requestUrl)
            where TInnerSourceVersion : ISourceVersion, new()
        {
            var requestResponse = await GetRequestResponseAsync(requestUrl, settings).ConfigureAwait(false);
            if (requestResponse.IsSuccessStatusCode)
            {
                var result = await requestResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<TInnerSourceVersion>(result);
            }

            return default(TInnerSourceVersion);
        }

        private static async Task<TSourceVersion> GetTfsGitSourceVersionAsync(BuildMonitorSettings settings, String projectUrl, TBuild build)
        {
            var requestUrl = $"{projectUrl}/_apis/git/repositories/{build.Repository.Id}/commits/{build.SourceVersionInternal}?api-version=1.0";
            var sourceVersion = await GetSourceVersionAsync<TSourceVersion>(settings, requestUrl).ConfigureAwait(false);

            // special case detection if it as auto merge commit made by TFS pull requests
            if (build.Reason == BuildReason.PullRequest || 
                build.Reason == BuildReason.Validation)
            {
                if (sourceVersion?.Parents != null && sourceVersion.Parents.Length > 1 && sourceVersion.Pusher != null && sourceVersion.Pusher.IsServiceUser)
                {
                    var innerRequestUrl = $"{projectUrl}/_apis/git/repositories/{build.Repository.Id}/commits/{sourceVersion.Parents.Last()}?api-version=1.0";

                    return await GetSourceVersionAsync<TSourceVersion>(settings, innerRequestUrl).ConfigureAwait(false);
                }
            }

            return sourceVersion;
        }

        private static async Task ResolveTestRunsAsync(IEnumerable<TBuild> builds, String projectUrl, BuildMonitorSettings settings)
        {
            foreach (var build in builds)
            {
                var requestUrl = $"{projectUrl}/_apis/test/runs?api-version=1.0&buildUri={build.Uri}";

                var requestResponse = await GetRequestResponseAsync(requestUrl, settings).ConfigureAwait(false);
                if (requestResponse.IsSuccessStatusCode)
                {
                    var result = await requestResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                    build.TestRuns = JsonConvert.DeserializeObject<IEnumerable<TTestRun>>(JObject.Parse(result)["value"].ToString());
                }
            }
        }

        private async Task<DataResponse<IEnumerable<TBuild>>> GetBuildsOfPullRequestAsync(IPullRequest pullRequest, BuildMonitorSettings settings)
        {
            var projectUrl = settings.GetValueStrict<String>(ProjectUrlSettingKey).TrimEnd('/');
            var maxBuilds = settings.GetDefaultValueIfNotExists<Int32?>(MaxBuildsPerDefinitionSettingsKey) ?? 5;

            // use fallback value when no value was defined via settings

            var requestUrl = $"{projectUrl}/_apis/build/builds?api-version={ApiVersion}&branchName=refs%2Fpull%2F{pullRequest.Id}%2Fmerge&$top={maxBuilds}";
            var requestResponse = await GetRequestResponseAsync(requestUrl, settings).ConfigureAwait(false);
            requestResponse.ThrowIfUnsuccessful();

            var result = await requestResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var data = JsonConvert.DeserializeObject<List<TBuild>>(JObject.Parse(result)["value"].ToString());

            await ResolveAdditionalBuildDataAsync(settings, data, projectUrl).ConfigureAwait(false);

            return new DataResponse<IEnumerable<TBuild>> {Data = data, StatusCode = requestResponse.StatusCode};
        }

        private Tuple<String, String> GetGitOwnerAndRepo(String gitHubRepoUrl)
        {
            if (!String.IsNullOrWhiteSpace(gitHubRepoUrl))
            {
                var split = gitHubRepoUrl.Split('/');

                if (split.Length > 2)
                {
                    var owner = split[split.Length - 2];
                    var repoRaw = split[split.Length - 1];
                    var gitIndex = repoRaw.LastIndexOf(".git", StringComparison.InvariantCultureIgnoreCase);
                    var repo = repoRaw.Substring(0, gitIndex != -1 ? gitIndex : repoRaw.Length);

                    return new Tuple<String, String>(owner, repo);
                }
            }

            return null;
        }

        private async Task ResolveAdditionalBuildDataAsync(BuildMonitorSettings settings, List<TBuild> data, String projectUrl)
        {
            data.Select(d => d.Definition).OfType<TBuildDefinition>().ToList().ForEach(d => d.BuildSettingsId = settings.UniqueId);

            await ResolveSourceVersionAsync(data, projectUrl, settings).ConfigureAwait(false);
            await ResolveArtifactsAsync(data, projectUrl, settings).ConfigureAwait(false);
            await ResolveTestRunsAsync(data, projectUrl, settings).ConfigureAwait(false);

            data.Select(d => d.DisplayUser).OfType<TUser>().ToList().ForEach(a => a.ImageDataLoader = GetImageDataAsync(settings, a));
        }

        private async Task ResolveArtifactsAsync(IEnumerable<TBuild> builds, String projectUrl, BuildMonitorSettings settings)
        {
            foreach (var build in builds)
            {
                var requestUrl = $"{projectUrl}/_apis/build/builds/{build.Id}/artifacts?api-version={ApiVersion}";

                var requestResponse = await GetRequestResponseAsync(requestUrl, settings).ConfigureAwait(false);
                if (requestResponse.IsSuccessStatusCode)
                {
                    var result = await requestResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                    build.Artifacts = JsonConvert.DeserializeObject<IEnumerable<TArtifact>>(JObject.Parse(result)["value"].ToString());
                }
            }
        }

        private async Task ResolveSourceVersionAsync(IEnumerable<TBuild> builds, String projectUrl, BuildMonitorSettings settings)
        {
            var grpByRepoType = builds.GroupBy(a => a.Repository.RepositoryType);
            foreach (var group in grpByRepoType)
            {
                if (group.Key == RepositoryType.TfsVersionControl)
                {
                    foreach (var build in group)
                    {
                        var requestUrl = $"{projectUrl}/_apis/tfvc/changesets/{build.SourceVersionInternal}?api-version=1.0&includeDetails=true";
                        build.SourceVersion = await GetSourceVersionAsync<TSourceVersion>(settings, requestUrl).ConfigureAwait(false);
                    }
                }
                else if (group.Key == RepositoryType.TfsGit)
                {
                    foreach (var build in group)
                    {
                        build.SourceVersion = await GetTfsGitSourceVersionAsync(settings, projectUrl, build).ConfigureAwait(false);
                    }
                }
                else if (group.Key == RepositoryType.GitHub)
                {
                    // ToDo: can't poll everytime because of rate limits...
                    return;
#if DEBUG

                    // special case when it is a TFS project with external GitHub repository
                    foreach (var build in group)
                    {
                        var ownerAndRepo = GetGitOwnerAndRepo(build.Repository.Id);
                        var requestUrl = $"{GitHubApiPrefix}/repos/{ownerAndRepo.Item1}/{ownerAndRepo.Item2}/commits/{build.SourceVersionInternal}";

                        var requestResponse = await HttpRequestHelper.GetRequestResponseAsync(requestUrl).ConfigureAwait(false);
                        if (requestResponse.IsSuccessStatusCode)
                        {
                            var result = await requestResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                            build.SourceVersion = JsonConvert.DeserializeObject<GitHubSourceVersion>(result);
                        }
                    }
#endif
                }
            }
        }
    }

    internal abstract class TfsBuildProviderBase
    {
        internal const String GitHubApiPrefix = "https://api.github.com";

        internal const String ProjectUrlSettingKey = "ProjectUrl";

        internal const String ProjectCredentialsSettingKey = "ProjectCredentials";

        internal const String PersonalAccessTokenSettingsKey = "PersonalAccessToken";

        internal const String MaxBuildsPerDefinitionSettingsKey = "MaxBuildsPerDefinition";
    }
}
using System;
using System.Collections.Generic;
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

namespace BuildsAppReborn.Access
{
    internal abstract class TfsBuildProviderBase<TBuild, TBuildDefinition, TUser, TSourceVersion, TArtifact, TTestRun> : TfsBuildProviderBase, IBuildProvider
        where TBuild : TfsBuild, new()
        where TBuildDefinition : TfsBuildDefinition, new()
        where TUser : TfsUser, new()
        where TSourceVersion : TfsSourceVersion, new()
        where TArtifact : TfsArtifact, new()
        where TTestRun :  TfsTestRun, new()
    {
        #region Implementation of IBuildProvider

        public virtual async Task<DataResponse<IEnumerable<IBuildDefinition>>> GetBuildDefinitions(BuildMonitorSettings settings)
        {
            var projectUrl = settings.GetValueStrict<String>(ProjectUrlSettingKey).TrimEnd('/');
            if (!String.IsNullOrWhiteSpace(projectUrl))
            {
                var requestUrl = $"{projectUrl}/_apis/build/definitions?api-version={ApiVersion}";

                var requestResponse = await GetRequestResponse(requestUrl, settings);
                if (requestResponse.IsSuccessStatusCode)
                {
                    var result = await requestResponse.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<TBuildDefinition>>(JObject.Parse(result)["value"].ToString());
                    foreach (var buildDefinition in data)
                    {
                        buildDefinition.BuildSettingsId = settings.UniqueId;
                    }

                    return new DataResponse<IEnumerable<IBuildDefinition>> {Data = data, StatusCode = requestResponse.StatusCode};
                }

                return new DataResponse<IEnumerable<IBuildDefinition>> {Data = Enumerable.Empty<IBuildDefinition>(), StatusCode = requestResponse.StatusCode};
            }

            throw new Exception($"Error while processing method!");
        }

        public virtual async Task<DataResponse<IEnumerable<IBuild>>> GetBuilds(IEnumerable<IBuildDefinition> buildDefinitions, BuildMonitorSettings settings)
        {
            var buildDefinitionsList = buildDefinitions.ToList();
            if (!buildDefinitionsList.Any())
            {
                return new DataResponse<IEnumerable<IBuild>> {Data = Enumerable.Empty<IBuild>(), StatusCode = HttpStatusCode.NoContent};
            }

            var projectUrl = settings.GetValueStrict<String>(ProjectUrlSettingKey).TrimEnd('/');
            var maxBuilds = settings.GetDefaultValueIfNotExists<Int32?>(MaxBuildsPerDefinitionSettingsKey);

            // use fallback value when no value was defined via settings
            if (!maxBuilds.HasValue) maxBuilds = 5;

            if (!String.IsNullOrWhiteSpace(projectUrl))
            {
                var buildDefinitionsCommaList = String.Join(",", buildDefinitionsList.Select(a => a.Id));
                var requestUrl = $"{projectUrl}/_apis/build/builds?api-version={ApiVersion}&definitions={buildDefinitionsCommaList}&maxBuildsPerDefinition={maxBuilds}";
                var requestResponse = await GetRequestResponse(requestUrl, settings);
                if (requestResponse.IsSuccessStatusCode)
                {
                    var result = await requestResponse.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<TBuild>>(JObject.Parse(result)["value"].ToString());
                    data.Select(d => d.Definition).OfType<TBuildDefinition>().ToList().ForEach(d => d.BuildSettingsId = settings.UniqueId);
                    data.Select(d => d.Requester).OfType<TUser>().ToList().ForEach(a => a.ImageDataLoader = GetImageData(settings, a));

                    await ResolveSourceVersion(data, projectUrl, settings);
                    await ResolveArtifacts(data, projectUrl, settings);
                    await ResolveTestRuns(data, projectUrl, settings);

                    return new DataResponse<IEnumerable<IBuild>> {Data = data, StatusCode = requestResponse.StatusCode};
                }

                return new DataResponse<IEnumerable<IBuild>> {Data = Enumerable.Empty<IBuild>(), StatusCode = requestResponse.StatusCode};
            }

            throw new Exception($"Error while processing method!");
        }

        #endregion

        #region Protected Properties

        protected abstract String ApiVersion { get; }

        #endregion

        #region Private Static Methods

        private static async Task<Byte[]> GetImageData(BuildMonitorSettings settings, IUser user)
        {
            var response = await GetRequestResponse(user.ImageUrl, settings);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }

            return null;
        }

        private static async Task<HttpResponseMessage> GetRequestResponse(String requestUrl, BuildMonitorSettings settings)
        {
            var credentials = settings.GetValueStrict<ICredentials>(ProjectCredentialsSettingKey);
            var accessToken = settings.GetDefaultValueIfNotExists<String>(PersonalAccessTokenSettingsKey);

            if (!String.IsNullOrWhiteSpace(accessToken))
            {
                return await HttpRequestHelper.GetRequestResponse(requestUrl, accessToken);
            }

            return await HttpRequestHelper.GetRequestResponse(requestUrl, credentials);
        }

        #endregion

        #region Private Methods

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

        private async Task ResolveArtifacts(IEnumerable<TBuild> builds, String projectUrl, BuildMonitorSettings settings)
        {
            foreach (var build in builds)
            {
                var requestUrl = $"{projectUrl}/_apis/build/builds/{build.Id}/artifacts?api-version={ApiVersion}";

                var requestResponse = await GetRequestResponse(requestUrl, settings);
                if (requestResponse.IsSuccessStatusCode)
                {
                    var result = await requestResponse.Content.ReadAsStringAsync();
                    build.Artifacts = JsonConvert.DeserializeObject<IEnumerable<TArtifact>>(JObject.Parse(result)["value"].ToString());
                }
            }
        }

        private async Task ResolveSourceVersion(IEnumerable<TBuild> builds, String projectUrl, BuildMonitorSettings settings)
        {
            var grpByRepoType = builds.GroupBy(a => a.Repository.RepositoryType);
            foreach (var group in grpByRepoType)
            {
                if (group.Key == RepositoryType.TfsVersionControl)
                {
                    foreach (var build in group)
                    {
                        var requestUrl = $"{projectUrl}/_apis/tfvc/changesets/{build.SourceVersionInternal}?api-version=1.0&includeDetails=true";
                        await SetSourceVersion<TSourceVersion>(settings, requestUrl, build);
                    }
                }
                else if (group.Key == RepositoryType.TfsGit)
                {
                    foreach (var build in group)
                    {
                        var requestUrl = $"{projectUrl}/_apis/git/repositories/{build.Repository.Id}/commits/{build.SourceVersionInternal}?api-version=1.0";
                        await SetSourceVersion<TSourceVersion>(settings, requestUrl, build);
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

                        var requestResponse = await HttpRequestHelper.GetRequestResponse(requestUrl);
                        if (requestResponse.IsSuccessStatusCode)
                        {
                            var result = await requestResponse.Content.ReadAsStringAsync();
                            build.SourceVersion = JsonConvert.DeserializeObject<GitHubSourceVersion>(result);
                        }
                    }
#endif
                }
            }
        }

        private async Task ResolveTestRuns(IEnumerable<TBuild> builds, String projectUrl, BuildMonitorSettings settings)
        {
            foreach (var build in builds)
            {
                var requestUrl = $"{projectUrl}/_apis/test/runs?api-version=1.0&buildUri={build.Uri}";

                var requestResponse = await GetRequestResponse(requestUrl, settings);
                if (requestResponse.IsSuccessStatusCode)
                {
                    var result = await requestResponse.Content.ReadAsStringAsync();
                    var value = JObject.Parse(result)["value"].ToString();
                    build.TestRun = JsonConvert.DeserializeObject<TTestRun>(value);
                }
            }
        }

        private async Task SetSourceVersion<TInnerSourceVersion>(BuildMonitorSettings settings, String requestUrl, TBuild build) where TInnerSourceVersion : ISourceVersion, new()
        {
            var requestResponse = await GetRequestResponse(requestUrl, settings);
            if (requestResponse.IsSuccessStatusCode)
            {
                var result = await requestResponse.Content.ReadAsStringAsync();
                build.SourceVersion = JsonConvert.DeserializeObject<TInnerSourceVersion>(result);
            }
        }

        #endregion
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
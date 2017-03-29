using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BuildsAppReborn.Access.Models;
using BuildsAppReborn.Contracts;
using BuildsAppReborn.Contracts.Composition;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BuildsAppReborn.Access
{
    [BuildProviderExport(typeof(IBuildProvider), Id, Name, AuthenticationModes.Default | AuthenticationModes.AccessToken)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class Tfs2017BuildProvider : IBuildProvider
    {
        #region Implementation of IBuildProvider

        public async Task<DataResponse<IEnumerable<IBuildDefinition>>> GetBuildDefinitions(BuildMonitorSettings settings)
        {
            var projectUrl = settings.GetValueStrict<String>(ProjectUrlSettingKey).TrimEnd('/');
            if (!String.IsNullOrWhiteSpace(projectUrl))
            {
                var requestUrl = $"{projectUrl}/_apis/build/definitions?api-version={ApiVersion}";

                var requestResponse = await GetRequestResponse(requestUrl, settings);
                if (requestResponse.IsSuccessStatusCode)
                {
                    var result = await requestResponse.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<Tfs2017BuildDefinition>>(JObject.Parse(result)["value"].ToString());
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

        public async Task<DataResponse<IEnumerable<IBuild>>> GetBuilds(IEnumerable<IBuildDefinition> buildDefinitions, BuildMonitorSettings settings)
        {
            var buildDefinitionsList = buildDefinitions.ToList();
            if (!buildDefinitionsList.Any())
            {
                return new DataResponse<IEnumerable<IBuild>> {Data = Enumerable.Empty<IBuild>()};
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
                    var data = JsonConvert.DeserializeObject<List<Tfs2017Build>>(JObject.Parse(result)["value"].ToString());
                    data.Select(d => d.Definition).OfType<Tfs2017BuildDefinition>().ToList().ForEach(d => d.BuildSettingsId = settings.UniqueId);
                    data.Select(d => d.Requester).OfType<Tfs2017User>().ToList().ForEach(a => a.ImageDataLoader = GetImageData(settings, a));

                    return new DataResponse<IEnumerable<IBuild>> {Data = data, StatusCode = requestResponse.StatusCode};
                }
                return new DataResponse<IEnumerable<IBuild>> {Data = Enumerable.Empty<IBuild>(), StatusCode = requestResponse.StatusCode};
            }

            throw new Exception($"Error while processing method!");
        }

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

        private const String ApiVersion = "2.0";

        internal const String Id = "08e2efa9-2407-4f2e-a159-aa2b223abaac";

        internal const String Name = "TFS 2017";

        internal const String ProjectUrlSettingKey = "ProjectUrl";

        internal const String ProjectCredentialsSettingKey = "ProjectCredentials";

        internal const String PersonalAccessTokenSettingsKey = "PersonalAccessToken";

        internal const String MaxBuildsPerDefinitionSettingsKey = "MaxBuildsPerDefinition";
    }
}
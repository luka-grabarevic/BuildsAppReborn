using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;

using BuildsAppReborn.Access.Models;
using BuildsAppReborn.Contracts;
using BuildsAppReborn.Contracts.Composition;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BuildsAppReborn.Access
{
    [BuildProviderExport(typeof(IBuildProvider), Id, Name)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class Tfs2017BuildProvider : IBuildProvider
    {
        #region Implementation of IBuildProvider

        public IEnumerable<IBuildDefinition> GetBuildDefinitions(BuildMonitorSettings settings)
        {
            var projectUrl = settings.GetValueStrict<String>(ProjectUrlSettingKey).TrimEnd('/');
            var credentials = settings.GetValueStrict<ICredentials>(ProjectCredentialsSettingKey);

            if (!String.IsNullOrWhiteSpace(projectUrl) && credentials != null)
            {
                var requestUrl = $"{projectUrl}/_apis/build/definitions?api-version={ApiVersion}";

                var result = HttpRequestHelper.GetRequestResult(requestUrl, credentials);

                return JsonConvert.DeserializeObject<List<Tfs2017BuildDefinition>>(JObject.Parse(result.Result)["value"].ToString());
            }

            throw new Exception($"Error while processing method!");
        }

        public IEnumerable<IBuild> GetBuilds(IEnumerable<IBuildDefinition> buildDefinitions, BuildMonitorSettings settings)
        {
            var buildDefinitionsList = buildDefinitions.ToList();
            if (!buildDefinitionsList.Any())
            {
                return Enumerable.Empty<IBuild>();
            }
            var projectUrl = settings.GetValueStrict<String>(ProjectUrlSettingKey).TrimEnd('/');
            var credentials = settings.GetValueStrict<ICredentials>(ProjectCredentialsSettingKey);
            var maxBuilds = settings.GetDefaultValueIfNotExists<Int32?>(MaxBuildsPerDefinitionSettingsKey);

            // use fallback value when no value was defined via settings
            if (!maxBuilds.HasValue) maxBuilds = 5;

            if (!String.IsNullOrWhiteSpace(projectUrl) && credentials != null)
            {
                var buildDefinitionsCommaList = String.Join(",", buildDefinitionsList.Select(a => a.Id));
                var requestUrl = $"{projectUrl}/_apis/build/builds?api-version={ApiVersion}&definitions={buildDefinitionsCommaList}&maxBuildsPerDefinition={maxBuilds}";

                var result = HttpRequestHelper.GetRequestResult(requestUrl, credentials);

                return JsonConvert.DeserializeObject<List<Tfs2017Build>>(JObject.Parse(result.Result)["value"].ToString());
            }

            throw new Exception($"Error while processing method!");
        }

        #endregion

        private const String ApiVersion = "2.0";

        internal const String Id = "08e2efa9-2407-4f2e-a159-aa2b223abaac";

        internal const String Name = "TFS 2017";

        internal const String ProjectUrlSettingKey = "ProjectUrl";

        internal const String ProjectCredentialsSettingKey = "ProjectCredentials";

        internal const String MaxBuildsPerDefinitionSettingsKey = "MaxBuildsPerDefinition";
    }
}
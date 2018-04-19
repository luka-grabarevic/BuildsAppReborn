using System;
using BuildsAppReborn.Contracts.Models;
using Newtonsoft.Json;

namespace BuildsAppReborn.Access.Models
{
    internal abstract class TfsTestRun : ITestRun
    {
        #region Implementation of ITestRun

        [JsonProperty("url")]
        public String Url { get; private set; }

        [JsonProperty("webAccessUrl")]
        public String WebUrl { get; private set; }

        [JsonProperty("id")]
        public Int32 Id { get; private set; }

        [JsonProperty("incompleteTests")]
        public Int32 IncompleteTests { get; private set; }

        [JsonProperty("name")]
        public String Name { get; private set; }

        [JsonProperty("notApplicableTests")]
        public Int32 NotApplicableTests { get; private set; }

        [JsonProperty("passedTests")]
        public Int32 PassedTests { get; private set; }

        [JsonProperty("state")]
        public String State { get; private set; }

        [JsonProperty("totalTests")]
        public Int32 TotalTests { get; private set; }

        [JsonProperty("unanalyzedTests")]
        public Int32 FailedTests { get; private set; }

        #endregion
    }
}
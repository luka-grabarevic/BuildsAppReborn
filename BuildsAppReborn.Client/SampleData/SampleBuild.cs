using System;
using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Client.SampleData
{
    internal class SampleBuild : IBuild
    {
        #region Implementation of IBuild

        public IBuildDefinition Definition { get; set; }
        public DateTime FinishDateTime { get; set; }
        public Int32 Id { get; set; }
        public DateTime QueueDateTime { get; set; }
        public DateTime StartDateTime { get; set; }
        public BuildStatus Status { get; set; }
        public IUser Requester { get; }
        public String Url { get; }

        #endregion
    }
}
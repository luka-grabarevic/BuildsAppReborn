using System;
using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Client.SampleData
{
    internal class SampleBuildDefinition : IBuildDefinition
    {
        #region Implementation of IBuildDefinition

        public Int32 Id { get; set; }
        public String Name { get; set; }
        public IProject Project { get; set; }
        public String Type { get; set; }
        public String Url { get; }

        #endregion
    }
}
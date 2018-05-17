using System;
using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Infrastructure.Tests.TestData
{
    public class DummyDefinition : IBuildDefinition
    {
        public String BuildSettingsId { get; }

        public Int32 Id { get; set; }

        public String Name { get; set; }

        public IProject Project { get; set; }

        public String Type { get; set; }

        public String Url { get; }
    }
}
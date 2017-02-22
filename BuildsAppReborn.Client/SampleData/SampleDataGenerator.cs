using System;
using System.Collections.Generic;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Contracts.UI;
using BuildsAppReborn.Infrastructure.Collections;

namespace BuildsAppReborn.Client.SampleData
{
    internal static class SampleDataGenerator
    {
        #region Internal Static Methods

        internal static RangeObservableCollection<BuildStatusGroup> GenerateSampleData()
        {
            var result = new RangeObservableCollection<BuildStatusGroup>();
            for (var i = 0; i < 10; i++)
            {
                var project = new SampleProject {Description = "My fancy project", Name = "Project", Id = $"Id{i}"};
                var buildDefinition = new SampleBuildDefinition {Id = i, Name = $"Build Definition {i}", Project = project, Type = "New"};
                var allBuilds = new List<BuildItem>();
                for (var j = 0; j < 5; j++)
                {
                    var sampleBuild = new SampleBuild
                                      {
                                          Definition = buildDefinition,
                                          FinishDateTime = DateTime.UtcNow,
                                          Id = i,
                                          QueueDateTime = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(1)),
                                          StartDateTime = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(1)),
                                          Status = GetRandomStatus(i)
                                      };
                    allBuilds.Add(new BuildItem(sampleBuild));
                }

                var build = new BuildStatusGroup {AllBuildItems = allBuilds, BuildDefinition = buildDefinition};
                result.Add(build);
            }
            return result;
        }

        #endregion

        #region Private Static Methods

        private static BuildStatus GetRandomStatus(Int32 i)
        {
            var mod = i % 7;
            switch (mod)
            {
                case 0:
                    return BuildStatus.Succeeded;
                case 1:
                    return BuildStatus.Failed;
                case 2:
                    return BuildStatus.PartiallySucceeded;
                case 3:
                    return BuildStatus.Queued;
                case 4:
                    return BuildStatus.Running;
                case 5:
                    return BuildStatus.Stopped;
                case 6:
                    return BuildStatus.Unknown;
            }
            return BuildStatus.Succeeded;
        }

        #endregion
    }
}
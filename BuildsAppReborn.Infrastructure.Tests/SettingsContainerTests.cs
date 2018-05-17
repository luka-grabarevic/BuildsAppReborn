using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure.Tests.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BuildsAppReborn.Infrastructure.Tests
{
    [TestClass]
    public class SettingsContainerTests
    {
        [TestMethod]
        public void TestIfSavingAndLoadingWorks()
        {
            var tempFile = Path.GetTempFileName();

            var settings = new BuildMonitorSettings(Guid.NewGuid().ToString());
            settings.Add("Test1", "02fdb4e0-fa5d-472a-918a-fc02c48b11a8");
            settings.Add("Test2", new List<String> {"Item1", "Item2"});
            //settings.Add("Test3", new List<IBuildDefinition> { GetTestBuildDefinition(1), GetTestBuildDefinition(2) });

            var container = new SettingsContainer<BuildMonitorSettings>();
            container.Add(settings);

            container.Save(tempFile);

            var loadedContainer = SettingsContainer<BuildMonitorSettings>.Load(tempFile);

            Assert.IsNotNull(loadedContainer);
            Assert.AreEqual(container.Count, loadedContainer.Count);

            var loadedSettings = loadedContainer.Single();

            Assert.IsNotNull(loadedSettings);

            Assert.AreEqual(settings.BuildProviderId, loadedSettings.BuildProviderId);
            Assert.AreEqual(settings.UniqueId, loadedSettings.UniqueId);
            Assert.AreEqual(settings.Count, loadedSettings.Count);

            foreach (var keyValuePair in settings)
            {
                var loadedValue = loadedSettings[keyValuePair.Key];
                Assert.IsNotNull(loadedValue);

                var expected = keyValuePair.Value as ICollection;
                if (expected != null)
                {
                    CollectionAssert.AreEqual(expected, loadedValue as ICollection);
                }
                else
                {
                    Assert.AreEqual(keyValuePair.Value, loadedValue);
                }
            }
        }

        private IBuildDefinition GetTestBuildDefinition(Int32 id)
        {
            return new DummyDefinition
            {
                Id = id,
                Name = Path.GetRandomFileName(),
                Type = "TestType",
                Project = new DummyProject {Name = Path.GetRandomFileName(), Description = Path.GetRandomFileName(), Id = Guid.NewGuid().ToString()}
            };
        }
    }
}
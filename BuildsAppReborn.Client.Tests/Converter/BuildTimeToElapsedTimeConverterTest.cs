using System;
using System.Globalization;
using BuildsAppReborn.Client.Converter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BuildsAppReborn.Client.Tests.Converter
{
    [TestClass]
    public class BuildTimeToElapsedTimeConverterTest
    {
        #region Public Methods

        [TestMethod]
        public void TestIfQuestionMarkTimeIsFixed()
        {
            var buildTime = DateTime.Parse("28.03.2017 23:10:18");
            var nowTime = DateTime.Parse("30.03.2017 12:07:10");

            var converter = new BuildTimeToElapsedTimeConverter();
            var result = converter.Convert(buildTime, typeof(String), nowTime, CultureInfo.CurrentUICulture);
            Assert.AreEqual("1d", result);
        }

        #endregion
    }
}
using System;
using BuildsAppReborn.Contracts.Models.Base;

namespace BuildsAppReborn.Contracts.Models
{
    public interface ITestRun : IObjectItem, IWebItem
    {
        #region Public Properties

        Int32 Id { get; }

        Int32 IncompleteTests { get; }

        String Name { get; }

        Int32 PassedTests { get; }

        String State { get; }

        Int32 TotalTests { get; }

        Int32 UnanalyzedTests { get; }

        #endregion
    }
}
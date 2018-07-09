using System;
using BuildsAppReborn.Contracts.Models.Base;

namespace BuildsAppReborn.Contracts.Models
{
    public interface ITestRun : IObjectItem, IWebItem
    {
        Int32 FailedTests { get; }

        Int32 Id { get; }

        Int32 IncompleteTests { get; }

        String Name { get; }

        Int32 NotApplicableTests { get; }

        Int32 PassedTests { get; }

        String State { get; }

        Int32 TotalTests { get; }
    }
}
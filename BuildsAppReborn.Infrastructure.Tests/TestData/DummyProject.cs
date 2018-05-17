using System;
using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Infrastructure.Tests.TestData
{
    public class DummyProject : IProject
    {
        public String Description { get; set; }

        public String Id { get; set; }

        public String Name { get; set; }
        public String Url { get; }
    }
}
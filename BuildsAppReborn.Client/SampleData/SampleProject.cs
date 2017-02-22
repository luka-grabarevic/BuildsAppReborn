using System;
using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Client.SampleData
{
    internal class SampleProject : IProject
    {
        #region Implementation of IProject

        public String Description { get; set; }
        public String Id { get; set; }
        public String Name { get; set; }
        public String Url { get; }

        #endregion
    }
}
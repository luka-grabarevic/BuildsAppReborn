using System;
using System.ComponentModel.Composition;

namespace BuildsAppReborn.Contracts.Composition
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class BuildProviderExportAttribute : ExportAttribute, IBuildProviderMetadata
    {
        #region Constructors

        public BuildProviderExportAttribute(Type contractType, String guid, String name)
            : base(contractType)
        {
            Id = guid;
            Name = name;
        }

        #endregion

        #region Implementation of IBuildProviderMetadata

        public String Id { get; }

        public String Name { get; }

        #endregion
    }
}
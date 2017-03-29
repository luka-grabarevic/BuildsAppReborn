using System;
using System.ComponentModel.Composition;

namespace BuildsAppReborn.Contracts.Composition
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class IdentifierExportAttribute : ExportAttribute, IIdentifierMetadata
    {
        #region Constructors

        public IdentifierExportAttribute(Type contractType, String guid, String name = "")
            : base(contractType)
        {
            Id = guid;
            Name = name;
        }

        #endregion

        #region Implementation of IIdentifierMetadata

        public String Id { get; }

        public String Name { get; }

        #endregion
    }
}
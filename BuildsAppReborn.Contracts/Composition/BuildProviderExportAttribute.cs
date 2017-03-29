using System;
using System.ComponentModel.Composition;
using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Contracts.Composition
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class BuildProviderExportAttribute : ExportAttribute, IBuildProviderMetadata
    {
        #region Constructors

        public BuildProviderExportAttribute(Type contractType, String guid, String name, AuthenticationModes supportedAuthenticationModes)
            : base(contractType)
        {
            Id = guid;
            Name = name;
            SupportedAuthenticationModes = supportedAuthenticationModes;
        }

        #endregion

        #region Implementation of IBuildProviderMetadata

        public String Name { get; }

        public AuthenticationModes SupportedAuthenticationModes { get; }

        public String Id { get; }

        #endregion
    }
}
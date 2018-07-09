using System;
using System.ComponentModel.Composition;
using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Contracts.Composition
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class BuildProviderExportAttribute : ExportAttribute, IBuildProviderMetadata
    {
        public BuildProviderExportAttribute(Type contractType, String guid, String name, AuthenticationModes supportedAuthenticationModes)
            : base(contractType)
        {
            Id = guid;
            Name = name;
            SupportedAuthenticationModes = supportedAuthenticationModes;
        }

        public String Id { get; }

        public String Name { get; }

        public AuthenticationModes SupportedAuthenticationModes { get; }
    }
}
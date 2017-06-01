using System;
using System.ComponentModel.Composition;
using BuildsAppReborn.Access.Models;
using BuildsAppReborn.Contracts;
using BuildsAppReborn.Contracts.Composition;
using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Access
{
    [BuildProviderExport(typeof(IBuildProvider), Id, Name, AuthenticationModes.AccessToken)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class VstsBuildProvider : TfsBuildProviderBase<VstsBuild, VstsBuildDefinition, VstsUser, VstsSourceVersion, VstsArtifact>
    {
        #region Overrides of Base

        protected override String ApiVersion => "2.0";

        #endregion

        internal const String Id = "d1d878e8-7658-4eb0-b0bf-e6ab688e0b39";

        internal const String Name = "VSTS";
    }
}
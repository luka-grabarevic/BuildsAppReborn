using System;
using System.ComponentModel.Composition;
using BuildsAppReborn.Access.Models;
using BuildsAppReborn.Contracts;
using BuildsAppReborn.Contracts.Composition;
using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Access
{
    [BuildProviderExport(typeof(IBuildProvider), Id, Name, AuthenticationModes.Default | AuthenticationModes.AccessToken)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class Tfs2017BuildProvider : TfsBuildProviderBase<Tfs2017Build, Tfs2017BuildDefinition, Tfs2017User, Tfs2017SourceVersion, Tfs2017Artifact, Tfs2017TestRun>
    {
        #region Overrides of Base

        protected override String ApiVersion => "2.0";

        #endregion

        internal const String Id = "08e2efa9-2407-4f2e-a159-aa2b223abaac";

        internal const String Name = "TFS 2017";
    }
}
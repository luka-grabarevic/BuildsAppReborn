using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Contracts.Composition
{
    public interface IBuildProviderMetadata : IIdentifierMetadata
    {
        #region Public Properties

        AuthenticationModes SupportedAuthenticationModes { get; }

        #endregion
    }
}
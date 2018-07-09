using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Contracts.Composition
{
    public interface IBuildProviderMetadata : IIdentifierMetadata
    {
        AuthenticationModes SupportedAuthenticationModes { get; }
    }
}
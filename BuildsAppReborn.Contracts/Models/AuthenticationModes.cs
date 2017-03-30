using System;

namespace BuildsAppReborn.Contracts.Models
{
    [Flags]
    public enum AuthenticationModes
    {
        None = 0,
        Default = 1,
        UsernamePassword = 2,
        AccessToken = 4,
        OAuth = 8
    }
}
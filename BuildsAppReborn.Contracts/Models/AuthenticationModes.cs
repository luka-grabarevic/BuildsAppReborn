using System;

namespace BuildsAppReborn.Contracts.Models
{
    [Flags]
    public enum AuthenticationModes
    {
        Default = 0,
        UsernamePassword = 1,
        AccessToken = 2,
        OAuth = 4
    }
}
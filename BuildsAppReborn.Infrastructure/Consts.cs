using Newtonsoft.Json;

namespace BuildsAppReborn.Infrastructure
{
    public static class Consts
    {
        public static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, PreserveReferencesHandling = PreserveReferencesHandling.All };
    }
}
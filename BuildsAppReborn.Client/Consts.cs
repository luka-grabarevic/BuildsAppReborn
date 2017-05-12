using System;
using System.IO;

namespace BuildsAppReborn.Client
{
    public static class Consts
    {
        public const String ApplicationName = "BuildsAppReborn";

        public static readonly String ApplicationUserProfileFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ApplicationName);

        public static readonly String InstallationFolder = ApplicationUserProfileFolder;
    }
}
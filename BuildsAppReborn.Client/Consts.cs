using System;
using System.IO;

namespace BuildsAppReborn.Client
{
    public static class Consts
    {
        public static readonly String ApplicationUserProfileFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BuildsAppReborn");
    }
}
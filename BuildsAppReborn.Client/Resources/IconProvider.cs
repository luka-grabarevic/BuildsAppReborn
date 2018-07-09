using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using BuildsAppReborn.Contracts.Models;

namespace BuildsAppReborn.Client.Resources
{
    internal static class IconProvider
    {
        static IconProvider()
        {
            try
            {
                var iconResources = GetResourceNames().Where(a => a.Item1.EndsWith(".ico") || a.Item1.EndsWith(".png")).ToList();
                if (Directory.Exists(IconCacheFolder))
                {
                    Directory.Delete(IconCacheFolder, true);
                }

                Directory.CreateDirectory(IconCacheFolder);

                foreach (var iconResource in iconResources)
                {
                    try
                    {
                        var fileName = Path.GetFileName(iconResource.Item1);
                        if (fileName != null)
                        {
                            var filenamePath = Path.Combine(IconCacheFolder, fileName);
                            using (var resourceStream = iconResource.Item2)
                            {
                                using (var fileStream = File.Create(filenamePath, (Int32) resourceStream.Length))
                                {
                                    var bytesInStream = new Byte[resourceStream.Length];
                                    resourceStream.Read(bytesInStream, 0, bytesInStream.Length);
                                    fileStream.Write(bytesInStream, 0, bytesInStream.Length);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine(e);
                    }
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }

        public static String FailIcon => $"{IconsPath}{IcoPrefix}/failure.ico";

        public static String LoadingIcon => $"{IconsPath}{IcoPrefix}/loading.ico";

        public static String SettingsIcon => $"{IconsPath}{IcoPrefix}/settings.ico";

        public static String SuccessIcon => $"{IconsPath}{IcoPrefix}/succeeded.ico";

        public static String UnknownIcon => $"{IconsPath}{IcoPrefix}/partially_question.ico";

        public static String WarningIcon => $"{IconsPath}{IcoPrefix}/partially_exclamation.ico";

        public static String GetCachedIconPathForBuildStatus(BuildStatus status)
        {
            return Path.Combine(IconCacheFolder, GetIconFilename(status));
        }

        public static String GetIconForBuildStatus(BuildStatus status)
        {
            return $"{IconsPath}{IcoPrefix}/{GetIconFilename(status)}";
        }

        private static String GetIconFilename(BuildStatus status)
        {
            switch (status)
            {
                case BuildStatus.Succeeded:
                    return "succeeded.ico";
                case BuildStatus.Failed:
                    return "failure.ico";
                case BuildStatus.PartiallySucceeded:
                    return "partially_exclamation.ico";
                case BuildStatus.Running:
                case BuildStatus.Stopped:
                case BuildStatus.Queued:
                case BuildStatus.Unknown:
                default:
                    return "loading.ico"; // ToDo
            }
        }

        private static Tuple<String, Stream>[] GetResourceNames()
        {
            var asm = Assembly.GetEntryAssembly();
            var resName = asm.GetName().Name + ".g.resources";
            using (var stream = asm.GetManifestResourceStream(resName))
            {
                if (stream != null)
                {
                    using (var reader = new ResourceReader(stream))
                    {
                        return reader.Cast<DictionaryEntry>().Select(entry => new Tuple<String, Stream>((String) entry.Key, (Stream) entry.Value)).ToArray();
                    }
                }
            }

            return Enumerable.Empty<Tuple<String, Stream>>().ToArray();
        }

        private static readonly String IconCacheFolder = Path.Combine(Consts.ApplicationUserProfileFolder, "IconCache");

        private const String IcoPrefix = "ICO";

        private const String IconsPath = "pack://application:,,,/BuildsAppReborn.Client;component/Resources/Icons/";
    }
}
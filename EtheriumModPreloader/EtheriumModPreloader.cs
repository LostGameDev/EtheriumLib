using BepInEx;
using BepInEx.Logging;
using ICSharpCode.SharpZipLib.Zip;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace EtheriumLib
{
    public class EtheriumModPreloader
    {
        static EtheriumModPreloader()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                string name = new AssemblyName(args.Name).Name + ".dll";
                string resourceName = $"EtheriumModPreloader.Dependencies.{name}";

                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                        return null;

                    byte[] raw = new byte[stream.Length];
                    stream.Read(raw, 0, raw.Length);
                    return Assembly.Load(raw);
                }
            };
        }

        private static readonly ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("EtheriumModPreloader");

        // Where .etheriummod files are located
        private static readonly string ModsFolder = Paths.PluginPath;

        // Temp folder where extracted mods go
        private static readonly string TempModsFolder = Path.Combine(Paths.PluginPath, "_TempEtheriumMods");

        public static IEnumerable<string> TargetDLLs { get; } = new[] { "Assembly-CSharp.dll" };

        public static void Patch(AssemblyDefinition assembly)
        {
        }

        public static void Initialize()
        {
            CleanupTempMods();
            try
            {
                ExtractAllEtheriumMods();
            }
            catch (Exception ex)
            {
                Logger.LogError($"Failed to initialize EtheriumModPreloader: {ex}");
            }
        }

        private static void ExtractAllEtheriumMods()
        {
            if (!Directory.Exists(ModsFolder))
                return;

            if (!Directory.Exists(TempModsFolder))
                Directory.CreateDirectory(TempModsFolder);

            foreach (var file in Directory.GetFiles(ModsFolder, "*.etheriummod", SearchOption.TopDirectoryOnly))
            {
                Logger.LogInfo($"Extracting {Path.GetFileName(file)}...");

                string modTempFolder = Path.Combine(TempModsFolder, Path.GetFileNameWithoutExtension(file));
                if (!Directory.Exists(modTempFolder))
                    Directory.CreateDirectory(modTempFolder);

                using (FileStream fs = File.OpenRead(file))
                using (ZipFile zipFile = new ZipFile(fs))
                {
                    foreach (ZipEntry entry in zipFile)
                    {
                        if (!entry.IsFile)
                            continue;

                        string targetPath = Path.Combine(modTempFolder, entry.Name.Replace('/', Path.DirectorySeparatorChar));
                        string dir = Path.GetDirectoryName(targetPath);
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);

                        using (Stream zipStream = zipFile.GetInputStream(entry))
                        using (FileStream fileStream = File.Create(targetPath))
                        {
                            CopyStream(zipStream, fileStream);
                        }
                    }
                }

                Logger.LogInfo($"Extracted mod to: {modTempFolder}");
            }
        }

        private static void CleanupTempMods()
        {
            if (Directory.Exists(TempModsFolder))
            {
                try
                {
                    Directory.Delete(TempModsFolder, recursive: true);
                    Logger.LogInfo($"Deleted old temp mods folder: {TempModsFolder}");
                }
                catch (System.Exception ex)
                {
                    Logger.LogWarning($"Failed to delete temp mods folder: {ex}");
                }
            }
        }

        private static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8192];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                output.Write(buffer, 0, read);
        }
    }
}

using BepInEx;
using EtheriumLib.Debug;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace EtheriumLib.UI
{
    public static class ScaleformGFxUtils
    {
        private static readonly Dictionary<string, string> overrides = new(StringComparer.OrdinalIgnoreCase);

        public static void RegisterOverride(BaseUnityPlugin callerPlugin, string swfName, string assetsPath)
        {
            string modId = CallerUtils.GetPluginGUID(callerPlugin);

            string fullPath = Path.Combine(assetsPath, swfName);

            if (File.Exists(fullPath))
            {
                overrides[swfName] = fullPath;
                Plugin.Logger.LogInfo($"[ScaleformGFxUtils] Registered override for {modId}: {swfName} -> {fullPath}");
            }
            else
            {
                Plugin.Logger.LogWarning($"[ScaleformGFxUtils] Tried to register override but file not found: {fullPath}");
            }
        }

        public static bool TryGetOverride(string swfName, out string overridePath)
        {
            return overrides.TryGetValue(swfName, out overridePath);
        }
    }
}

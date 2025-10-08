using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;

namespace EtheriumLib;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("Etherium.exe")]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    // Hides a warning about Awake not being used (The IDE is wrong Awake IS used)
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members")]
    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded! Version: {MyPluginInfo.PLUGIN_VERSION}");

        // Initialize Harmony
        Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members")]
    private void OnApplicationQuit()
    {
        CleanupTempMods();
    }

    private void CleanupTempMods()
    {
        string tempModsFolder = Path.Combine(Paths.PluginPath, "_TempEtheriumMods");

        if (Directory.Exists(tempModsFolder))
        {
            try
            {
                Directory.Delete(tempModsFolder, recursive: true);
                Logger.LogInfo($"Deleted temp mods folder: {tempModsFolder}");
            }
            catch (System.Exception ex)
            {
                Logger.LogWarning($"Failed to delete temp mods folder: {ex}");
            }
        }
    }
}
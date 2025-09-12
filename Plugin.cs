using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace EtheriumLib;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("Etherium.exe")]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    // Config Values
    public static ConfigEntry<bool> configRemoveExtensionCost;

    // Hides a warning about Awake not being used (The IDE is wrong Awake IS used)
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members")]
    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded! Version: {MyPluginInfo.PLUGIN_VERSION}");
        CreateConfigs();

        // Initialize Harmony
        Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll();
    }

    private void CreateConfigs()
    {
        // Creates the config entries in the mods config file but only if the config value does not already exist
        // This is done so that already existing config entries dont get erased every time the game starts or when your mod updates
        if (!Config.TryGetEntry("General", "RemoveExtensionCost", out configRemoveExtensionCost))
        {
            configRemoveExtensionCost = Config.Bind("General", "RemoveExtensionCost", true,
                                                "Removes the Etherium cost from all extensions.");
        }
    }
}

[HarmonyPatch(typeof(Extension))]
public static class RemoveExtensionCost
{
    [HarmonyPostfix]
    [HarmonyPatch("getEtheriumCost")]
    public static void Extension_getEtheriumCost_Postfix(ref int __result)
    {
        // Only do anything if the config value is true
        if (Plugin.configRemoveExtensionCost.Value)
        {
            // Set the return of the original method to a value of 0 making every extension cost 0 Etherium
            __result = 0;

            /* 
             * Original Method:
             * public int getEtheriumCost()
             * {
             *     return this.i_etheriumCost;
             * }
             */
        }
    }
}
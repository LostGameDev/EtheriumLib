using System.IO;
using UnityEngine;
using BepInEx;
using EtheriumLib.Debug;

namespace EtheriumLib.Assets
{
    public static class AssetBundleUtils
    {
        public static AssetBundle LoadAssetBundle(BaseUnityPlugin callerPlugin, string bundleName, string assetsPath)
        {
            string modId = CallerUtils.GetPluginGUID(callerPlugin);
            string bundlePath = Path.Combine(assetsPath, bundleName);

            Plugin.Logger.LogInfo($"Loading AssetBundle: {bundleName}, for: {modId}");

            if (!File.Exists(bundlePath))
            {
                Plugin.Logger.LogError($"Failed to load {bundleName}, for: {modId} at {assetsPath}, {bundlePath} does not exist!");
                return null;
            }

            AssetBundle assetBundle = AssetBundle.CreateFromFile(bundlePath);
            if (assetBundle == null)
            {
                Plugin.Logger.LogError($"Failed to load {bundleName}, for: {modId} at {assetsPath}!");
                return null;
            }

            // Set the name via the extension
            assetBundle.SetName(bundleName);

            Plugin.Logger.LogInfo($"Loaded AssetBundle: {bundleName}, for: {modId}");
            return assetBundle;
        }

        public static GameObject LoadPrefabFromAssetBundle(BaseUnityPlugin callerPlugin, AssetBundle assetBundle, string prefabName)
        {
            string modId = CallerUtils.GetPluginGUID(callerPlugin);

            Plugin.Logger.LogInfo($"Loading Prefab: {prefabName}, for: {modId}, from {assetBundle.GetName()}");

            if (assetBundle == null)
            {
                Plugin.Logger.LogError($"Failed to load {prefabName}, for: {modId}, {assetBundle.GetName()} is null!");
                return null;
            }

            GameObject prefab = assetBundle.Load(prefabName) as GameObject;
            if (prefab == null)
            {
                Plugin.Logger.LogError($"Failed to load {prefabName}, for: {modId} from {assetBundle.GetName()}!");
                return null;
            }

            Plugin.Logger.LogInfo($"Loaded AssetBundle: {prefabName}, for: {modId}, from {assetBundle.GetName()}");

            return prefab;
        }
    }
}

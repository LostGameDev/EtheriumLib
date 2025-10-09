using System.IO;
using UnityEngine;
using BepInEx;
using EtheriumLib.Debug;

namespace EtheriumLib.Assets
{
	public static class AssetBundleUtils
	{
		/// <summary>
		/// Loads an AssetBundle
		/// </summary>
		/// <param name="bundleName">
		/// The name of the AssetBundle file
		/// </param>
		/// <param name="assetsPath">
		/// The full path to the AssetBundle file
		/// </param>
		/// <returns>
		/// Returns the loaded AssetBundle
		/// </returns>
		public static AssetBundle LoadAssetBundle(BaseUnityPlugin callerPlugin, string bundleName, string assetsPath)
		{
			string modId = CallerUtils.GetPluginGUID(callerPlugin);
			string bundlePath = Path.Combine(assetsPath, bundleName);

			if (Plugin.configDebugLogging.Value)
			{
				Plugin.Logger.LogInfo($"[AssetBundleUtils] Loading AssetBundle: {bundleName}, for: {modId}");
			}
			
			if (!File.Exists(bundlePath))
			{
				Plugin.Logger.LogError($"[AssetBundleUtils] Failed to load {bundleName}, for: {modId} at {assetsPath}, {bundlePath} does not exist!");
				return null;
			}

			AssetBundle assetBundle = AssetBundle.CreateFromFile(bundlePath);
			if (assetBundle == null)
			{
				Plugin.Logger.LogError($"[AssetBundleUtils] Failed to load {bundleName}, for: {modId} at {assetsPath}!");
				return null;
			}

			// Set the name via the extension
			assetBundle.SetName(bundleName);

			if (Plugin.configDebugLogging.Value)
			{
				Plugin.Logger.LogInfo($"[AssetBundleUtils] Loaded AssetBundle: {bundleName}, for: {modId}");
			}
			return assetBundle;
		}

		/// <summary>
		/// Loads a Prefab from an AssetBundle
		/// </summary>
		/// <param name="assetBundle">
		/// The AssetBundle to load from
		/// </param>
		/// <param name="prefabName">
		/// The name of the Prefab GameObject to load
		/// </param>
		/// <returns>
		/// Returns the loaded Prefab GameObject
		/// </returns>
		public static GameObject LoadPrefabFromAssetBundle(BaseUnityPlugin callerPlugin, AssetBundle assetBundle, string prefabName)
		{
			string modId = CallerUtils.GetPluginGUID(callerPlugin);

			if (Plugin.configDebugLogging.Value)
			{
				Plugin.Logger.LogInfo($"[AssetBundleUtils] Loading Prefab: {prefabName}, for: {modId}, from {assetBundle.GetName()}");
			}

			if (assetBundle == null)
			{
				Plugin.Logger.LogError($"[AssetBundleUtils] Failed to load {prefabName}, for: {modId}, {assetBundle.GetName()} is null!");
				return null;
			}

			GameObject prefab = assetBundle.Load(prefabName) as GameObject;
			if (prefab == null)
			{
				Plugin.Logger.LogError($"[AssetBundleUtils] Failed to load {prefabName}, for: {modId} from {assetBundle.GetName()}!");
				return null;
			}

			if (Plugin.configDebugLogging.Value)
			{
				Plugin.Logger.LogInfo($"[AssetBundleUtils] Loaded AssetBundle: {prefabName}, for: {modId}, from {assetBundle.GetName()}");
			}

			return prefab;
		}
	}
}

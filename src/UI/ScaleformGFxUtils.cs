using BepInEx;
using EtheriumLib.Debug;
using System;
using System.Collections.Generic;
using System.IO;

namespace EtheriumLib.UI
{
	public static class ScaleformGFxUtils
	{
		private static readonly Dictionary<string, string> overrides = new(StringComparer.OrdinalIgnoreCase);

		/// <summary>
		/// Registers an SWF file override to replace a vanilla SWF file with a custom one
		/// </summary>
		/// <param name="swfName">
		/// The name of the SWF file being replaced
		/// </param>
		/// <param name="swfReplacementPath">
		/// The full path of the custom SWF file
		/// </param>
		public static void RegisterOverride(BaseUnityPlugin callerPlugin, string swfName, string swfReplacementPath)
		{
			string modId = CallerUtils.GetPluginGUID(callerPlugin);

			if (File.Exists(swfReplacementPath))
			{
				overrides[swfName] = swfReplacementPath;
				if (Plugin.configDebugLogging.Value)
				{
					Plugin.Logger.LogInfo($"[ScaleformGFxUtils] Registered override for {modId}: {swfName} -> {swfReplacementPath}");
				}
			}
			else
			{
				Plugin.Logger.LogError($"[ScaleformGFxUtils] Tried to register override but file not found: {swfReplacementPath}");
			}
		}

		/// <summary>
		/// Gets the override path of a registered SWF file override
		/// </summary>
		/// <param name="swfName">
		/// The name of the SWF file that is being replaced
		/// </param>
		/// <param name="overridePath">
		/// The full path of the custom SWF file
		/// </param>
		/// <returns>
		/// Returns the full path of a registered SWF file override
		/// </returns>
		public static bool TryGetOverride(string swfName, out string overridePath)
		{
			return overrides.TryGetValue(swfName, out overridePath);
		}
	}
}

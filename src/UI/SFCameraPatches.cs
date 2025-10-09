using HarmonyLib;
using Scaleform;

namespace EtheriumLib.UI
{
    [HarmonyPatch(typeof(SFCamera))]
    public static class SFCameraPatches
    {
        [HarmonyPostfix]
        [HarmonyPatch("CreateMovieCreationParams", new[] { typeof(string), typeof(int) })]
        public static void SFCamera_CreateMovieCreationParams_Postfix(ref SFMovieCreationParams __result, ref string swfName)
        {
            if (ScaleformGFxUtils.TryGetOverride(swfName, out var overridePath))
            {
                __result.MovieName = overridePath;
                if (Plugin.configDebugLogging.Value)
                {
                    Plugin.Logger.LogInfo($"[SFCamera] Redirected SWF {swfName} -> {overridePath}");
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("CreateMovieCreationParams", new[] { typeof(string), typeof(int), typeof(UnityEngine.Color32), typeof(bool) })]
        public static void SFCamera_CreateMovieCreationParams_Extended_Postfix(ref SFMovieCreationParams __result, ref string swfName)
        {
            if (ScaleformGFxUtils.TryGetOverride(swfName, out var overridePath))
            {
                __result.MovieName = overridePath;
                if (Plugin.configDebugLogging.Value)
                {
                    Plugin.Logger.LogInfo($"[SFCamera] Redirected SWF {swfName} -> {overridePath}");
                }
            }
        }
    }
}

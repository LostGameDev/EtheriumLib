using BepInEx;

namespace EtheriumLib.Debug
{
    public static class CallerUtils
    {
        /// <summary>
        /// Returns the GUID of the plugin.
        /// </summary>
        public static string GetPluginGUID(BaseUnityPlugin plugin)
        {
            if (plugin == null || plugin.Info == null)
                return "Unknown";

            return plugin.Info.Metadata.GUID;
        }

        /// <summary>
        /// Returns the Name of the plugin.
        /// </summary>
        public static string GetPluginName(BaseUnityPlugin plugin)
        {
            if (plugin == null || plugin.Info == null)
                return "Unknown";

            return plugin.Info.Metadata.Name;
        }

        /// <summary>
        /// Returns the Version of the plugin.
        /// </summary>
        public static string GetPluginVersion(BaseUnityPlugin plugin)
        {
            if (plugin == null || plugin.Info == null)
                return "Unknown";

            return plugin.Info.Metadata.Version.ToString();
        }
    }
}

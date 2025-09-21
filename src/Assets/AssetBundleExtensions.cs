using System.Collections.Generic;
using UnityEngine;

namespace EtheriumLib.Assets
{
    public static class AssetBundleExtensions
    {
        private static readonly Dictionary<AssetBundle, string> _names = new();

        /// <summary>
        /// Sets the name of the AssetBundle.
        /// </summary>
        public static void SetName(this AssetBundle bundle, string name)
        {
            if (bundle == null) return;
            _names[bundle] = name;
        }

        /// <summary>
        /// Gets the name of the AssetBundle.
        /// </summary>
        public static string GetName(this AssetBundle bundle)
        {
            if (bundle == null) return "Unknown";
            return _names.TryGetValue(bundle, out var name) ? name : "Unknown";
        }
    }
}

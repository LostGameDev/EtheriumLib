using UnityEngine;
using System.Linq;

namespace EtheriumLib.Debug
{
    public class InspectorDebugUtils
    {
        public static void PrintHierarchy(GameObject parent = null, string indent = "", bool isLast = true, bool isRoot = true)
        {
            if (parent == null)
            {
                // Scene header
                string sceneName = Application.loadedLevelName;
                Plugin.Logger.LogInfo($"[Scene: {sceneName}]");

                // Collect all root objects
                Object[] allObjects = GameObject.FindObjectsOfType(typeof(GameObject));
                GameObject[] rootObjects = System.Array.FindAll(
                    System.Array.ConvertAll(allObjects, o => o as GameObject),
                    go => go != null && go.transform.parent == null
                );

                // Sort alphabetically by name
                rootObjects = rootObjects.OrderBy(go => go.name).ToArray();

                for (int i = 0; i < rootObjects.Length; i++)
                {
                    bool rootIsLast = (i == rootObjects.Length - 1);
                    PrintHierarchy(rootObjects[i], "", rootIsLast, false);
                }
                return;
            }

            // Print the root without branch characters
            if (isRoot)
            {
                Plugin.Logger.LogInfo(parent.name);
            }
            else
            {
                string branch = isLast ? "└── " : "├── ";
                Plugin.Logger.LogInfo(indent + branch + parent.name);
            }

            // Prepare indent for children
            string childIndent = indent + (isRoot ? "" : (isLast ? "    " : "│   "));

            // Sort children alphabetically before printing
            var children = Enumerable.Range(0, parent.transform.childCount)
                                     .Select(i => parent.transform.GetChild(i).gameObject)
                                     .OrderBy(go => go.name)
                                     .ToList();

            for (int i = 0; i < children.Count; i++)
            {
                bool childIsLast = (i == children.Count - 1);
                PrintHierarchy(children[i], childIndent, childIsLast, false);
            }
        }
    }
}

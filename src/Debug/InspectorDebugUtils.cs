using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EtheriumLib.Debug
{
	public class InspectorDebugUtils
	{
		/// <summary>
		/// Prints the object hierarchy of a scene, or the hierarchy of a specific GameObject
		/// </summary>
		/// <param name="parent">
		/// The GameObject to print the hierarchy of, if parent is null, prints the entire scene hierarchy
		/// </param>
		public static void PrintHierarchy(GameObject parent = null)
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

				// Sort alphabetically
				rootObjects = rootObjects.OrderBy(go => go.name).ToArray();

				// Print each root and its children
				for (int i = 0; i < rootObjects.Length; i++)
				{
					bool isLast = (i == rootObjects.Length - 1);
					PrintHierarchyRecursive(rootObjects[i], "", isLast, false);
				}
			}
			else
			{
				// Print hierarchy starting from this GameObject
				PrintHierarchyRecursive(parent, "", true, true);
			}
		}

		private static void PrintHierarchyRecursive(GameObject parent, string indent, bool isLast, bool isRoot)
		{
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

			// Sort children alphabetically
			var children = Enumerable.Range(0, parent.transform.childCount)
				.Select(i => parent.transform.GetChild(i).gameObject)
				.OrderBy(go => go.name)
				.ToList();

			for (int i = 0; i < children.Count; i++)
			{
				bool childIsLast = (i == children.Count - 1);
				PrintHierarchyRecursive(children[i], childIndent, childIsLast, false);
			}
		}

		/// <summary>
		/// Lists all the components of a specific GameObject
		/// </summary>
		/// <param name="gameObject">
		/// The GameObject to list the components of
		/// </param>
		public static void PrintComponents(GameObject gameObject)
		{
			if (gameObject == null)
			{
				Plugin.Logger.LogError("[InspectorDebugUtils] GameObject is null");
				return;
			}

			Component[] components = gameObject.GetComponents<Component>();
			if (components.Length == 0)
			{
				Plugin.Logger.LogInfo($"[InspectorDebugUtils] {gameObject.name} has no components");
				return;
			}

			Plugin.Logger.LogInfo($"Components of {gameObject.name}:");
			foreach (var comp in components)
			{
				Plugin.Logger.LogInfo($"- {comp.GetType().Name}");
			}
		}

		/// <summary>
		/// Prints all the information about a specific component from a specific GameObject
		/// </summary>
		/// <param name="component">
		/// The component to list the information of
		/// </param>
		public static void PrintComponentInfo(Component component, string indent = "-")
		{
			if (component == null)
			{
				Plugin.Logger.LogError("[InspectorDebugUtils] Component is null");
				return;
			}

			Type type = component.GetType();
			Plugin.Logger.LogInfo($"[InspectorDebugUtils] Component Info: {type.Name}");

			void PrintValue(string name, object value)
			{
				if (value == null) return;

				switch (value)
				{
					case Vector3 v3:
						Plugin.Logger.LogInfo($"{indent}  {name}: x={v3.x:F2}, y={v3.y:F2}, z={v3.z:F2}");
						break;
					case Vector2 v2:
						Plugin.Logger.LogInfo($"{indent}  {name}: x={v2.x:F2}, y={v2.y:F2}");
						break;
					case Quaternion q:
						Plugin.Logger.LogInfo($"{indent}  {name}: x={q.x:F2}, y={q.y:F2}, z={q.z:F2}, w={q.w:F2}");
						break;
					case Color c:
						Plugin.Logger.LogInfo($"{indent}  {name}: r={c.r:F2}, g={c.g:F2}, b={c.b:F2}, a={c.a:F2}");
						break;
					case Matrix4x4 m:
						string mat = "";
						for (int i = 0; i < 4; i++)
							mat += $"{indent}    {m.GetRow(i).x:F5}\t{m.GetRow(i).y:F5}\t{m.GetRow(i).z:F5}\t{m.GetRow(i).w:F5}\n";
						Plugin.Logger.LogInfo($"{indent}  {name}:\n{mat.TrimEnd()}");
						break;
					case GameObject go:
						Plugin.Logger.LogInfo($"{indent}  {name}: {go.name}");
						break;
					default:
						if (value is Component) return; // skip redundant Unity references
						Plugin.Logger.LogInfo($"{indent}  {name}: {value}");
						break;
				}
			}

			// Print public properties
			foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
			{
				try
				{
					object value = prop.GetValue(component, null);
					PrintValue(prop.Name, value);
				}
				catch { }
			}

			// Print public fields
			foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
			{
				try
				{
					object value = field.GetValue(component);
					PrintValue(field.Name, value);
				}
				catch { }
			}
		}
	}
}

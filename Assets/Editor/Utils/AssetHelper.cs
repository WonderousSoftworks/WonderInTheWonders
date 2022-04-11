using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor.Utils
{
    public static class AssetHelper
    {
        public static T GetAssetWithName<T>(string name, bool useFallback = false) where T : Object
        {
            List<string> paths = AssetDatabase.FindAssets($"t:{typeof(T)}")
                .Select(AssetDatabase.GUIDToAssetPath).ToList();

            if (paths.Find(p => p.EndsWith($"{name}.asset")) is { } path)
                return AssetDatabase.LoadAssetAtPath<T>(path);

            if (useFallback && paths.Count > 0)
                return AssetDatabase.LoadAssetAtPath<T>(paths[0]);

            return null;
        }

        public static string GetAssetPath(Object assetObject)
        {
            return AssetDatabase.GetAssetPath(assetObject);
        }
    }
}

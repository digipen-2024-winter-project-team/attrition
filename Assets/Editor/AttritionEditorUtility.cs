using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Attrition
{
    public static class AttritionEditorUtility
    {
        /// <summary>
        /// Aggregates relevant information related to a scene like GUID, name, path, and asset.
        /// </summary>
        public readonly struct SceneInfo
        {
            public SceneInfo(string guid)
            {
                this.guid = guid;
                path = AssetDatabase.GUIDToAssetPath(this.guid);
                asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
                name = asset.name;
            }

            public readonly string guid;
            public readonly string name;
            public readonly string path;
            public readonly SceneAsset asset;

            public static SceneInfo Construct(string guid) => new SceneInfo(guid);
        }

        /// <summary>
        /// Get SceneInfo objects for every scene in the project.
        /// </summary>
        /// <returns>An array of all SceneInfo objects.</returns>
        public static SceneInfo[] GetAllSceneInfo() =>
            AssetDatabase.FindAssets("t:scene")
                .Select(SceneInfo.Construct)
                .ToArray();

        /// <summary>
        /// Get all SceneInfo objects for a given array of scene names.
        /// </summary>
        /// <param name="sceneNames">The scene names to get SceneInfo objects for.</param>
        /// <returns>All SceneInfo objects for the array of scene names.</returns>
        public static SceneInfo[] GetAllSceneInfoForNames(this string[] sceneNames) =>
            GetAllSceneInfo()
                .Where(sceneInfo => sceneNames.Contains(sceneInfo.name))
                .ToArray();
        
        /// <summary>
        /// Converts a scene name to Scene Asset object.
        /// </summary>
        /// <param name="sceneName">The scene name to get the SceneAsset for.</param>
        /// <returns>The scene asset for the given scene name.</returns>
        public static SceneAsset StringToSceneAsset(this string sceneName) =>
            GetAllSceneInfo()
                .FirstOrDefault(sceneInfo => sceneInfo.name == sceneName)
                .asset;
    }
}

using System.Linq;
using Attrition.Debug_Scene_Selector;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Attrition.DebugSceneSelector.Editor
{
    [CustomEditor(typeof(SceneSelectionMenu))]
    public class SceneSelectionMenuEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            InspectorElement.FillDefaultInspector(root, serializedObject, this);
            
            root.Add(new Button(BuildScenes) 
            {
                text = "Add Scenes to Build Scenes",
            });

            void BuildScenes()
            {
                var buildScenes = EditorBuildSettings.scenes.ToList();

                var newScenes = ((SceneSelectionMenu)target).GetSceneNames()
                    .GetAllSceneInfoForNames()
                    .Where(sceneInfo => !buildScenes.Exists(buildScene => buildScene.path == sceneInfo.path))
                    .Select(sceneInfo => new EditorBuildSettingsScene(sceneInfo.path, true))
                    .ToList();
                
                buildScenes.AddRange(newScenes);

                EditorBuildSettings.scenes = buildScenes.ToArray();
            }
            
            return root;
        }
    }
}

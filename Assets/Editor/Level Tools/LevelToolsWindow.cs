using System;
using UnityEditor;
using UnityEngine;
using Attrition.Common;
using Attrition.CameraTriggers;

namespace Attrition.LevelTools
{
    public class LevelToolsWindow : EditorWindow
    {
        [MenuItem("Attrition/Level Tools")]
        private static void OpenWindow()
        {
            var window = CreateWindow<LevelToolsWindow>("Level Tools");
        }
        
        private void OnGUI()
        {
            EditorGUILayout.Space();
            
            using (var changeCheck = new EditorGUI.ChangeCheckScope())
            {
                var selection = (GizmoVisibility)EditorGUILayout.EnumPopup(new GUIContent("Camera Trigger Gizmo Visibility"), CameraTrigger.TriggerGizmoVisibility);

                if (changeCheck.changed)
                {
                    CameraTrigger.TriggerGizmoVisibility = selection;
                }
            }
        }
    }
}

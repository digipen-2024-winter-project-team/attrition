using System;
using UnityEditor;
using UnityEngine;
using Attrition.Common;
using Attrition.CameraTriggers;
using Attrition.DamageSystem;
using Attrition.PlayerCharacter;
using Unity.Cinemachine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Attrition.LevelTools
{
    public class LevelToolsWindow : EditorWindow
    {
        [MenuItem("Attrition/Level Tools")]
        private static void OpenWindow()
        {
            CreateWindow<LevelToolsWindow>("Level Tools");
        }

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange change)
        {
            if (change == PlayModeStateChange.EnteredPlayMode && EditorPrefs.GetBool(startWithFullBrightKey, false))
            {
                SpawnFullbrightLight();
            }
        }

        private const string startWithFullBrightKey = "StartWithFullbBright";
        private GameObject fullbrightLight;

        private void SpawnFullbrightLight()
        {
            fullbrightLight = new GameObject("Full bright");
            
            var light = fullbrightLight.AddComponent<Light>();
            light.intensity = 0.25f;
            light.type = LightType.Directional;
            
            light.transform.SetParent(FindAnyObjectByType<CinemachineBrain>(FindObjectsInactive.Exclude).transform);
            light.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
        
        private void OnGUI()
        {
            EditorGUILayout.Space();
            
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button("Reload Scene", GUILayout.Height(20)))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            GUI.enabled = true;

            EditorGUILayout.Space();
            
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button($"Full Bright: {(fullbrightLight != null ? "ON" : "OFF")}"))
            {
                if (fullbrightLight == null)
                {
                    SpawnFullbrightLight();
                }
                else
                {
                    Destroy(fullbrightLight);
                }
            }
            GUI.enabled = true;

            using (var changeCheck = new EditorGUI.ChangeCheckScope())
            {
                bool start = EditorGUILayout.Toggle("Full Bright on Play",
                    EditorPrefs.GetBool(startWithFullBrightKey, false));
                
                if (changeCheck.changed)
                {
                    EditorPrefs.SetBool(startWithFullBrightKey, start);
                }
            }
            
            EditorGUILayout.Space();
            
            using (var changeCheck = new EditorGUI.ChangeCheckScope())
            {
                var selection = (int)(GizmoVisibility)EditorGUILayout.EnumPopup(new GUIContent("Camera Triggers"),
                    (GizmoVisibility)EditorPrefs.GetInt(CameraTrigger.TriggerGizmoVisibilityKey));

                if (changeCheck.changed)
                {
                    EditorPrefs.SetInt(CameraTrigger.TriggerGizmoVisibilityKey, selection);
                }
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Player Take Damage"))
            {
                PlayerTakeDamage();
                
                void PlayerTakeDamage()
                {
                    var player = FindFirstObjectByType<Player>();

                    if (player == null) return;

                    player.GetComponent<Damageable>().TakeDamage(new(1, null, null));
                }
            }
        }
    }
}

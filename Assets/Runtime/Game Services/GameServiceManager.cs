using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Attrition.GameServices
{
    [CreateAssetMenu(fileName = GameServiceManagerResourceName, menuName = "Attrition/" + GameServiceManagerResourceName)]
    public class GameServiceManager : ScriptableObject
    {
        [SerializeField] private List<GameService> services;

        private const string GameServiceManagerResourceName = "Game Service Manager";

        private static MonoBehaviourCallbacks instance;
        public static MonoBehaviourCallbacks Instance => instance != null ? instance : SpawnInstance();

        public void AddService(GameService gameService) => services.Add(gameService);
        public bool ContainsService(GameService gameService) => services.Contains(gameService);
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static MonoBehaviourCallbacks SpawnInstance()
        {
            instance = new GameObject($"{GameServiceManagerResourceName} Instance").AddComponent<MonoBehaviourCallbacks>();
            DontDestroyOnLoad(instance);
            
            // load manager
            
            var manager = Resources.Load<GameServiceManager>(GameServiceManagerResourceName);

            if (manager == null)
            {
                Debug.LogWarning("No GameServiceManager was found in a resources folder!");
                return null;
            }

            // initialize services

            foreach (var service in manager.services)
                service.InitializeService();

            return instance;
        }
    }

    public class MonoBehaviourCallbacks : MonoBehaviour
    {
        public event Action Started;
        public event Action Updated;
        public event Action LateUpdated;
        public event Action FixedUpdated;
        public event Action<Scene, Scene> SceneChanged;
        public event Action ApplicationQuited;
        public event Action Destroyed;
        
        private void Awake() => SceneManager.activeSceneChanged += (from, to) => SceneChanged?.Invoke(from, to);
        private void Start() => Started?.Invoke();
        private void Update() => Updated?.Invoke();
        private void LateUpdate() => LateUpdated?.Invoke();
        private void FixedUpdate() => FixedUpdated?.Invoke();
        private void OnDestroy() => Destroyed?.Invoke();
        private void OnApplicationQuit() => ApplicationQuited?.Invoke();
    }
    
    public abstract class GameService : ScriptableObject
    {
        public void InitializeService()
        {
            CallbacksInstance.Started += Start;
            CallbacksInstance.Updated += Update;
            CallbacksInstance.LateUpdated += LateUpdate;
            CallbacksInstance.FixedUpdated += FixedUpdate;
            CallbacksInstance.SceneChanged += OnSceneChange;
            CallbacksInstance.Destroyed += InstanceDestroyed;
            CallbacksInstance.ApplicationQuited += OnApplicationQuit;
            
            Initialize();
        }

        private static MonoBehaviourCallbacks CallbacksInstance => GameServiceManager.Instance;
        
        protected static MonoBehaviour Instance => GameServiceManager.Instance;
        
        protected virtual void Initialize() { }
        protected virtual void Start() { }
        protected virtual void Update() { }
        protected virtual void LateUpdate() { }
        protected virtual void FixedUpdate() { }
        protected virtual void OnSceneChange(Scene from, Scene to) { }
        protected virtual void InstanceDestroyed() { }
        protected virtual void OnApplicationQuit() { }

        #region Editor
        #if UNITY_EDITOR

        // displayed in editor for easy access
        [SerializeField, HideInInspector] private GameServiceManager gameServiceManager;

        [CustomEditor(typeof(GameService), true)]
        protected class ServiceEditor : Editor
        {
            private const string
                NoManagersError       = "No Game Service Managers Found!",
                MultipleManagersError = "Multiple Game Service Managers Found!",
                AddToManagerButton    = "Add to Game Service Manager";

            private GameService GameService => target as GameService;

            private GameServiceManager FindManager(out string errorMessage)
            {
                var managerGUIDs = AssetDatabase.FindAssets($"t:{nameof(GameServiceManager)}");

                errorMessage = "";

                // already has manager
                if (GameService.gameServiceManager != null)
                {
                    return GameService.gameServiceManager;
                }
                
                else switch (managerGUIDs.Length)
                {
                    // no managers found
                    case 0:
                        errorMessage = NoManagersError;
                        return null;
                    
                    // multiple managers found
                    case > 1:
                        errorMessage = MultipleManagersError;
                        return null;
                }

                // find manager asset
                string managerPath = AssetDatabase.GUIDToAssetPath(managerGUIDs[0]);
                return AssetDatabase.LoadAssetAtPath<GameServiceManager>(managerPath);
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                EditorGUILayout.Space();

                var manager = FindManager(out var errorMessage);
                bool onManager = manager != null && manager.ContainsService(GameService);

                // display connected manager
                if (onManager)
                {
                    GUI.enabled = false;
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(gameServiceManager)));
                    GUI.enabled = true;
                }

                // error message
                else if (errorMessage != "")
                {
                    EditorGUILayout.HelpBox(errorMessage, MessageType.Error, true);
                }

                // prompt to add service to manager
                else if (!onManager && GUILayout.Button(AddToManagerButton))
                {
                    manager.AddService(GameService);
                    EditorUtility.SetDirty(manager);

                    GameService.gameServiceManager = manager;
                    EditorUtility.SetDirty(GameService);

                    AssetDatabase.SaveAssets();
                }
            }
        }

        #endif
        #endregion
    }
}

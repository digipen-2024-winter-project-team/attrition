using System;
using System.Linq;
using Attrition.Common;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Attrition.Debug_Scene_Selector
{
    public class SceneSelectionMenu : MonoBehaviour
    {
        [SerializeField] private SceneOption sceneOptionButtonPrefab;
        [SerializeField] private Transform optionsParent;
        [SceneAsset]
        [SerializeField] private string mainMenuScene;
        [SerializeField] private SceneOptionInfo[] sceneOptions;
        [SerializeField] private InputActionReference cancel;
        
        [Serializable]
        private struct SceneOptionInfo
        {
            [SceneAsset]
            public string scene;
            public string description;
        }

        public string[] GetSceneNames() => this.sceneOptions.Select(sceneOption => sceneOption.scene).ToArray();

        private void OnEnable()
        {
            cancel.action.performed += OnCancelPerformed;
        }

        private void OnDisable()
        {
            cancel.action.performed -= OnCancelPerformed;
        }

        private void OnCancelPerformed(InputAction.CallbackContext context)
        {
            ReturnToMain();
        }

        public void ReturnToMain()
        {
            SceneManager.LoadScene(this.mainMenuScene);
        }

        private void Start()
        {
            foreach (var sceneOptionInfo in this.sceneOptions)
            {
                var ui = Instantiate(this.sceneOptionButtonPrefab, this.optionsParent);
                ui.button.onClick.AddListener(() => SceneManager.LoadScene(sceneOptionInfo.scene));
                ui.sceneName.text = sceneOptionInfo.scene;
                ui.description.text = sceneOptionInfo.description;
            }
        }
    }
}

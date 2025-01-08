using System;
using System.Linq;
using Attrition.Common;
using UnityEngine;
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
        
        [Serializable]
        private struct SceneOptionInfo
        {
            [SceneAsset]
            public string scene;
            public string description;
        }

        public string[] GetSceneNames() => this.sceneOptions.Select(sceneOption => sceneOption.scene).ToArray(); 
        
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

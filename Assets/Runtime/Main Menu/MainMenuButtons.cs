using Attrition.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

namespace Attrition.Main_Menu
{
    public class MainMenuButtons : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuContent;
        [SerializeField] private GameObject playButton, settingsButton, creditsButton;
        [SerializeField] private GameObject settingsContent;
        [SerializeField] private GameObject settingsFirstSelected;
        [SerializeField] private GameObject creditsContent;
        [SerializeField] private GameObject creditsFirstSelected;
        [SceneAsset]
        [SerializeField] private string mainMenuScene;
        
        private void Start()
        {
            this.SetState(State.Main);
        }

        private State state;
        private enum State
        {
            Main,
            Credits,
            Settings,
        }

        private void Update()
        {
            var input =(InputSystemUIInputModule)EventSystem.current.currentInputModule;

            if (input.cancel.action.WasPerformedThisFrame()
                && this.state != State.Main)
            {
                this.SetState(State.Main);
            }
        }

        private void SetState(State state)
        {

            this.mainMenuContent.SetActive(false);
            this.creditsContent.SetActive(false);
            this.settingsContent.SetActive(false);

            var (content, select) = state switch
            {
                State.Credits => (this.creditsContent, this.creditsFirstSelected),
                State.Settings => (this.settingsContent, this.settingsFirstSelected),
                State.Main or _ => (this.mainMenuContent, this.state switch { State.Credits => this.creditsButton, State.Settings => this.settingsButton, _ => this.playButton }),
            };
            
            content.SetActive(true);
            EventSystem.current.SetSelectedGameObject(select);
            
            this.state = state;
        }
        
        public void PlayGame()
        {
            SceneManager.LoadScene(this.mainMenuScene);
        }

        public void OpenSettings()
        {
            this.SetState(State.Settings);
        }

        public void OpenCredits()
        {
            this.SetState(State.Credits);
        }

        public void OpenMain()
        {
            this.SetState(State.Main);
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}

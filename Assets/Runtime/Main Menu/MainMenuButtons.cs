using System;
using UnityEngine;
using Attrition.Common;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace Attrition.MainMenu
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
            SetState(State.Main);
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
                && state != State.Main)
            {
                SetState(State.Main);
            }
        }

        private void SetState(State state)
        {

            mainMenuContent.SetActive(false);
            creditsContent.SetActive(false);
            settingsContent.SetActive(false);

            var (content, select) = state switch
            {
                State.Credits => (creditsContent, creditsFirstSelected),
                State.Settings => (settingsContent, settingsFirstSelected),
                State.Main or _ => (mainMenuContent, this.state switch { State.Credits => creditsButton, State.Settings => settingsButton, _ => playButton }),
            };
            
            content.SetActive(true);
            EventSystem.current.SetSelectedGameObject(select);
            
            this.state = state;
        }
        
        public void PlayGame()
        {
            SceneManager.LoadScene(mainMenuScene);
        }

        public void OpenSettings()
        {
            SetState(State.Settings);
        }

        public void OpenCredits()
        {
            SetState(State.Credits);
        }

        public void OpenMain()
        {
            SetState(State.Main);
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}

using System;
using Attrition.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Attrition
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private InputActionReference pause;
        [SerializeField] private GameObject content;
        [SerializeField] private GameObject firstSelected;
        [SceneAsset]
        [SerializeField] private string mainMenuScene;
        [SerializeField] private GameObject quitConfirmationContent;
        [SerializeField] private GameObject quitConfirmationFirstSelected;
        [SerializeField] private GameObject quitButton;
        
        private bool paused;

        private void Start()
        {
            content.SetActive(false);
            quitConfirmationContent.SetActive(false);
        }

        private void OnEnable()
        {
            pause.action.performed += OnPaused;  
        }

        private void OnDisable()
        {
            pause.action.performed -= OnPaused;
        }

        private void OnPaused(InputAction.CallbackContext obj)
        {
            Pause(!paused);
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }

        public void Pause(bool pause)
        {
            paused = pause;

            if (paused)
            {
                Time.timeScale = 0;
                content.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                content.SetActive(false);
            }
        }

        public void QuitToMainMenu()
        {
            quitConfirmationContent.SetActive(true);
            EventSystem.current.SetSelectedGameObject(quitConfirmationFirstSelected);
        }

        public void QuitConfirm()
        {
            Pause(false);
            SceneManager.LoadScene(mainMenuScene);
        }

        public void QuitCancel()
        {
            quitConfirmationContent.SetActive(false);
            EventSystem.current.SetSelectedGameObject(quitButton);
        }
    }
}

using System;
using Attrition.Common;
using Attrition.Common.ScriptableVariables.DataTypes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

namespace Attrition.PauseMenu
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
        
        [SerializeField] private BoolVariable paused;
        [SerializeField] private BoolVariable playerDead;

        [SerializeField] private InputActionReference cancel;

        private void Start()
        {
            content.SetActive(false);
            quitConfirmationContent.SetActive(false);
            paused.Value = false;
        }

        private void OnEnable()
        {
            pause.action.performed += OnPaused;
            cancel.action.performed += OnCancelPerformed;
        }

        private void OnDisable()
        {
            pause.action.performed -= OnPaused;
            cancel.action.performed -= OnCancelPerformed;
        }

        private void OnCancelPerformed(InputAction.CallbackContext context)
        {
            if (quitConfirmationContent.activeSelf)
            {
                QuitCancel();
            }
        }

        private void OnPaused(InputAction.CallbackContext obj)
        {
            if (playerDead.Value)
            {
                return;
            }
            
            Pause(!paused.Value);
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }

        public void Pause(bool pause)
        {
            paused.Value = pause;

            if (paused.Value)
            {
                Time.timeScale = 0;
                content.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                quitConfirmationContent.SetActive(false);
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

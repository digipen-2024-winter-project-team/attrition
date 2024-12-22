using System;
using System.Collections;
using System.Collections.Generic;
using Attrition.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Attrition
{
    public class DeathMenu : MonoBehaviour
    {
        [SerializeField] private GameObject content;
        [SceneAsset]
        [SerializeField] private string mainMenuScene;
        [SerializeField] private float animationStartDelay;
        [SerializeField] private float animationItemDuration;
        [SerializeField] private List<GameObject> animationItems;
        [SerializeField] private GameObject firstSelected;

        private void Start()
        {
            content.SetActive(false);
        }

        public void PlayDeathScreenAnimation()
        {
            StartCoroutine(DeathScreenAnimation());
        }
        
        private IEnumerator DeathScreenAnimation()
        {
            content.SetActive(true);
            
            foreach (var item in animationItems)
            {
                item.SetActive(false);
            }

            yield return new WaitForSeconds(animationStartDelay);
            
            foreach (var item in animationItems)
            {
                item.SetActive(true);
                
                yield return new WaitForSeconds(animationItemDuration);
            }
            
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }

        public void ReturnToMain()
        {
            SceneManager.LoadScene(mainMenuScene);
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

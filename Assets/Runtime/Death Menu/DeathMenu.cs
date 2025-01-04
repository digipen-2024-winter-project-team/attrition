using System.Collections.Generic;
using Attrition.Common;
using Attrition.Common.ScriptableVariables.DataTypes;
using DG.Tweening;
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
        [SerializeField] private float fadeDuration;
        [SerializeField] private AnimationCurve fadeCurve;
        [SerializeField] private CanvasGroup fadeGroup;
        [SerializeField] private float animationStartDelay;
        [SerializeField] private float animationItemDuration;
        [SerializeField] private List<GameObject> animationItems;
        [SerializeField] private GameObject firstSelected;
        [SerializeField] private BoolVariable dead;
        
        private void Start()
        {
            content.SetActive(false);
            
        }

        private void OnEnable()
        {
            dead.ValueChanged += DeadOnValueChanged;
        }

        private void OnDisable()
        {
            dead.ValueChanged -= DeadOnValueChanged;
        }

        private void DeadOnValueChanged(ValueChangeArgs<bool> args)
        {
            if (args.From == false && args.To == true)
            {
                PlayDeathScreenAnimation();
            }
        }

        private void PlayDeathScreenAnimation()
        {
            Time.timeScale = 0;
            content.SetActive(true);
            
            foreach (var item in animationItems)
            {
                item.SetActive(false);
            }

            fadeGroup.alpha = 0;
            
            var fadeTween = DOTween.To(() => fadeGroup.alpha, value => fadeGroup.alpha = value, 1, fadeDuration)
                .SetEase(fadeCurve);
            
            var animationSequence = DOTween.Sequence()
                .Append(fadeTween)
                .AppendInterval(animationStartDelay);
            
            animationItems.ForEach(item => animationSequence
                .AppendCallback(() => item.SetActive(true))
                .AppendInterval(animationItemDuration));

            animationSequence
                .SetUpdate(true)
                .OnComplete(() => EventSystem.current.SetSelectedGameObject(firstSelected));
        }

        public void ReturnToMain()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(mainMenuScene);
        }

        public void Restart()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

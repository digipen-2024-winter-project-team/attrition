using System;
using System.Collections;
using Attrition.Common.DOTweenParameters;
using DG.Tweening;
using UnityEngine;

namespace Attrition.Character_Selection
{
    public class PanelTransitionBehaviour : MonoBehaviour
    {
        [SerializeField]
        private bool transitionInOnEnable;
        [SerializeField]
        private bool transitionOutOnDisable;
        [Header("Translation")]
        [SerializeField]
        private Vector2 translateInFrom;
        [SerializeField]
        private Vector2 translateOutTo;
        [SerializeField]
        private DOTweenParameters translateInAnimation;
        [SerializeField]
        private DOTweenParameters translateOutAnimation;
        private Vector2 startPosition;
        
        private void OnEnable()
        {
            this.startPosition = ((RectTransform)this.transform).anchoredPosition;
            
            if (this.transitionInOnEnable)
            {
                this.PlayTransitionIn();
            }
        }

        private void OnDisable()
        {
            if (this.transitionOutOnDisable)
            {
                this.PlayTransitionOut();
            }
        }

        public void PlayTransitionIn()
        {
            this.StopCurrentTransition();
            
            var rectTransform = (RectTransform)this.transform;
            var translateTween = rectTransform
                .DOAnchorPos(this.startPosition, this.translateInAnimation.Duration)
                .From(this.translateInFrom)
                .ApplyParameters(this.translateInAnimation)
                .SetTarget(this.transform);;
            
            DOTween.Sequence()
                .Append(translateTween);
        }

        public void PlayTransitionOut()
        {
            this.StopCurrentTransition();
            
            var rectTransform = (RectTransform)this.transform;
            var translateTween = rectTransform
                .DOAnchorPos(this.translateOutTo, this.translateOutAnimation.Duration)
                .ApplyParameters(this.translateOutAnimation)
                .SetTarget(this.transform);
            
            DOTween.Sequence()
                .Append(translateTween);
        }

        public void StopCurrentTransition()
        {
            DOTween.Pause(this.transform);
            this.transform.DOKill();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(this.transform.position, this.transform.position + (Vector3)this.translateInFrom);
            Gizmos.DrawWireSphere(this.transform.position + (Vector3)this.translateInFrom, 25f);
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(this.transform.position, this.transform.position + (Vector3)this.translateOutTo);
            Gizmos.DrawWireSphere(this.transform.position + (Vector3)this.translateOutTo, 25f);
        }
    }
}

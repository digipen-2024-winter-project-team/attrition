using System;
using Attrition.PlayerCharacter;
using UnityEngine;

namespace Attrition.Player_Character.Animation
{
    public class PlayerAnimationHandler : MonoBehaviour
    {
        private Animator animator;
        private PlayerMovement mover;

        private void Awake()
        {
            var firstEnabledAnimator = this.GetComponentInChildren<Animator>(false);
            this.SetAnimator(firstEnabledAnimator);

            this.mover = this.GetComponentInParent<PlayerMovement>();
        }

        private void OnEnable()
        {
            throw new NotImplementedException();
        }

        public void SetAnimator(Animator animator)
        {
            this.animator = animator;
        }

        private void Update()
        {
            if (this.animator == null || this.mover == null)
            {
                return;
            }
            
            var speedPercent = this.mover.DirectionalSpeedPercent;
            var speedForward = speedPercent.y;
            var speedRight = speedPercent.x;
            var isMoving = speedPercent.magnitude > 0f;
            
            this.animator.SetBool("IsMoving", isMoving);
            this.animator.SetFloat("SpeedForward", speedForward);
            this.animator.SetFloat("SpeedRight", speedRight);
        }
    }
}

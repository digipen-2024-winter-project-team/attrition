using System;
using Attrition.PlayerCharacter;
using UnityEditor.Animations;
using UnityEngine;

namespace Attrition.Player_Character.Animation
{
    public class PlayerAnimationHandler : MonoBehaviour
    {
        private PlayerMovement mover;
        private Animator animator;

        private void Awake()
        {
            this.SetAnimator(this.GetComponent<Animator>());
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

            var speed = this.mover.SpeedPercent;
            var isMoving = speed > 0f;
            
            this.animator.SetBool("IsMoving", isMoving);
            this.animator.SetFloat("Speed", speed);
        }
    }
}

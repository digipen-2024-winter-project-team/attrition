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

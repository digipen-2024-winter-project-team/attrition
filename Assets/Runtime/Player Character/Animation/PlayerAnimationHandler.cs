using System;
using Attrition.Common;
using Attrition.PlayerCharacter;
using UnityEngine;

namespace Attrition.Player_Character.Animation
{
    public class PlayerAnimationHandler : MonoBehaviour
    {
        private Animator animator;
        private PlayerMovement mover;
        private PlayerAttack attacker;

        private void Awake()
        {
            var firstEnabledAnimator = this.GetComponentInChildren<Animator>(false);
            this.SetAnimator(firstEnabledAnimator);

            this.mover = this.GetComponentInParent<PlayerMovement>();
            this.attacker = this.GetComponentInParent<PlayerAttack>();
        }

        private void OnEnable()
        {
            mover.MoveStateChanged.Invoked += OnMoveStateChanged;
            attacker.Attacked.Invoked += OnAttack;
        }

        private void OnDisable()
        {
            mover.MoveStateChanged.Invoked -= OnMoveStateChanged;
            attacker.Attacked.Invoked -= OnAttack;
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

        private void OnMoveStateChanged(ValueChangeArgs<PlayerMovement.MoveState> args)
        {
            if (args.To == PlayerMovement.MoveState.Dodging)
            {
                this.animator.SetTrigger("Dodging");
            }
        }
        
        private void OnAttack()
        {
            this.animator.SetTrigger("Attacking");
        }
    }
}

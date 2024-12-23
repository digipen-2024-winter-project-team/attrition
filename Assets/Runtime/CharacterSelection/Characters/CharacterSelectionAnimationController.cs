using UnityEngine;

namespace Attrition.CharacterSelection.Characters
{
    public class CharacterSelectionAnimationController
    {
        private static readonly int IsSittingDown = Animator.StringToHash("IsSittingDown");
        private static readonly int IsStandingUp = Animator.StringToHash("IsStandingUp");
        
        private readonly Animator animator;

        public CharacterSelectionAnimationController(Animator animator)
        {
            this.animator = animator;
        }

        public void PlayStandUpAnimation()
        {
            this.animator.SetBool(IsSittingDown, false);
            this.animator.SetBool(IsStandingUp, true);
        }

        public void PlaySitDownAnimation()
        {
            this.animator.SetBool(IsStandingUp, false);
            this.animator.SetBool(IsSittingDown, true);
        }
    }
}

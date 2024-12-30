using Attrition.CharacterClasses;
using Unity.Cinemachine;
using UnityEngine;

namespace Attrition.CharacterSelection.Characters
{
    public class CharacterSelectionCharacterBehaviour : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private CinemachineCamera inspectCamera;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private Transform cycleFollowTarget;
        private CharacterClassBehaviour classBehaviour;

        private CharacterSelectionAnimationController selectionAnimationController;
        private CharacterSelectionCameraFocusController selectionCameraController;

        public Transform CycleFollowTarget => this.cycleFollowTarget;

        private void Awake()
        {
            this.selectionAnimationController = new(this.animator);
            this.selectionCameraController = new(this.inspectCamera);
        }

        public void Inspect()
        {
            this.selectionAnimationController.PlayStandUpAnimation();
            this.selectionCameraController.Focus();
        }

        public void Uninspect()
        {
            this.selectionAnimationController.PlaySitDownAnimation();
            this.selectionCameraController.Unfocus();
        }

        public void SetAnimator(Animator animator)
        {
            this.animator = animator;
            this.selectionAnimationController = new(animator);
        }
    }
}

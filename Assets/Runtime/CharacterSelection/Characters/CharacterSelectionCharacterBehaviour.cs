using Attrition.Common;
using Unity.Cinemachine;
using UnityEngine;

namespace Attrition.CharacterSelection.Characters
{
    public class CharacterSelectionCharacterBehaviour : MonoBehaviour
    {
        [SerializeField]
        private CinemachineCamera focusCamera;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private Transform browseFollowTarget;
        private CharacterClassBehaviour classBehaviour;

        private CharacterSelectionAnimationController selectionAnimationController;
        private CharacterSelectionCameraFocusController selectionCameraController;

        private void Awake()
        {
            this.selectionAnimationController = new(this.animator);
            this.selectionCameraController = new(this.focusCamera);
        }
        
        public Transform BrowseFollowTarget => this.browseFollowTarget;

        public void Focus()
        {
            this.selectionAnimationController.PlayStandUpAnimation();
            this.selectionCameraController.Focus();
        }

        public void Unfocus()
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

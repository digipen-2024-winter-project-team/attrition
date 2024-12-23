using Attrition.Common;
using Unity.Cinemachine;
using UnityEngine;

namespace Attrition.CharacterSelection
{
    public class SelectableCharacterBehaviour : MonoBehaviour
    {
        [SerializeField]
        private CinemachineCamera focusCamera;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private Transform browseFollowTarget;
        private CharacterClassBehaviour classBehaviour;

        private SelectableCharacterAnimationController animationController;
        private SelectableCharacterCameraFocusController cameraController;

        private void Awake()
        {
            this.animationController = new(this.animator);
            this.cameraController = new(this.focusCamera);
        }
        
        public Transform BrowseFollowTarget => this.browseFollowTarget;

        public void Focus()
        {
            this.animationController.PlayStandUpAnimation();
            this.cameraController.Focus();
        }

        public void Unfocus()
        {
            this.animationController.PlaySitDownAnimation();
            this.cameraController.Unfocus();
        }

        public void SetAnimator(Animator animator)
        {
            this.animator = animator;
            this.animationController = new(animator);
        }
    }
}

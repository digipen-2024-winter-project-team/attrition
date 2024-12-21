using Unity.Cinemachine;
using UnityEngine;

namespace Attrition.Character_Selection
{
    public class SelectableCharacterBehaviour : MonoBehaviour
    {
        [SerializeField]
        private CinemachineCamera focusCamera;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private Transform browseFollowTarget;
        [SerializeField]
        private CharacterIdentity identity;

        public CharacterClass CharacterClass
        {
            get => this.identity.CharacterClass;
            set => this.identity.SetCharacterClass(value);
        }
        public Transform BrowseFollowTarget => this.browseFollowTarget;

        public void Focus()
        {
            // Play stand-up animation
            this.animator.SetBool("IsSittingDown", false);
            this.animator.SetBool("IsStandingUp", true);
            
            // Focus camera
            this.focusCamera.gameObject.SetActive(true);
        }
        
        public void Unfocus()
        {
            // Play sit down animation
            this.animator.SetBool("IsStandingUp", false);
            this.animator.SetBool("IsSittingDown", true);
            
            // Unfocus camera
            this.focusCamera.gameObject.SetActive(false);
        }

        public void SetAnimator(Animator animator)
        {
            this.animator = animator;
        }
    }
}
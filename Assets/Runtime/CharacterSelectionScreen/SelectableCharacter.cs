using Unity.Cinemachine;
using UnityEngine;

namespace Attrition.CharacterSelectionScreen
{
    public class SelectableCharacter : MonoBehaviour
    {
        [SerializeField]
        private CinemachineCamera closeUpCamera;
        [SerializeField]
        private Animator animator;
        
        public void EnterCloseUp()
        {
            this.closeUpCamera.gameObject.SetActive(true);
            this.animator.SetBool("IsReady", true);
        }
        
        public void ExitCloseUp()
        {
            this.closeUpCamera.gameObject.SetActive(false);
            this.animator.SetBool("IsReady", false);
        }
    }
}

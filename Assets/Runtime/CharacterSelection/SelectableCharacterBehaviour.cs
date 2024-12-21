﻿using Unity.Cinemachine;
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
        [SerializeField]
        private CharacterClass characterClass;

        public CharacterClass CharacterClass
        {
            get => this.characterClass;
            private set => this.characterClass = value;
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
    }
}
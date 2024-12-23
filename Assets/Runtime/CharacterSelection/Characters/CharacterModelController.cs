using System;
using System.Collections.Generic;
using System.Linq;
using Attrition.CharacterClasses;
using Attrition.Common;
using UnityEngine;

namespace Attrition.CharacterSelection.Characters
{
    [RequireComponent(typeof(CharacterClassBehaviour))]
    public class CharacterModelController : MonoBehaviour
    {
        [Serializable]
        private struct ModelKeyValuePair
        {
            public CharacterClass Class;
            public Transform Transform;
        }
        
        [SerializeField]
        private Transform modelContainer;
        [SerializeField]
        private List<ModelKeyValuePair> models;
        private CharacterClassBehaviour characterClassBehaviour;
        private SelectableCharacterBehaviour character;
        
        private void Awake()
        {
            this.GetDependencies();
        }

        private void Reset()
        {
            this.GetDependencies();
        }

        private void OnEnable()
        {
            this.UpdateCharacterModel();
        }

        public void UpdateCharacterModel()
        {
            foreach (Transform child in this.modelContainer)
            {
                child.gameObject.SetActive(false);
            }

            if (this.characterClassBehaviour == null)
            {
                this.GetDependencies();

                if (this.characterClassBehaviour == null)
                {
                    return;
                }
            }
            
            var characterClass = this.characterClassBehaviour.CharacterClass;
            
            if (characterClass == null)
            {
                return;
            }

            var model = this.models
                .Where(kvp => kvp.Class == this.characterClassBehaviour.CharacterClass)
                .Select(kvp => kvp.Transform)
                .FirstOrDefault();
            
            if (model != null)
            {
                model.gameObject.SetActive(true);

                if (model.TryGetComponent(out Animator animator))
                {
                    this.character.SetAnimator(animator);
                }
            }
        }
        
        private void GetDependencies()
        {
            if (this.characterClassBehaviour == null)
            {
                this.characterClassBehaviour = this.GetComponent<CharacterClassBehaviour>();
            }

            if (this.character == null)
            {
                this.character = this.GetComponent<SelectableCharacterBehaviour>();
            }
        }
    }
}

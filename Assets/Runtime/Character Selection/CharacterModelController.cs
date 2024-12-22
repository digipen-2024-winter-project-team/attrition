using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Attrition.Character_Selection
{
    [RequireComponent(typeof(Identity))]
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
        private Identity identity;
        private SelectableCharacterBehaviour character;
        
        private void Awake()
        {
            this.ResolveDependencies();
        }

        private void Reset()
        {
            this.ResolveDependencies();
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

            if (this.identity == null)
            {
                this.ResolveDependencies();

                if (this.identity == null)
                {
                    return;
                }
            }
            
            var characterClass = this.identity.CharacterClass;
            
            if (characterClass == null)
            {
                return;
            }

            var model = this.models
                .Where(kvp => kvp.Class == this.identity.CharacterClass)
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
        
        private void ResolveDependencies()
        {
            if (this.identity == null)
            {
                this.identity = this.GetComponent<Identity>();
            }

            if (this.character == null)
            {
                this.character = this.GetComponent<SelectableCharacterBehaviour>();
            }
        }
    }
}

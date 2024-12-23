using System;
using System.Collections.Generic;
using System.Linq;
using Attrition.CharacterClasses;
using Attrition.CharacterSelection.Characters;
using UnityEngine;

namespace Attrition.Common
{
    [RequireComponent(typeof(CharacterClassBehaviour))]
    public class CharacterClassModelController : MonoBehaviour
    {
        [Serializable]
        private class ClassModelData
        {
            public CharacterClass Class;
            public Animator Animator;
        }

        [SerializeField]
        private Transform modelContainer;
        [SerializeField]
        private List<ClassModelData> models;

        private CharacterClassBehaviour characterClassBehaviour;
        private CharacterSelectionCharacterBehaviour characterSelectionCharacter;

        private void Awake() => this.Initialize();

        private void Reset() => this.Initialize();

        private void OnEnable() => this.UpdateCharacterModel();

        public void UpdateCharacterModel()
        {
            if (!this.GetDependencies()) return;

            this.DisableAllModels();

            var activeModel = this.FindModelForClass(this.characterClassBehaviour.CharacterClass);
            if (activeModel == null) return;

            this.EnableModel(activeModel);
        }

        private void Initialize()
        {
            this.characterClassBehaviour = this.GetComponent<CharacterClassBehaviour>();
            this.characterSelectionCharacter = this.GetComponent<CharacterSelectionCharacterBehaviour>();
        }

        private ClassModelData FindModelForClass(CharacterClass characterClass)
        {
            return characterClass == null
                ? null
                : this.models.FirstOrDefault(kvp => kvp.Class == characterClass);
        }

        private void EnableModel(ClassModelData classModelKvp)
        {
            classModelKvp.Animator.gameObject.SetActive(true);
            
            if (classModelKvp.Animator != null)
            {
                this.characterSelectionCharacter.SetAnimator(classModelKvp.Animator);
            }
        }

        private void DisableAllModels()
        {
            foreach (Transform child in this.modelContainer)
            {
                child.gameObject.SetActive(false);
            }
        }

        private bool GetDependencies()
        {
            if (this.characterClassBehaviour == null || this.characterSelectionCharacter == null)
            {
                this.Initialize();
            }

            return this.characterClassBehaviour != null && this.characterSelectionCharacter != null;
        }
    }
}

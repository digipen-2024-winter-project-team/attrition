using System;
using System.Collections.Generic;
using Attrition.CharacterClasses;
using Attrition.Common.SerializedEvents;
using UnityEngine;

namespace Attrition.ClassCharacterModel
{
    public class CharacterClassModelBehaviour : MonoBehaviour
    {
        [Serializable]
        public class ClassModelData
        {
            public CharacterClass Class;
            public Animator Animator;
        }

        [SerializeField]
        private Transform modelContainer;
        [SerializeField]
        private List<ClassModelData> models;
        [SerializeField]
        private SerializedEvent<Animator> modelUpdated;
        private CharacterClassModelController modelController;
        
        public IReadOnlySerializedEvent<Animator> ModelUpdated => this.modelUpdated;

        private void Awake()
        {
            this.modelController = new(this.modelContainer, this.models, this.modelUpdated);
        }

        /// <summary>
        /// Sets the character model based on the given character class.
        /// </summary>
        /// <param name="characterClass">The character class to activate the corresponding model.</param>
        public void SetCharacterModelByClass(CharacterClass characterClass)
        {
            this.modelController.UpdateActiveModel(characterClass);
        }
    }
}

using System;
using System.Collections.Generic;
using Attrition.CharacterClasses;
using Attrition.Common;
using Attrition.Common.SerializedEvents;
using UnityEngine;

namespace Attrition.ClassCharacterModel
{
    [DefaultExecutionOrder(-501)]
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
        private CharacterClassBehaviour classBehaviour;

        public IReadOnlySerializedEvent<Animator> ModelUpdated => this.modelUpdated;

        private void Awake()
        {
            this.modelController = new(this.modelContainer, this.models, this.modelUpdated);
            this.classBehaviour = this.GetComponentInParent<CharacterClassBehaviour>();
        }

        private void OnEnable()
        {
            if (this.classBehaviour != null)
            {
                if (this.classBehaviour.ClassChanged != null)
                {
                    this.classBehaviour.ClassChanged.Invoked += this.OnClassChanged;
                }
                else
                {
                    Debug.LogError($"No {nameof(this.classBehaviour)} {nameof(this.classBehaviour.ClassChanged)} event found for {this.name}.");
                }
                
                this.SetCharacterModelByClass(this.classBehaviour.CharacterClass);   
            }
            else
            {
                Debug.LogError($"No {nameof(this.classBehaviour)} found for {this.gameObject.name}.");
            }
        }

        private void OnDisable()
        {
            if (this.classBehaviour != null && this.classBehaviour.ClassChanged != null)
            {
                this.classBehaviour.ClassChanged.Invoked -= this.OnClassChanged;
            }
        }
        
        /// <summary>
        /// Sets the character model based on the given character class.
        /// </summary>
        /// <param name="characterClass">The character class to activate the corresponding model.</param>
        public void SetCharacterModelByClass(CharacterClass characterClass)
        {
            this.modelController.UpdateActiveModel(characterClass);
        }
        
        private void OnClassChanged(ValueChangeArgs<CharacterClass> args)
        {
            this.SetCharacterModelByClass(args.To);
        }
    }
}

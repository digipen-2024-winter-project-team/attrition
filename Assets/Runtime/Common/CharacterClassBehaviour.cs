using System;
using Attrition.CharacterClasses;
using Attrition.Common.ScriptableVariables.DataTypes;
using UnityEngine;

namespace Attrition.Common
{
    public class CharacterClassBehaviour : MonoBehaviour
    {
        [SerializeField]
        private CharacterClassVariable characterClass;
        
        public CharacterClass CharacterClass
        {
            get => this.characterClass.Value;
            set => this.characterClass.Value = value;
        }

        private void Awake()
        {
            this.characterClass = ScriptableObject.CreateInstance<CharacterClassVariable>();
        }
    }
}

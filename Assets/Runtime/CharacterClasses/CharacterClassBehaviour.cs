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
            get
            {
                if (this.characterClass == null)
                {
                    return null;
                }
                
                return this.characterClass.Value;
            }
            set
            {
                if (this.characterClass != null)
                {
                    this.characterClass.Value = value;
                }
            }
        }

        private void Awake()
        {
            this.characterClass ??= ScriptableObject.CreateInstance<CharacterClassVariable>();
        }
    }
}

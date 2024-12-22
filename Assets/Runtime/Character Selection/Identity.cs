using System;
using Attrition.Common.ScriptableVariables.DataTypes;
using Attrition.Name_Generation;
using UnityEngine;

namespace Attrition.Character_Selection
{
    public class Identity : MonoBehaviour
    {
        [SerializeField]
        private StringVariable displayName;
        [SerializeField]
        private CharacterClassVariable characterClass;
        [SerializeField]
        private NameData nameData;
        
        public string DisplayName
        {
            get => this.displayName.Value;
            private set => this.displayName.Value = value;
        }
        
        public CharacterClass CharacterClass
        {
            get => this.characterClass.Value;
            private set => this.characterClass.Value = value;
        }

        private void OnEnable()
        {
            var nameGenerator = new NameGenerator(this.nameData, DateTime.Now.Millisecond + this.GetInstanceID());
            this.SetDisplayName(nameGenerator.GenerateName());
        }

        public void SetDisplayName(string displayName)
        {
            this.DisplayName = displayName;
        }
        
        public void SetCharacterClass(CharacterClass characterClass)
        {
            this.CharacterClass = characterClass;
        }
    }
}

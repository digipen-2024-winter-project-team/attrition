using System;
using Attrition.NameGeneration;
using UnityEngine;

namespace Attrition.CharacterSelection
{
    public class CharacterIdentity : MonoBehaviour
    {
        [SerializeField]
        private string displayName;
        [SerializeField]
        private CharacterClass characterClass;
        [SerializeField]
        private NameData nameData;
        
        public string DisplayName
        {
            get => this.displayName;
            private set => this.displayName = value;
        }
        public CharacterClass CharacterClass { get; private set; }

        private void OnEnable()
        {
            var nameGenerator = new NameGenerator(this.nameData, DateTime.Now.Millisecond);
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

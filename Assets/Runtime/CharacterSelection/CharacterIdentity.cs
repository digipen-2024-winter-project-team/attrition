using UnityEngine;

namespace Attrition.CharacterSelection
{
    public class CharacterIdentity : MonoBehaviour
    {
        public string DisplayName { get; private set; }
        public CharacterClass CharacterClass { get; private set; }

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

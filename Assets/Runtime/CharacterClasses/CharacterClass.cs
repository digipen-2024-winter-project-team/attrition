using UnityEngine;

namespace Attrition.CharacterClasses
{
    [CreateAssetMenu(menuName = "Scriptables/Character Class")]
    public class CharacterClass : ScriptableObject
    {
        [SerializeField]
        private string displayName;

        public string DisplayName
        {
            get => this.displayName;
            private set => this.displayName = value;
        }
    }
}

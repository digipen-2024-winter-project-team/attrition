using Attrition.Common;
using Attrition.Common.ScriptableVariables.DataTypes;
using Attrition.Common.SerializedEvents;
using UnityEngine;

namespace Attrition.CharacterClasses
{
    public class CharacterClassBehaviour : MonoBehaviour
    {
        [SerializeField]
        private CharacterClassVariable characterClass;
        [SerializeField]
        private SerializedEvent<ValueChangeArgs<CharacterClass>> classChanging;
        [SerializeField]
        private SerializedEvent<ValueChangeArgs<CharacterClass>> classChanged;
        
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
        }

        public IReadOnlySerializedEvent<ValueChangeArgs<CharacterClass>> ClassChanging => this.classChanged;
        public IReadOnlySerializedEvent<ValueChangeArgs<CharacterClass>> ClassChanged => this.classChanged;

        private void Awake()
        {
            this.characterClass ??= ScriptableObject.CreateInstance<CharacterClassVariable>();
        }

        public void SetClass(CharacterClass characterClass)
        {
            var args = new ValueChangeArgs<CharacterClass>()
            {
                From = this.CharacterClass,
                To = characterClass,
            };

            this.classChanging.Invoke(args);
            this.characterClass.Value = characterClass;
            this.classChanged.Invoke(args);
        }
        
        public void SetClassVariable(CharacterClassVariable variable)
        {
            var args = new ValueChangeArgs<CharacterClass>()
            {
                From = this.CharacterClass,
                To = variable?.Value,
            };
            
            this.classChanging.Invoke(args);
            this.characterClass = variable;
            this.classChanged.Invoke(args);
        }
    }
}

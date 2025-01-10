using Attrition.Common;
using Attrition.Common.Events.SerializedEvents;
using Attrition.Common.ScriptableVariables.DataTypes;
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
        private bool wasTemporaryScriptableObjectCreated;
        
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

        private void OnEnable()
        {
            this.SetClass(this.characterClass.Value);
        }

        private void OnDestroy()
        {
            if (this.wasTemporaryScriptableObjectCreated) 
            {
                Destroy(this.characterClass);
            }
        }

        public void SetClass(CharacterClass characterClass)
        {
            if (this.characterClass == null)
            {
                var variable = ScriptableObject.CreateInstance<CharacterClassVariable>();
                variable.Value = characterClass;
                this.SetClassVariable(variable);
                this.wasTemporaryScriptableObjectCreated = true;
            }
            
            this.DoSetClass(this.CharacterClass, characterClass);
        }
        
        public void SetClassVariable(CharacterClassVariable variable)
        {
            var args = new ValueChangeArgs<CharacterClass>()
            {
                From = this.CharacterClass,
                To = variable?.Value,
            };
            
            this.characterClass = variable;
            this.DoSetClass(this.CharacterClass, args.To);
        }
        
        private void DoSetClass(CharacterClass from, CharacterClass to)
        {
            var args = new ValueChangeArgs<CharacterClass>()
            {
                From = from,
                To = to,
            };

            this.classChanging.Invoke(args);
            this.characterClass.Value = to;
            this.classChanged.Invoke(args);
        }
    }
}

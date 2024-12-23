using Attrition.Common;
using Attrition.Common.ScriptableVariables.DataTypes;
using Attrition.Names;
using UnityEngine;

namespace Attrition.CharacterSelection
{
    public class CharacterSelectionApplier : MonoBehaviour
    {
        [SerializeField]
        private StringVariable playerName;
        [SerializeField]
        private CharacterClassVariable playerClass;

        private void Awake()
        {
            this.playerName ??= ScriptableObject.CreateInstance<StringVariable>();
            this.playerClass ??= ScriptableObject.CreateInstance<CharacterClassVariable>();
        }

        public void Apply(GameObject selectedCharacter)
        {
            if (selectedCharacter.TryGetComponent(out NameBehaviour nameBehaviour))
            {
                this.playerName.Value = nameBehaviour.DisplayName;
            }
            
            if (selectedCharacter.TryGetComponent(out CharacterClassBehaviour classBehaviour))
            {
                this.playerClass.Value = classBehaviour.CharacterClass;
            }
        }
    }
}

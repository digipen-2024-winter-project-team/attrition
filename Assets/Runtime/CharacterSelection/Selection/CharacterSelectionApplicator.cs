using Attrition.CharacterClasses;
using Attrition.Common.ScriptableVariables.DataTypes;
using Attrition.Names;
using UnityEngine;

namespace Attrition.CharacterSelection.Selection
{
    public class CharacterSelectionApplicator
    {
        private readonly StringVariable playerName;
        private readonly CharacterClassVariable playerClass;

        public CharacterSelectionApplicator(StringVariable playerName, CharacterClassVariable playerClass)
        {
            this.playerName = playerName;
            this.playerClass = playerClass;
        }
        
        public void ApplyToPlayableCharacter(GameObject selectedCharacter)
        {
            if (selectedCharacter == null)
            {
                return;
            }

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

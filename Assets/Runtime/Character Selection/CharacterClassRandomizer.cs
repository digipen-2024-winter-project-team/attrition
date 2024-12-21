using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Attrition.CharacterSelection
{
    public class CharacterClassRandomizer : MonoBehaviour
    {
        [SerializeField]
        private List<CharacterClass> availableClasses;
        [SerializeField]
        private List<SelectableCharacterBehaviour> characters;

        public IReadOnlyList<SelectableCharacterBehaviour> Characters => this.characters;
        
        private void OnEnable()
        {
            this.Randomize();
        }

        private void Randomize()
        {
            CharacterClass PickRandomClass()
            {
                var randomIndex = Random.Range(0, this.availableClasses.Count);
                return this.availableClasses[randomIndex];
            }

            foreach (var character in this.characters)
            {
                var characterClass = PickRandomClass();
                
                while (this.characters.Exists(c => c.CharacterClass == characterClass))
                {
                    characterClass = PickRandomClass();
                }

                character.gameObject.name = $"{characterClass.DisplayName}";
                character.CharacterClass = characterClass;
                
                if (character.TryGetComponent(out CharacterModelController modelController))
                {
                    modelController.UpdateCharacterModel();
                }
            }
        }
    }
    
}

// using System.Collections.Generic;
// using System.Linq;
// using Attrition.CharacterSelection;
// using Attrition.Common;
// using UnityEngine;
// using Random = UnityEngine.Random;
//
// namespace Attrition.CharacterClasses
// {
//     [DefaultExecutionOrder(10)]
//     public class CharacterClassRandomizer : MonoBehaviour
//     {
//         [SerializeField]
//         private List<CharacterClass> availableClasses;
//         [SerializeField]
//         private List<SelectableCharacterBehaviour> characters;
//
//         public IReadOnlyList<SelectableCharacterBehaviour> Characters => this.characters;
//         
//         private void OnEnable()
//         {
//             this.Randomize();
//         }
//
//         private void Randomize()
//         {
//             CharacterClass PickRandomClass()
//             {
//                 var randomIndex = Random.Range(0, this.availableClasses.Count);
//                 return this.availableClasses[randomIndex];
//             }
//
//             foreach (var character in this.characters)
//             {
//                 var characterClass = PickRandomClass();
//
//                 var characterClassBehaviours = this.characters
//                     .Select(c => c.GetComponent<CharacterClassBehaviour>())
//                     .Where(cb => cb != null)
//                     .ToList();
//                 
//                 while (characterClassBehaviours.Any(ccb => ccb.CharacterClass == characterClass))
//                 {
//                     characterClassBehaviours = this.characters
//                         .Select(c => c.GetComponent<CharacterClassBehaviour>())
//                         .Where(cb => cb != null)
//                         .ToList();
//                     
//                     characterClass = PickRandomClass();
//                 }
//                 
//                 character.gameObject.name = $"{characterClass.DisplayName}";
//                 character = characterClass;
//                 
//                 if (character.TryGetComponent(out CharacterModelController modelController))
//                 {
//                     modelController.UpdateCharacterModel();
//                 }
//             }
//         }
//     }
//     
// }

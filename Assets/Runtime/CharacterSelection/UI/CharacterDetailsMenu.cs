using Attrition.CharacterSelection.Characters;
using Attrition.Common;
using Attrition.Common.SerializedEvents;
using Attrition.Names;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Attrition.CharacterSelection.UI
{
    [ExecuteAlways]
    public class CharacterDetailsMenu : MonoBehaviour
    {
        [SerializeField]
        private CharacterSelectionCharacterBehaviour boundTo;
        [SerializeField]
        private string characterName;
        [SerializeField]
        private string characterClass;
        [SerializeField]
        private SerializedEvent submitted;
        [Header("Component References")]
        [SerializeField]
        private TextMeshProUGUI nameTextComponent;
        [SerializeField]
        private TextMeshProUGUI classTextComponent;
        [SerializeField]
        private Button buttonComponent;
        
        public string CharacterName
        {
            get => this.characterName;
            set
            {
                this.characterName = value;
                this.UpdateContents();
            }
        }

        public string CharacterClass
        {
            get => this.characterClass;
            set
            {
                this.characterClass = value;
                this.UpdateContents();
            }
        }
        
        public IReadOnlySerializedEvent Submitted
        {
            get => this.submitted;
        }

        private void OnValidate()
        {
            this.Bind(this.boundTo);
            this.UpdateContents();
        }

        private void UpdateContents()
        {
            if (this.nameTextComponent != null)
            {
                this.nameTextComponent.text = this.characterName;
            }
            
            if (this.classTextComponent != null)
            {
                this.classTextComponent.text = this.CharacterClass;
            }
        }

        public void Bind(CharacterSelectionCharacterBehaviour characterSelectionCharacter)
        {
            if (characterSelectionCharacter == null)
            {
                return;
            }
            
            this.boundTo = characterSelectionCharacter;
            var nameBehaviour = characterSelectionCharacter.GetComponent<NameBehaviour>();
            var classBehaviour = characterSelectionCharacter.GetComponent<CharacterClassBehaviour>();
            var @class = classBehaviour.CharacterClass;

            this.CharacterName = nameBehaviour.DisplayName;
            this.CharacterClass = @class.DisplayName;
        }

        public void Submit()
        {
            if (Application.isPlaying)
            {
                this.buttonComponent.onClick.Invoke();
                this.submitted?.Invoke();
            }
        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Attrition.Character_Selection
{
    [ExecuteAlways]
    public class CharacterDetailsMenu : MonoBehaviour
    {
        [SerializeField]
        private SelectableCharacterBehaviour boundTo;
        [SerializeField]
        private string characterName;
        [SerializeField]
        private string characterClass;
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

        public void Bind(SelectableCharacterBehaviour character)
        {
            if (character == null)
            {
                return;
            }
            
            this.boundTo = character;
            var identity = character.GetComponent<CharacterIdentity>();
            var @class = identity.CharacterClass;

            this.CharacterName = identity.DisplayName;
            this.CharacterClass = @class.DisplayName;
        }

        public void Submit()
        {
            if (Application.isPlaying)
            {
                this.buttonComponent.onClick.Invoke();
            }
        }
    }
}

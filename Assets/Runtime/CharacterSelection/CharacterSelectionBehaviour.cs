using Unity.Cinemachine;
using UnityEngine;

namespace Attrition.CharacterSelection
{
    public class CharacterSelectionBehaviour : MonoBehaviour
    {
        private enum SelectionState
        {
            Browsing,
            Focused,
        }

        [SerializeField]
        private CinemachineCamera browseCamera;
        [SerializeField]
        private SelectableCharacterBehaviour[] characters;
        private SelectionState state;
        private int currentCharacterIndex;
        
        private SelectableCharacterBehaviour CurrentCharacter => this.characters[this.currentCharacterIndex];
        
        public void Navigate(Vector2 direction) 
        {
            // Sanitize the input so that we only take the value which is the most significant
            direction = new(Mathf.Round(direction.x), Mathf.Round(direction.y));
            
            // If we are browsing, left and right should change the character selection
            if (this.state == SelectionState.Browsing)
            {
                if (direction.y > 0)
                {
                    this.CurrentCharacter.Focus();
                    this.state = SelectionState.Focused;
                }
                else if (direction.x > 0)
                {
                    // Move to the next character
                    this.NavigateLeft();
                }
                else if (direction.x < 0)
                {
                    // Move to the previous character
                    this.NavigateRight();
                }
            }
            // If we are focused, up and down should focus (up) or unfocus (down) the current character
            else if (this.state == SelectionState.Focused)
            {
                if (direction.y < 0)
                {
                    this.CurrentCharacter.Unfocus();
                    this.state = SelectionState.Browsing;
                }
            }
        }

        private void NavigateLeft()
        {
            this.currentCharacterIndex = (this.currentCharacterIndex - 1 + this.characters.Length) % this.characters.Length;
            this.SetFocusToCurrentCharacter();
        }
        
        private void NavigateRight()
        {
            this.currentCharacterIndex = (this.currentCharacterIndex + 1) % this.characters.Length;
            this.SetFocusToCurrentCharacter();
        }

        private void SetFocusToCurrentCharacter()
        {
            this.browseCamera.Follow = this.CurrentCharacter.BrowseFollowTarget;
        }
    }
}

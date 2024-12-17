using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Attrition.CharacterSelectionScreen
{
    public class Carousel : MonoBehaviour
    {
        [SerializeField]
        private CinemachineCamera carouselCamera;
        [SerializeField]
        private SelectableCharacter[] characters;
        [SerializeField]
        private InputActionReference navigateAction;
        private int focusedIndex;
        private bool isCloseUp;
        
        private void OnEnable()
        {
            this.navigateAction.action.performed += this.OnNavigatePerformed;
        }
        
        private void OnDisable()
        {
            this.navigateAction.action.performed -= this.OnNavigatePerformed;
        }

        private void OnNavigatePerformed(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();
            this.HandleNavigation(value);
        }

        public void Next()
        {
            this.focusedIndex = (this.focusedIndex + 1) % this.characters.Length;
            var focusedCharacter = this.characters[this.focusedIndex];
            this.SetFocus(focusedCharacter);
        }
        
        public void Previous()
        {
            this.focusedIndex = (this.focusedIndex - 1 + this.characters.Length) % this.characters.Length;
            var focusedCharacter = this.characters[this.focusedIndex];
            this.SetFocus(focusedCharacter);
        }

        private void HandleNavigation(Vector2 value)
        {
            if (this.isCloseUp)
            {
                if (value.y < 0)
                {
                    var focusedCharacter = this.characters[this.focusedIndex];
                    this.ExitCloseUp(focusedCharacter);
                }
            }
            else
            {
                if (value.y == 0)
                {
                    if (value.x > 0)
                    {
                        this.Next();
                    }
                    else if (value.x < 0)
                    {
                        this.Previous();
                    }
                }
                else
                {
                    if (value.y > 0)
                    {
                        var focusedCharacter = this.characters[this.focusedIndex];
                        this.EnterCloseUp(focusedCharacter);
                    }
                }
            }
        }
        
        private void SetFocus(SelectableCharacter character)
        {
            this.carouselCamera.Follow = character.transform;
        }

        private void EnterCloseUp(SelectableCharacter character)
        {
            character.EnterCloseUp();
            this.isCloseUp = true;
        }
        
        private void ExitCloseUp(SelectableCharacter character)
        {
            character.ExitCloseUp();
            this.isCloseUp = false;
        }
    }
}

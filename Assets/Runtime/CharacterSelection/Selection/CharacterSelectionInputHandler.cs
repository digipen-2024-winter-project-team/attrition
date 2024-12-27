using Attrition.CharacterSelection.Selection.Navigation;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Attrition.CharacterSelection.Selection
{
    public class CharacterSelectionInputHandler
    {
        private readonly InputAction navigateAction;
        private readonly CharacterSelectionNavigator navigator;

        public CharacterSelectionInputHandler(InputAction navigateAction, CharacterSelectionNavigator navigator)
        {
            this.navigateAction = navigateAction;
            this.navigator = navigator;

            this.EnableInput();
        }

        public void EnableInput()
        {
            if (this.navigateAction == null)
            {
                return;
            }

            this.navigateAction.Enable();
            this.navigateAction.performed += this.OnNavigate;
        }

        public void DisableInput()
        {
            if (this.navigateAction == null)
            {
                return;
            }

            this.navigateAction.performed -= this.OnNavigate;
            this.navigateAction.Disable();
        }

        private void OnNavigate(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Performed)
            {
                return;
            }
            
            var value = context.ReadValue<Vector2>();
            this.navigator?.Navigate(value);
        }
    }
}
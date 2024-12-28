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

            var input = context.ReadValue<Vector2>();
            
            Direction? direction = input switch
            {
                { x: > 0, y: 0 } => Direction.Right,
                { x: < 0, y: 0 } => Direction.Left,
                { x: 0, y: > 0 } => Direction.Up,
                { x: 0, y: < 0 } => Direction.Down,
                _ => null
            };

            if (direction.HasValue)
            {
                this.navigator?.Navigate(direction.Value);
            }
        }
    }
}

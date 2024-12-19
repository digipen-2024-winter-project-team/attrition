using UnityEngine;
using UnityEngine.InputSystem;

namespace Attrition.CharacterSelection
{
    public class CharacterSelectionInputHandler : MonoBehaviour
    {
        [SerializeField]
        private InputActionReference navigateAction;
        [SerializeField]
        private CharacterSelectionBehaviour characterSelection;

        private void OnEnable()
        {
            this.navigateAction.action.Enable();
            this.navigateAction.action.performed += this.OnNavigate;
        }

        private void OnDisable()
        {
            this.navigateAction.action.performed -= this.OnNavigate;
        }

        private void OnNavigate(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();
            this.characterSelection.Navigate(value);
        }
    }
}
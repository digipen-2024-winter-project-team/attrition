using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Attrition.CharacterSelection.Selection
{
    public class CharacterSelectionInputHandler : MonoBehaviour
    {
        [SerializeField]
        private InputActionReference navigateAction;
        [FormerlySerializedAs("characterNavigation")]
        [FormerlySerializedAs("characterSelection")]
        [SerializeField]
        private CharacterSelectionNavigator characterSelectionNavigation;

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
            this.characterSelectionNavigation.Navigate(value);
        }
    }
}
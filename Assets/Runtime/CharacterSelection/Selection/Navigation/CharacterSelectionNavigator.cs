using System.Collections.Generic;
using Attrition.CharacterSelection.Characters;
using Unity.Cinemachine;
using UnityEngine;

namespace Attrition.CharacterSelection.Selection.Navigation
{
    public class CharacterSelectionNavigator
    {
        private readonly CharacterSelectionController controller;
        private readonly CharacterSelector selector;
        private readonly CharacterSelectionCameraController cameraController;
        private readonly CharacterSelectionStateHandler stateHandler;
        private readonly Cooldown navigationCooldown;
        private readonly Cooldown focusCooldown;
        
        public CharacterSelectionCharacterBehaviour CurrentSelection => this.selector.CurrentSelection;

        public CharacterSelectionNavigator(
            CharacterSelectionController controller,
            IList<CharacterSelectionCharacterBehaviour> characters,
            CharacterSelectionStateHandler stateHandler,
            CinemachineCamera dollyCamera)
        {
            this.controller = controller;
            this.selector = new(characters);
            this.cameraController = new(dollyCamera);
            this.stateHandler = stateHandler;
            this.navigationCooldown = new(controller);
            this.focusCooldown = new(controller);
        }

        public void Navigate(Vector2 direction)
        {
            // Round the input direction to the nearest integer vector
            var roundedDirection = Vector2Int.RoundToInt(direction);

            // Check for cooldowns
            var focusReady = !this.focusCooldown.IsOnCooldown;
            var navigationReady = !this.navigationCooldown.IsOnCooldown;

            if (this.stateHandler.IsFocused)
            {
                // Unfocus the character if navigating down
                if (roundedDirection.y < 0 && focusReady)
                {
                    this.stateHandler.UnfocusCharacter(this.selector.CurrentSelection);
                    this.StartCooldowns();
                }
            }
            else if (this.stateHandler.IsBrowsing)
            {
                if (roundedDirection.y > 0 && focusReady)
                {
                    // Focus the character if navigating up
                    this.stateHandler.FocusCharacter(this.selector.CurrentSelection);
                    this.StartCooldowns();
                }
                else if (roundedDirection.x != 0 && navigationReady)
                {
                    // Navigate left or right
                    var isNavigatingRight = roundedDirection.x > 0;
                    this.selector.Navigate(isNavigatingRight);
                    this.cameraController.MoveTo(this.selector.CurrentSelection, isNavigatingRight);
                    this.StartCooldowns();
                }
            }
        }
        
        private void StartCooldowns()
        {
            this.navigationCooldown.StartCooldown();
            this.focusCooldown.StartCooldown();
        }
    }
}

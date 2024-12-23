using System.Collections.Generic;
using Attrition.CharacterSelection.Characters;
using Unity.Cinemachine;
using UnityEngine;

namespace Attrition.CharacterSelection.Selection.Navigation
{
    public class CharacterSelectionNavigator
    {
        private readonly CharacterSelector selector;
        private readonly CharacterSelectionCameraController cameraController;
        private readonly CharacterSelectionStateHandler stateHandler;
        private readonly Cooldown navigationCooldown;
        private readonly Cooldown focusCooldown;

        public CharacterSelectionNavigator(
            CharacterSelectionController controller,
            IList<CharacterSelectionCharacterBehaviour> characters,
            CharacterSelectionStateHandler stateHandler,
            CinemachineCamera dollyCamera)
        {
            this.selector = new(characters);
            this.cameraController = new(dollyCamera);
            this.navigationCooldown = new Cooldown(controller);
            this.focusCooldown = new Cooldown(controller);
        }

        public void Navigate(Vector2 direction)
        {
            direction = Vector2Int.RoundToInt(direction);

            if (this.stateHandler.IsFocused)
            {
                if (direction.y < 0 && !this.focusCooldown.IsOnCooldown)
                {
                    this.stateHandler.UnfocusCharacter(this.selector.CurrentSelection);
                    this.StartCooldowns();
                }
            }
            else if (this.stateHandler.IsBrowsing)
            {
                if (direction.y > 0 && !this.focusCooldown.IsOnCooldown)
                {
                    this.stateHandler.FocusCharacter(this.selector.CurrentSelection);
                    this.StartCooldowns();
                }
                else if (direction.x != 0 && !this.navigationCooldown.IsOnCooldown)
                {
                    var isNavigatingRight = direction.x > 0;
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

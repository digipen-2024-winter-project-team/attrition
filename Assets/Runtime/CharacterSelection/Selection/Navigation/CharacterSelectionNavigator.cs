using System;
using System.Collections.Generic;
using Attrition.CharacterSelection.Characters;
using UnityEngine;

namespace Attrition.CharacterSelection.Selection.Navigation
{
    public class CharacterSelectionNavigator
    {
        private readonly IList<CharacterSelectionCharacterBehaviour> characters;
        private int currentIndex;
        private readonly CharacterSelectionCameraController cameraController;
        private readonly CharacterSelectionStateHandler stateHandler;
        private readonly Cooldown cycleCooldown;
        private readonly Cooldown inspectCooldown;

        public CharacterSelectionCharacterBehaviour CurrentSelection => this.characters[this.currentIndex];

        public CharacterSelectionNavigator(
            CharacterSelectionController controller,
            CharacterSelectionStateHandler stateHandler,
            CharacterSelectionCameraController cameraController)
        {
            this.characters = controller.Characters ?? throw new ArgumentNullException(nameof(controller.Characters));
            this.currentIndex = 0;
            this.cameraController = cameraController;
            this.stateHandler = stateHandler;
            this.cycleCooldown = new(controller);
            this.inspectCooldown = new(controller);
        }

        public void Navigate(Vector2 direction)
        {
            var roundedDirection = Vector2Int.RoundToInt(direction);

            if (this.stateHandler.IsInInspectMode && this.CanStopInspecting(roundedDirection))
            {
                this.StopInspectingCharacter();
                return;
            }

            if (this.stateHandler.IsInCycleMode)
            {
                if (this.CanInspect(roundedDirection))
                {
                    this.InspectCharacter();
                }
                else if (this.CanCycle(roundedDirection))
                {
                    this.CycleCharacters(roundedDirection.x > 0);
                }
            }
        }

        private void CycleCharacters(bool isCyclingRight)
        {
            this.CycleIndex(isCyclingRight);
            this.cameraController.MoveTo(this.CurrentSelection, isCyclingRight);
            this.StartCooldowns();
        }

        private void CycleIndex(bool isCyclingRight)
        {
            if (this.characters.Count == 0)
            {
                return;
            }

            this.currentIndex = isCyclingRight
                ? (this.currentIndex + 1) % this.characters.Count
                : (this.currentIndex - 1 + this.characters.Count) % this.characters.Count;
        }

        private bool CanStopInspecting(Vector2Int direction) =>
            direction.y < 0 && !this.inspectCooldown.IsOnCooldown;

        private bool CanInspect(Vector2Int direction) =>
            direction.y > 0 && !this.inspectCooldown.IsOnCooldown;

        private bool CanCycle(Vector2Int direction) =>
            direction.x != 0 && !this.cycleCooldown.IsOnCooldown;

        private void StopInspectingCharacter()
        {
            this.stateHandler.UninspectCharacter(this.CurrentSelection);
            this.StartCooldowns();
        }

        private void InspectCharacter()
        {
            this.stateHandler.InspectCharacter(this.CurrentSelection);
            this.StartCooldowns();
        }

        private void StartCooldowns()
        {
            this.cycleCooldown.StartCooldown();
            this.inspectCooldown.StartCooldown();
        }
    }
}

using System;
using System.Collections.Generic;
using Attrition.CharacterSelection.Characters;
using Attrition.Common.Events.SerializedEvents;
using Attrition.Common.Timing;

namespace Attrition.CharacterSelection.Selection.Navigation
{
    public class CharacterSelectionNavigator
    {
        private readonly IList<CharacterSelectionCharacterBehaviour> characters;
        private int currentIndex;
        private readonly CharacterSelectionController controller;
        private readonly CharacterSelectionCameraController cameraController;
        private readonly CharacterSelectionStateHandler stateHandler;
        private readonly Cooldown cycleCooldown;
        private readonly Cooldown inspectCooldown;
        private readonly SerializedEvent<CharacterSelectionCharacterBehaviour> inspectStarted;
        private readonly SerializedEvent<CharacterSelectionCharacterBehaviour> inspectStopped;

        public CharacterSelectionCharacterBehaviour CurrentSelection => this.characters[this.currentIndex];

        public CharacterSelectionNavigator(
            CharacterSelectionController controller,
            CharacterSelectionStateHandler stateHandler,
            CharacterSelectionCameraController cameraController,
            Cooldown cycleCooldown,
            Cooldown inspectCooldown,
            SerializedEvent<CharacterSelectionCharacterBehaviour> inspectStarted,
            SerializedEvent<CharacterSelectionCharacterBehaviour> inspectStopped)
        {
            this.controller = controller ?? throw new ArgumentNullException(nameof(controller));
            this.characters = controller.Characters ?? throw new ArgumentNullException(nameof(controller.Characters));
            this.currentIndex = 0;
            this.cameraController = cameraController ?? throw new ArgumentNullException(nameof(cameraController));
            this.stateHandler = stateHandler ?? throw new ArgumentNullException(nameof(stateHandler));
            this.cycleCooldown = cycleCooldown ?? throw new ArgumentNullException(nameof(cycleCooldown));
            this.inspectCooldown = inspectCooldown ?? throw new ArgumentNullException(nameof(inspectCooldown));
            this.inspectStarted = inspectStarted;
            this.inspectStopped = inspectStopped;
        }

        public void Navigate(Direction direction)
        {
            // Handle inverse navigation if needed
            direction = this.controller.NavigationDirection == NavigationDirection.Reverse
                ? direction.Inverse()
                : direction;

            if (this.stateHandler.IsInInspectMode)
            {
                if (direction == Direction.Down && this.CanUseCooldown(this.inspectCooldown))
                {
                    this.StopInspecting();
                    return;
                }
            }
            
            if (this.stateHandler.IsInCycleMode)
            {
                if (direction == Direction.Up && this.CanUseCooldown(this.inspectCooldown))
                {
                    this.StartInspecting();
                }
                else if ((direction == Direction.Left || direction == Direction.Right) && this.CanUseCooldown(this.cycleCooldown))
                {
                    this.CycleCharacters(direction == Direction.Right);
                }
            }
        }

        private void CycleCharacters(bool isCyclingRight)
        {
            this.UpdateIndex(isCyclingRight);
            this.cameraController.MoveTo(this.CurrentSelection, isCyclingRight);
            this.cycleCooldown.Restart();
        }

        private void UpdateIndex(bool isCyclingRight)
        {
            if (this.characters.Count == 0) return;

            this.currentIndex = (this.currentIndex + (isCyclingRight ? 1 : -1) + this.characters.Count) % this.characters.Count;
        }

        private bool CanUseCooldown(Cooldown cooldown)
        {
            if (!cooldown.IsRunning)
            {
                return true;
            }
            else
            {
                return cooldown.Time <= this.controller.CooldownGracePeriod;
            }
        }

        private void StartInspecting()
        {
            this.stateHandler.InspectCharacter(this.CurrentSelection);
            this.inspectCooldown.Restart();
            
            this.inspectStarted.Invoke(this.CurrentSelection);
        }
        
        private void StopInspecting()
        {
            this.stateHandler.UninspectCharacter(this.CurrentSelection);
            this.inspectCooldown.Restart();
            
            this.inspectStopped.Invoke(this.CurrentSelection);
        }
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public static class DirectionExtensions
    {
        public static Direction Inverse(this Direction direction) =>
            direction switch
            {
                Direction.Up => Direction.Up,
                Direction.Down => Direction.Down,
                Direction.Left => Direction.Right,
                Direction.Right => Direction.Left,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
    }
}

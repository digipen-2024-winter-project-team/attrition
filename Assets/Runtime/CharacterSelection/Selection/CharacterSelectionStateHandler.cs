using System;
using Attrition.CharacterSelection.Characters;
using Attrition.Common.Events.SerializedEvents;

namespace Attrition.CharacterSelection.Selection
{
    public class CharacterSelectionStateHandler
    {
        public enum SelectionState
        {
            CycleMode,
            InspectMode,
        }

        private readonly Func<SelectionState> stateGetter;
        private readonly Action<SelectionState> stateSetter;
        private readonly SerializedEvent<SelectionState> stateChanged;

        public CharacterSelectionStateHandler(
            Func<SelectionState> stateGetter,
            Action<SelectionState> stateSetter,
            SerializedEvent<SelectionState> stateChanged,
            SelectionState initialState = SelectionState.CycleMode)
        {
            this.stateGetter = stateGetter;
            this.stateSetter = stateSetter;
            this.stateChanged = stateChanged;
            
            this.SetState(initialState);
        }

        public bool IsInCycleMode => this.GetState() == SelectionState.CycleMode;
        public bool IsInInspectMode => this.GetState() == SelectionState.InspectMode;
        
        public void InspectCharacter(CharacterSelectionCharacterBehaviour character)
        {
            if (character == null)
            {
                return;
            }

            character.Inspect();
            this.SetState(SelectionState.InspectMode);
        }

        public void UninspectCharacter(CharacterSelectionCharacterBehaviour character)
        {
            if (character == null)
            {
                return;
            }

            character.Uninspect();
            this.SetState(SelectionState.CycleMode);
        }

        private SelectionState GetState()
        {
            return this.stateGetter.Invoke();
        }
        
        private void SetState(SelectionState state)
        {
            var currentState = this.GetState();
            
            if (currentState == state)
            {
                return;
            }

            this.stateSetter.Invoke(state);
            this.stateChanged?.Invoke(currentState);
        }
    }
}

using System;
using Attrition.CharacterSelection.Characters;
using Attrition.Common.SerializedEvents;

namespace Attrition.CharacterSelection.Selection
{
    public class CharacterSelectionStateHandler
    {
        public enum SelectionState
        {
            Browsing,
            Focused,
        }

        private readonly Func<SelectionState> stateGetter;
        private readonly Action<SelectionState> stateSetter;
        private readonly SerializedEvent<SelectionState> stateChanged;

        public CharacterSelectionStateHandler(
            Func<SelectionState> stateGetter,
            Action<SelectionState> stateSetter,
            SerializedEvent<SelectionState> stateChanged,
            SelectionState initialState = SelectionState.Browsing)
        {
            this.stateGetter = stateGetter;
            this.stateSetter = stateSetter;
            this.stateChanged = stateChanged;
            
            this.SetState(initialState);
        }

        public bool IsBrowsing => this.GetState() == SelectionState.Browsing;
        public bool IsFocused => this.GetState() == SelectionState.Focused;
        
        public void FocusCharacter(CharacterSelectionCharacterBehaviour character)
        {
            if (character == null)
            {
                return;
            }

            character.Focus();
            this.SetState(SelectionState.Focused);
        }

        public void UnfocusCharacter(CharacterSelectionCharacterBehaviour character)
        {
            if (character == null)
            {
                return;
            }

            character.Unfocus();
            this.SetState(SelectionState.Browsing);
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

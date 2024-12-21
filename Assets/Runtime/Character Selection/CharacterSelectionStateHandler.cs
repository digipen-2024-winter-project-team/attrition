using UnityEngine;
using UnityEngine.Events;

namespace Attrition.CharacterSelection
{
    public class CharacterSelectionStateHandler : MonoBehaviour
    {
        public enum SelectionState
        {
            Browsing,
            Focused,
        }

        [SerializeField]
        private SelectionState currentState = SelectionState.Browsing;
        [SerializeField]
        private UnityEvent<SelectionState> onStateChanged;
        
        public UnityEvent<SelectionState> OnStateChanged => this.onStateChanged;

        public bool IsBrowsing => this.currentState == SelectionState.Browsing;
        public bool IsFocused => this.currentState == SelectionState.Focused;

        private void OnValidate()
        {
            this.SetState(this.currentState);
        }

        public void SetState(SelectionState newState)
        {
            if (this.currentState == newState) return;

            this.currentState = newState;
            this.OnStateChanged?.Invoke(this.currentState);
        }

        public SelectionState GetCurrentState()
        {
            return this.currentState;
        }
    }
}

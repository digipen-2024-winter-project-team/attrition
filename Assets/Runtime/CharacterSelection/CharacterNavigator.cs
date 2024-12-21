using Attrition.Common.SerializedEvents;
using UnityEngine;
using UnityEngine.Serialization;

namespace Attrition.CharacterSelection
{
    public class CharacterNavigator : MonoBehaviour
    {
        [FormerlySerializedAs("instantiator")]
        [SerializeField]
        private CharacterClassRandomizer classRandomizer;
        [SerializeField]
        private CinemachineDollyTweener dollyController;
        [SerializeField]
        private CinemachineSplineDollyPitcher pitchController;
        [SerializeField]
        private CharacterSelectionStateHandler stateHandler;
        
        [field: SerializeField]
        public SerializedEvent<SelectableCharacterBehaviour> Focused { get; set; }
        [field: SerializeField]
        public SerializedEvent<SelectableCharacterBehaviour> Unfocused { get; set; }

        private int currentCharacterIndex;
        private bool isNavigatingRight;

        private SelectableCharacterBehaviour CurrentCharacter => this.classRandomizer.Characters[this.currentCharacterIndex];
        
        private void OnEnable()
        {
            this.stateHandler.OnStateChanged.AddListener(this.HandleStateChanged);
            this.Navigate();
        }

        private void OnDisable()
        {
            this.stateHandler.OnStateChanged.RemoveListener(this.HandleStateChanged);
        }

        public void Navigate(Vector2 direction)
        {
            direction = new Vector2(Mathf.Round(direction.x), Mathf.Round(direction.y));

            if (this.stateHandler.IsFocused)
            {
                if (direction.y < 0)
                {
                    this.UnfocusCurrentCharacter();
                }
            }
            else if (this.stateHandler.IsBrowsing)
            {
                if (direction.y > 0)
                {
                    this.FocusCurrentCharacter();
                }
                else if (direction.x > 0)
                {
                    this.NavigateRight();
                }
                else if (direction.x < 0)
                {
                    this.NavigateLeft();
                }
            }
        }
        
        private void NavigateLeft()
        {
            this.isNavigatingRight = false;
            this.Navigate();
        }
        
        private void NavigateRight()
        {
            this.isNavigatingRight = true;
            this.Navigate();
        }

        private void Navigate()
        {
            // Update the current character index based on the navigation direction.
            this.currentCharacterIndex = this.isNavigatingRight
                ? (this.currentCharacterIndex - 1 + this.classRandomizer.Characters.Count) % this.classRandomizer.Characters.Count
                : (this.currentCharacterIndex + 1) % this.classRandomizer.Characters.Count;

            // Move the dolly to the updated position, respecting the direction.
            this.dollyController.MoveToPosition(
                this.CurrentCharacter.BrowseFollowTarget.position, 
                this.isNavigatingRight);
        }

        private void FocusCurrentCharacter()
        {
            this.CurrentCharacter.Focus();
            this.stateHandler.SetState(CharacterSelectionStateHandler.SelectionState.Focused);
            this.Focused.Raise(this.CurrentCharacter);
        }

        private void UnfocusCurrentCharacter()
        {
            this.CurrentCharacter.Unfocus();
            this.stateHandler.SetState(CharacterSelectionStateHandler.SelectionState.Browsing);
            this.Unfocused.Raise(this.CurrentCharacter);
        }

        private void HandleStateChanged(CharacterSelectionStateHandler.SelectionState newState)
        {
            // Placeholder for additional logic triggered by state changes.
        }
    }
}

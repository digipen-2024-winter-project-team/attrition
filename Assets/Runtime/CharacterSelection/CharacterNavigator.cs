using UnityEngine;

namespace Attrition.CharacterSelection
{
    public class CharacterNavigator : MonoBehaviour
    {
        [SerializeField] private SelectableCharacterBehaviour[] characters;
        [SerializeField] private CinemachineDollyTweener dollyController;
        [SerializeField] private CinemachineSplineDollyPitcher pitchController;
        [SerializeField] private CharacterSelectionStateHandler stateHandler;

        private int currentCharacterIndex;
        private bool isNavigatingRight;

        private SelectableCharacterBehaviour CurrentCharacter => this.characters[this.currentCharacterIndex];

        private void OnEnable()
        {
            this.stateHandler.OnStateChanged.AddListener(this.HandleStateChanged);
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
                ? (this.currentCharacterIndex - 1 + this.characters.Length) % this.characters.Length
                : (this.currentCharacterIndex + 1) % this.characters.Length;

            // Move the dolly to the updated position, respecting the direction.
            this.dollyController.MoveToPosition(
                this.CurrentCharacter.BrowseFollowTarget.position, 
                this.isNavigatingRight);
        }

        private void FocusCurrentCharacter()
        {
            this.CurrentCharacter.Focus();
            this.stateHandler.SetState(CharacterSelectionStateHandler.SelectionState.Focused);
        }

        private void UnfocusCurrentCharacter()
        {
            this.CurrentCharacter.Unfocus();
            this.stateHandler.SetState(CharacterSelectionStateHandler.SelectionState.Browsing);
        }

        private void HandleStateChanged(CharacterSelectionStateHandler.SelectionState newState)
        {
            // Placeholder for additional logic triggered by state changes.
        }
    }
}

using System.Collections;
using Attrition.Common.SerializedEvents;
using UnityEngine;

namespace Attrition.Character_Selection
{
    public class CharacterNavigator : MonoBehaviour
    {
        [SerializeField]
        private CharacterClassRandomizer classRandomizer;
        [SerializeField]
        private CinemachineDollyTweener dollyController;
        [SerializeField]
        private CinemachineSplineDollyPitcher pitchController;
        [SerializeField]
        private CharacterSelectionStateHandler stateHandler;
        [SerializeField]
        private float navigationCooldown = 1f;
        [SerializeField]
        private float focusCooldown = 1f;
        private bool isNavigationOnCooldown;
        private bool isFocusOnCooldown;
        
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
                    if (!this.isFocusOnCooldown)
                    {
                        this.UnfocusCurrentCharacter();
                        this.StartCoroutine(this.StartFocusCooldown());
                        this.StartCoroutine(this.StartNavigationCooldown());
                    }
                }
            }
            else if (this.stateHandler.IsBrowsing)
            {
                if (direction.y > 0)
                {
                    if (!this.isFocusOnCooldown)
                    {
                        this.FocusCurrentCharacter();
                        this.StartCoroutine(this.StartFocusCooldown());
                    }
                }
                else if (direction.x > 0)
                {
                    if (!this.isNavigationOnCooldown)
                    {
                        this.NavigateRight();
                        this.StartCoroutine(this.StartNavigationCooldown());
                    }
                }
                else if (direction.x < 0)
                {
                    if (!this.isNavigationOnCooldown)
                    {
                        this.NavigateLeft();
                        this.StartCoroutine(this.StartNavigationCooldown());
                    }
                }
            }
        }
        
        private void NavigateLeft()
        {
            this.isNavigatingRight = false;
            this.Navigate();
            this.StartCoroutine(this.StartNavigationCooldown());
        }
        
        private void NavigateRight()
        {
            this.isNavigatingRight = true;
            this.Navigate();
        }

        private IEnumerator StartNavigationCooldown()
        {
            this.isNavigationOnCooldown = true;
            yield return new WaitForSeconds(this.navigationCooldown);
            this.isNavigationOnCooldown = false;
        }
        
        private IEnumerator StartFocusCooldown()
        {
            this.isFocusOnCooldown = true;
            yield return new WaitForSeconds(this.focusCooldown);
            this.isFocusOnCooldown = false;
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

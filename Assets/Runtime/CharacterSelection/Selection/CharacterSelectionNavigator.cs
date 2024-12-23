using System.Collections;
using System.Linq;
using Attrition.CharacterClasses;
using Attrition.CharacterSelection.Characters;
using Attrition.Common.Cinemachine;
using Attrition.Common.Containers;
using Attrition.Common.SerializedEvents;
using UnityEngine;

namespace Attrition.CharacterSelection.Selection
{
    public class CharacterSelectionNavigator : MonoBehaviour
    {
        [SerializeField]
        private float navigationCooldown = 1f;
        [SerializeField]
        private float focusCooldown = 1f;
        
        [SerializeField]
        private CinemachineDollyTweener dollyController;
        [SerializeField]
        private CinemachineSplineDollyPitcher pitchController;
        [SerializeField]
        private CharacterSelectionStateHandler stateHandler;
        [SerializeField]
        private CharacterClassRandomizer2 classRandomizer;
        [SerializeField]
        private GameObjectContainer characterContainer;
        
        private bool isNavigationOnCooldown;
        private bool isFocusOnCooldown;
        private int currentCharacterIndex;
        private bool isNavigatingRight;
        
        private CharacterSelectionCharacterBehaviour CurrentCharacterSelectionCharacter => 
            this.characterContainer[this.currentCharacterIndex].GetComponent<CharacterSelectionCharacterBehaviour>();
        
        
        [field: SerializeField]
        public SerializedEvent<CharacterSelectionCharacterBehaviour> Focused { get; set; }
        [field: SerializeField]
        public SerializedEvent<CharacterSelectionCharacterBehaviour> Unfocused { get; set; }
        
        private void Awake()
        {
            this.GetDependencies();
        }

        private void Reset()
        {
            this.GetDependencies();
        }

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
            var charactersCount = this.characterContainer.Contents.Count();
            
            // Update the current character index based on the navigation direction.
            this.currentCharacterIndex = this.isNavigatingRight
                ? (this.currentCharacterIndex - 1 + this.characterContainer.Contents.Count()) % charactersCount
                : (this.currentCharacterIndex + 1) % charactersCount;

            // Move the dolly to the updated position, respecting the direction.
            this.dollyController.MoveToPosition(
                this.CurrentCharacterSelectionCharacter.BrowseFollowTarget.position, 
                this.isNavigatingRight);
        }

        private void FocusCurrentCharacter()
        {
            this.CurrentCharacterSelectionCharacter.Focus();
            this.stateHandler.SetState(CharacterSelectionStateHandler.SelectionState.Focused);
            this.Focused.Raise(this.CurrentCharacterSelectionCharacter);
        }

        private void UnfocusCurrentCharacter()
        {
            this.CurrentCharacterSelectionCharacter.Unfocus();
            this.stateHandler.SetState(CharacterSelectionStateHandler.SelectionState.Browsing);
            this.Unfocused.Raise(this.CurrentCharacterSelectionCharacter);
        }

        private void HandleStateChanged(CharacterSelectionStateHandler.SelectionState newState)
        {
            // Placeholder for additional logic triggered by state changes.
        }

        private void GetDependencies()
        {
            
        }
    }
}

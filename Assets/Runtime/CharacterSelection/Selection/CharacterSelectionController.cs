using System.Collections.Generic;
using Attrition.CharacterSelection.Characters;
using Attrition.CharacterSelection.Selection.Navigation;
using Attrition.CharacterSelection.UI;
using Attrition.ClassCharacterModel;
using Attrition.Common;
using Attrition.Common.ScriptableVariables.DataTypes;
using Attrition.Common.SerializedEvents;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Attrition.CharacterSelection.Selection
{
    public class CharacterSelectionController : MonoBehaviour
    {
        [Header("State")]
        [SerializeField]
        private CharacterSelectionStateHandler.SelectionState initialState;
        [ReadOnly]
        [SerializeField]
        private CharacterSelectionStateHandler.SelectionState currentState;
        [SerializeField]
        public SerializedEvent<CharacterSelectionStateHandler.SelectionState> stateChanged;
        
        [Header("Navigation")]
        [SerializeField]
        private float navigationCooldown = 1f;
        [SerializeField]
        private float focusCooldown = 1f;
        
        [Header("References")]
        [SerializeField]
        private List<CharacterSelectionCharacterBehaviour> characters;
        [SerializeField]
        private StringVariable playerName;
        [SerializeField]
        private CharacterClassVariable playerClass;
        [SerializeField]
        private InputActionReference navigateAction;
        [SerializeField]
        private CinemachineCamera dollyCamera;
        [SerializeField]
        private CharacterDetailsMenu detailsMenu;
        
        private CharacterSelectionInputHandler inputHandler;
        private CharacterSelectionStateHandler stateHandler;
        private CharacterSelectionNavigator navigator;
        private CharacterSelectionApplicator applicator;

        public IReadOnlySerializedEvent<CharacterSelectionStateHandler.SelectionState> StateChanged => this.stateChanged;
        
        private void Awake()
        {
            this.Initialize();
        }

        private void OnEnable()
        {
            this.inputHandler.EnableInput();
            this.detailsMenu.Submitted.Invoked += this.OnDetailsMenuSubmitted;
        }

        private void Initialize()
        {
            this.stateHandler = new(
                () => this.currentState,
                (value) => this.currentState = value,
                this.stateChanged,
                this.initialState);
            
            this.navigator =
                new(
                    this,
                    this.characters, 
                    this.stateHandler,
                    this.dollyCamera);
            
            this.inputHandler = new(this.navigateAction, this.navigator);
            
            this.applicator = new(this.playerName, this.playerClass);
            
        }
        
        private void OnDetailsMenuSubmitted()
        {
            var currentSelection = this.navigator.CurrentSelection;
            this.applicator.ApplyToPlayableCharacter(currentSelection.gameObject);
        }
    }
}

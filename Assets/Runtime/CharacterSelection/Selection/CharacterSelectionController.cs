using System.Collections.Generic;
using Attrition.CharacterSelection.Characters;
using Attrition.CharacterSelection.Selection.Navigation;
using Attrition.CharacterSelection.UI;
using Attrition.Common;
using Attrition.Common.ScriptableVariables.DataTypes;
using Attrition.Common.SerializedEvents;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

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
        [Tooltip("The amount of time it takes to cycle between characters.")]
        [SerializeField]
        private float cycleDuration = 1f;
        [Tooltip("The amount of time it takes to inspect a character.")]
        [SerializeField]
        private float inspectDuration = 1f;
        
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
        private CharacterSelectionCameraController cameraController;

        public float CycleDuration => this.cycleDuration;
        public float InspectDuration => this.inspectDuration;
        public IList<CharacterSelectionCharacterBehaviour> Characters => this.characters;

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
            
            this.cameraController = new(this, this.dollyCamera);
            
            this.navigator =
                new(
                    this,
                    this.stateHandler,
                    this.cameraController);
            
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

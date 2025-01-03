using System;
using System.Collections.Generic;
using Attrition.CharacterSelection.Characters;
using Attrition.CharacterSelection.Selection.Navigation;
using Attrition.CharacterSelection.UI;
using Attrition.Common;
using Attrition.Common.Events.SerializedEvents;
using Attrition.Common.ScriptableVariables.DataTypes;
using Attrition.Common.Timing;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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

        [FormerlySerializedAs("navigationNavigationDirection")]
        [Header("Navigation")]
        [SerializeField]
        private NavigationDirection navigationDirection;
        [Tooltip("The amount of time it takes to cycle between characters.")]
        [SerializeField]
        private Cooldown cycleCooldown;
        [Tooltip("The amount of time it takes to inspect a character.")]
        [SerializeField]
        private Cooldown inspectCooldown;
        [SerializeField]
        private float cooldownGracePeriod = 0.5f;
        [SerializeField]
        private SerializedEvent<CharacterSelectionCharacterBehaviour> inspectStarted;
        [SerializeField]
        private SerializedEvent<CharacterSelectionCharacterBehaviour> inspectedStopped;
        
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

        public float CycleDuration => this.cycleCooldown.Duration;
        public float InspectDuration => this.inspectCooldown.Duration;
        public float CooldownGracePeriod => this.cooldownGracePeriod;
        public IList<CharacterSelectionCharacterBehaviour> Characters => this.characters;

        public IReadOnlySerializedEvent<CharacterSelectionStateHandler.SelectionState> StateChanged => this.stateChanged;
        public NavigationDirection NavigationDirection
        {
            get => this.navigationDirection;
            set => this.navigationDirection = value;
        }

        private void Awake()
        {
            this.Initialize();
        }

        private void OnEnable()
        {
            this.inputHandler.EnableInput();
            this.detailsMenu.Submitted.Invoked += this.OnDetailsMenuSubmitted;
            this.navigator.Navigate(Direction.Right);
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            this.cycleCooldown.Tick(deltaTime);
            this.inspectCooldown.Tick(deltaTime);
        }

        public void Submit()
        {
            this.applicator.ApplyToPlayableCharacter(this.navigator.CurrentSelection.gameObject);
            // TODO: Replace this with call to a scene loader service
            SceneManager.LoadScene("Sandbox");
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
                    this.cameraController,
                    this.cycleCooldown,
                    this.inspectCooldown,
                    this.inspectStarted,
                    this.inspectedStopped);
            
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

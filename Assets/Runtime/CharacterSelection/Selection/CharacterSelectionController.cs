using System;
using Attrition.CharacterSelection.Selection.Navigation;
using Attrition.Common;
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
        private InputActionReference navigateAction;
        [SerializeField]
        private CinemachineCamera dollyCamera;

        private CharacterSelectionInputHandler inputHandler;
        private CharacterSelectionStateHandler stateHandler;
        private CharacterSelectionApplicator applicator;
        private CharacterSelectionNavigator navigator;

        public SerializedEvent<CharacterSelectionStateHandler.SelectionState> StateChanged => this.stateChanged;
        
        private void GetDependencies()
        {
            this.inputHandler = new(this.navigateAction, this.navigator);
        }
    }
}

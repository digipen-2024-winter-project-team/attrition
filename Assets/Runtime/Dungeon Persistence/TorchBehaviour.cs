using System;
using Attrition.Common;
using Attrition.PlayerCharacter;
using UnityEngine;
using UnityEngine.Events;

namespace Attrition.Persistence
{
    public class TorchBehaviour : MonoBehaviour
    {
        [ReadOnly]
        [SerializeField] private string id;
        [SerializeField] private TorchPersistenceData data;
        [SerializeField] private TorchState startingState = TorchState.Unlit;
        [SerializeField] private Interactable interactable;
        [Space] [SerializeField] private UnityEvent lit;
        [Space] [SerializeField] private UnityEvent unlit;
        
        private TorchState currentState;
        
        private void OnEnable()
        {
            var state = data.GetTorchState(id, startingState);
            
            SetTorchState(state);

            if (interactable != null)
            {
                interactable.Interacted.Invoked += OnInteracted;
            }
        }

        private void OnDisable()
        {
            data.SetTorchState(id, currentState);

            if (interactable != null)
            {
                interactable.Interacted.Invoked -= OnInteracted;
            }
        }

        private void OnInteracted(Player player)
        {
            SetTorchState(TorchState.Lit);
        }
        
        public void SetTorchState(TorchState torchState)
        {
            currentState = torchState;
            
            switch (torchState)
            {
                case TorchState.Lit:
                    lit.Invoke();
                    break;
                
                case TorchState.Unlit:
                    unlit.Invoke();
                    break;
            }
        }
    }
}

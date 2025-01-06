using System.Collections.Generic;
using UnityEngine;

namespace Attrition.Persistence
{
    public enum TorchState
    {
        Lit,
        Unlit,
    }
    
    [CreateAssetMenu(menuName = "Attrition/Persistence/Torch Data")]
    public class TorchPersistenceData : ScriptableObject
    {
        [SerializeField] private List<string> torchIDs;
        [SerializeField] private List<TorchState> torchStates;

        private void AddTorchState(string id, TorchState state)
        {
            torchIDs.Add(id);
            torchStates.Add(state);
        }
        
        public void SetTorchState(string id, TorchState state)
        {
            int index = torchIDs.IndexOf(id);

            if (index == -1)
            {
                AddTorchState(id, state);
            }
            else
            {
                torchStates[index] = state;
            }
            
        }

        public TorchState GetTorchState(string id, TorchState defaultState)
        {
            int index = torchIDs.IndexOf(id);

            if (index == -1)
            {
                SetTorchState(id, defaultState);
                return defaultState;
            }

            return torchStates[index];
        }

        public void RemoveTorchState(string id)
        {
            int index = torchIDs.IndexOf(id);

            if (index == -1)
            {
                return;
            }

            torchIDs.RemoveAt(index);
            torchStates.RemoveAt(index);
        }
    }
}

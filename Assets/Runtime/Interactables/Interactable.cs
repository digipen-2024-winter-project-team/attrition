using Attrition.Common.Events.SerializedEvents;
using Attrition.PlayerCharacter;
using UnityEngine;

namespace Attrition
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private bool active = true;
        [SerializeField] private SerializedEvent<Player> interacted;

        public IReadOnlySerializedEvent<Player> Interacted => interacted;

        public bool Active
        {
            get => active;
            set => active = value;
        }
        
        public void Interact(Player player)
        {
            if (!active)
            {
                return;
            }
            
            interacted.Invoke(player);
        }
    }
}

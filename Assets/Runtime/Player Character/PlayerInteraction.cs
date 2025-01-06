using System;
using System.Linq;
using Attrition.Common.Physics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Attrition.PlayerCharacter
{
    public class PlayerInteraction : Player.Component
    {
        [SerializeField] private InputActionReference interactInput;
        [SerializeField] private CollisionAggregate interactionTrigger;

        private void Update()
        {
            if (interactInput.action.WasPerformedThisFrame())
            {
                var interacting = interactionTrigger.Colliders
                    .Select(collider => collider.GetComponent<Interactable>())
                    .Where(interactable => interactable != null && interactable.Active)
                    .OrderBy(interactable => Vector3.Dot(Movement.MoveDirection,
                        interactable.transform.position - transform.position))
                    .FirstOrDefault();
                
                if (interacting != null)
                {
                    interacting.Interact(Player);
                }
            }
        }
    }
}

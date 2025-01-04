using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using Attrition.CombatTargeting;
using Attrition.Common.Physics;
using Attrition.Common.ScriptableVariables.ComponentTypes;

namespace Attrition.PlayerCharacter.Targeting
{
    public class PlayerTargeting : Player.Component
    {
        [SerializeField] private InputActionReference chooseTarget;
        [SerializeField] private GameObjectVariable targetGameObject;
        [SerializeField] private InputActionReference movementInput;
        [SerializeField] private float maxDistance;
        [SerializeField] private LayerMask targetingMask;
        [SerializeField] private float dotWeight;
        [SerializeField] private float distanceWeight;
        
        private Targetable targeting;

        public Transform Target => targeting == null ? null : targeting.transform;

        private void Update()
        {
            if (chooseTarget.action.WasPerformedThisFrame())
            {
                var targetDistances = FindObjectsByType<Targetable>(FindObjectsSortMode.None)
                    .Select(targetable =>
                    {
                        Vector3 vector = targetable.transform.position - transform.position;
                        float distance = vector.magnitude;
                        return (targetable, vector, distance);
                    })
                    .ToArray();

                Vector3 aimDirection = Movement.MoveDirection != Vector3.zero
                    ? Movement.MoveDirection
                    : transform.forward;
                float maxDistance = targetDistances.Length > 0 
                    ? targetDistances.Max(obj => obj.distance)
                    : 1;
                
                var newTarget = targetDistances
                    .OrderBy(obj =>
                    {
                        (_, Vector3 vector, float distance) = obj;

                        float dot = Vector3.Dot(aimDirection, vector / distance);
                        float dotScore = (dot + 1) / 2f;

                        float distanceScore = 1f - distance / maxDistance;

                        float score = (dotWeight * dotScore + distanceScore * distanceWeight)
                                      / (dotWeight + distanceWeight);
                        
                        return score;
                    })
                    .LastOrDefault().targetable;

                targeting = newTarget == targeting 
                    ? null 
                    : newTarget;

                targetGameObject.Value = targeting != null 
                    ? targeting.gameObject 
                    : null;
            }

            if (targeting != null)
            {
                bool hitSomething = Physics.Raycast(transform.position, targeting.transform.position - transform.position,
                    out var hit, maxDistance, targetingMask);
                
                if (!hitSomething || hit.collider.gameObject.layer == GameInfo.Ground.LayerIndex)
                {
                    targeting = null;
                }
            }
        }
    }
}

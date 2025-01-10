using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using Attrition.Common.Physics;
using Attrition.CombatTargeting;

namespace Attrition.PlayerCharacter
{
    public class PlayerTargeting : Player.Component
    {
        [SerializeField] private InputActionReference chooseTarget;
        [SerializeField] private RectTransform indicator;
        [SerializeField] private InputActionReference movementInput;
        [SerializeField] private float indicatorSpeed;
        [SerializeField] private float maxDistance;
        [SerializeField] private LayerMask targetingMask;
        [SerializeField] private float dotWeight;
        [SerializeField] private float distanceWeight;
        [SerializeField] private Vector2 startingUVPosition;
        
        private Targetable targeting;

        public Transform Target => targeting == null ? null : targeting.transform;
        
        private Vector2 indicatorPosition;
        private Vector2 indicatorVelocity;
        
        private void Start()
        {
            indicatorPosition = startingUVPosition;
        }

        private void Update()
        {
            if (chooseTarget.action.WasPerformedThisFrame())
            {
                if (targeting != null)
                {
                    indicatorPosition += GetUVPosition(targeting.transform.position);
                }
                else
                {
                    indicatorPosition = startingUVPosition;
                }
                
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
                
                if (targeting != null)
                {
                    indicatorPosition -= GetUVPosition(targeting.transform.position);
                }
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
            
            indicator.gameObject.SetActive(targeting != null);
        }

        private void LateUpdate()
        {
            if (targeting != null)
            {
                indicatorPosition = Vector2.SmoothDamp(indicatorPosition, Vector2.zero, ref indicatorVelocity, indicatorSpeed);

                Vector2 targetUV = GetUVPosition(targeting.transform.position) + indicatorPosition;
                var parentRect = ((RectTransform)indicator.parent).rect;
                indicator.anchoredPosition = parentRect.size * targetUV;
                
                print(targetUV);
            }
        }
    }
}

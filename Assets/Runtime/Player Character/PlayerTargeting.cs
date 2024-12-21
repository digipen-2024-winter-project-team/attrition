using System.Linq;
using Attrition.Common.Physics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Attrition.Player_Character
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

        public Transform Target => this.targeting == null ? null : this.targeting.transform;
        
        private Vector2 indicatorPosition;
        private Vector2 indicatorVelocity;
        
        private Vector2 GetUVPosition(Vector3 position) =>
            (Vector2)this.CinemachineBrain.OutputCamera.WorldToViewportPoint(position) - Vector2.one / 2f;

        private void Start()
        {
            this.indicatorPosition = this.startingUVPosition;
        }

        private void Update()
        {
            if (this.chooseTarget.action.WasPerformedThisFrame())
            {
                if (this.targeting != null)
                {
                    this.indicatorPosition += this.GetUVPosition(this.targeting.transform.position);
                }
                else
                {
                    this.indicatorPosition = this.startingUVPosition;
                }
                
                var targetDistances = FindObjectsByType<Targetable>(FindObjectsSortMode.None)
                    .Select(targetable =>
                    {
                        Vector3 vector = targetable.transform.position - this.transform.position;
                        float distance = vector.magnitude;
                        return (targetable, vector, distance);
                    })
                    .ToArray();

                Vector3 aimDirection = this.Movement.MoveDirection != Vector3.zero
                    ? this.Movement.MoveDirection
                    : this.transform.forward;
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

                        float score = (this.dotWeight * dotScore + distanceScore * this.distanceWeight)
                                      / (this.dotWeight + this.distanceWeight);
                        
                        return score;
                    })
                    .LastOrDefault().targetable;

                this.targeting = newTarget == this.targeting 
                    ? null 
                    : newTarget;
                
                if (this.targeting != null)
                {
                    this.indicatorPosition -= this.GetUVPosition(this.targeting.transform.position);
                }
            }

            if (this.targeting != null)
            {
                bool hitSomething = Physics.Raycast(this.transform.position, this.targeting.transform.position - this.transform.position,
                    out var hit, this.maxDistance, this.targetingMask);
                
                if (!hitSomething || hit.collider.gameObject.layer == GameInfo.Ground.LayerIndex)
                {
                    this.targeting = null;
                }
            }
            
            this.indicator.gameObject.SetActive(this.targeting != null);
        }

        private void LateUpdate()
        {
            if (this.targeting != null)
            {
                this.indicatorPosition = Vector2.SmoothDamp(this.indicatorPosition, Vector2.zero, ref this.indicatorVelocity, this.indicatorSpeed);

                Vector2 targetUV = this.GetUVPosition(this.targeting.transform.position) + this.indicatorPosition;
                var parentRect = ((RectTransform)this.indicator.parent).rect;
                this.indicator.anchoredPosition = parentRect.size * targetUV;
                
                print(targetUV);
            }
        }
    }
}

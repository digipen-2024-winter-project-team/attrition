using System.Collections.Generic;
using System.Linq;
using Attrition.Common;
using Attrition.Common.Physics;
using Attrition.DamageSystem;
using UnityEngine;

namespace Attrition.CombatTargeting
{
    public class TargetDetection : MonoBehaviour
    {
        [Tooltip("Ranking for which combat teams should be prioritized for targeting.")]
        [SerializeField] private List<CombatTeam> targetableTeamsRanking;
        [SerializeField] private float detectionRadius;
        
        [Tooltip("Duration of time the detection can remember a target after losing sight of it.")]
        [SerializeField] private float detectionRetentionDuration;
        
        [SerializeField] private Transform raycastOrigin;
        [SerializeField] private float raycastThickness = 1f;
        
        [SerializeField] private LayerMask detectionMask;
        [Tooltip("The maximum number of hits for raycast detection. Ideally should never change.")]
        [SerializeField] private int maxRaycastHits = 10;

        private bool targetVisible;
        private Targetable target;
        private float detectionRetentionTimer;
        
        public Targetable Target => target;

        private RaycastHit[] raycastResults;

        public bool TargetVisible => targetVisible;

        private void Reset()
        {
            detectionMask =
                LayerMask.GetMask(GameInfo.Targetable.Name, GameInfo.Ground.Name, GameInfo.Default.Name);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            GizmoUtility.DrawCircle(raycastOrigin.position, detectionRadius, Vector3.up);

            if (target != null)
            {
                Gizmos.color = targetVisible
                    ? Color.green
                    : Color.red;
                GizmoUtility.DrawSphereCast(raycastOrigin.position, target.transform.position, raycastThickness);
            }
        }

        private bool TargetableVisible(Targetable targetable)
        {
             int hits = Physics.SphereCastNonAlloc(raycastOrigin.position, raycastThickness, (targetable.transform.position - raycastOrigin.position).normalized,
                 raycastResults, detectionRadius, detectionMask);

             if (hits == 0) return false;
             
             foreach (var hit in raycastResults
                  .Take(hits)
                  .OrderBy(hit => hit.distance))
             {
                 // if something is hit before reaching targetable, view of target is obstructed.
                 if (hit.collider.gameObject.layer != GameInfo.Targetable.LayerIndex)
                 {
                     return false;
                 }
                 
                 if (hit.collider.gameObject == targetable.gameObject)
                 {
                     return true;
                 }
             }
             
             return false;
        }

        private void Awake()
        {
            raycastResults = new RaycastHit[maxRaycastHits];
        }

        private void Update()
        {
            if (target == null)
            {
                target = Targetable.Targetables
                        
                    // Filter for teams that are being targeted 
                    .Where(targetable => targetableTeamsRanking.Contains(targetable.Team))
                    
                    // Filter for targets that are visible
                    .Where(TargetableVisible)
                    
                    // Group by team and select top priority team
                    .GroupBy(targetable => targetable.Team)
                    .OrderBy(group => targetableTeamsRanking.IndexOf(group.Key))
                    .FirstOrDefault()?
                    
                    // Sort targets by distance
                    .OrderBy(targetable => Vector3.Distance(transform.position, targetable.transform.position))
                    .FirstOrDefault();
            }

            if (target != null)
            {
                targetVisible = TargetableVisible(target);
                
                if (targetVisible)
                {
                    detectionRetentionTimer = 0;
                }
                else
                {
                    detectionRetentionTimer += Time.deltaTime;

                    if (detectionRetentionTimer > detectionRetentionDuration)
                    {
                        target = null;
                    }
                }
            }
            else
            {
                targetVisible = false;
            }
        }
    }
}

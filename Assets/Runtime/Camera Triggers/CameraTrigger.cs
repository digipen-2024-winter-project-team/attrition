using System;
using Unity.Cinemachine;
using UnityEngine;
using Attrition.Common;

namespace Attrition.CameraTriggers
{
    public class CameraTrigger : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera cinemachineCamera;
        
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                cinemachineCamera.Priority = 1;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                cinemachineCamera.Priority = 0;
            }
        }

        #region Gizmos

        private static readonly Color triggerBoundsColor = new(1, 0, 0, 0.5f);
        
        public static GizmoVisibility TriggerGizmoVisibility = GizmoVisibility.Never;

        private void OnDrawGizmos()
        {
            if (TriggerGizmoVisibility == GizmoVisibility.Always)
            {
                DrawGizmos();
            }

            CameraTrigger.TriggerGizmoVisibility = GizmoVisibility.Never;
        }

        private void OnDrawGizmosSelected()
        {
            if (TriggerGizmoVisibility == GizmoVisibility.WhenSelected)
            {
                DrawGizmos();
            }
        }

        private void DrawGizmos()
        {
            var colliders = GetComponentsInChildren<Collider>();

            var bounds = colliders[0].bounds;

            foreach (var collider in colliders)
            {
                bounds.Encapsulate(collider.bounds);
            }

            Gizmos.color = triggerBoundsColor; 
            Gizmos.DrawCube(bounds.center, bounds.size);
        }
        
        #endregion
    }
}

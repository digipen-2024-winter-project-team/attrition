using Attrition.Common;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;

namespace Attrition.Camera_Triggers
{
    public class CameraTrigger : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera cinemachineCamera;
        
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                this.cinemachineCamera.Priority = 1;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                this.cinemachineCamera.Priority = 0;
            }
        }

        #region Gizmos

        private static readonly Color triggerBoundsColor = new(1, 0, 0, 0.5f);
        
        public const string TriggerGizmoVisibilityKey = "CameraTriggerGizmoVisiblity";

        private static GizmoVisibility TriggerGizmoVisibility => 
        #if UNITY_EDITOR
            (GizmoVisibility)EditorPrefs.GetInt(TriggerGizmoVisibilityKey, 0);
        #else
            GizmoVisibility.NeverShow;
        #endif
        
        private void OnDrawGizmos()
        {
            if (TriggerGizmoVisibility == GizmoVisibility.AlwaysShow)
            {
                this.DrawGizmos();
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (TriggerGizmoVisibility == GizmoVisibility.ShowWhenSelected)
            {
                this.DrawGizmos();
            }
        }

        private void DrawGizmos()
        {
            var colliders = this.GetComponentsInChildren<Collider>();

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

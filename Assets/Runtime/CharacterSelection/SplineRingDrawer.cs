using UnityEngine;
using UnityEngine.Splines;

namespace Attrition.CharacterSelection
{
    [RequireComponent(typeof(SplineContainer))]
    public class SplineRingDrawer : MonoBehaviour
    {
        [Header("Ring Settings")]
        [SerializeField]
        private float radius = 5f;
        [SerializeField]
        private int numberOfPoints = 2;
        [SerializeField]
        private Vector3 upVector = Vector3.up;
        private SplineContainer splineContainer;

        private void Awake()
        {
            this.GetDependencies();
        }

        private void Reset()
        {
            this.GetDependencies();
        }

        private void OnValidate()
        {
            this.GetDependencies();
            this.DrawCircle();
        }

        private void DrawCircle()
        {
            if (this.splineContainer == null || this.splineContainer.Spline == null)
            {
                return;
            }
            
            if (this.numberOfPoints < 2)
            {
                Debug.LogWarning("Number of points must be at least 2 to create a valid circle.");
                return;
            }

            // Clear the current spline
            this.splineContainer.Spline.Clear();

            // Adjust forwardVector to avoid near-parallel vectors
            var forwardVector = Vector3.Dot(Vector3.forward, this.upVector) > 0.99f ? Vector3.right : Vector3.forward;
            var rightVector = Vector3.Cross(this.upVector, forwardVector).normalized;
            forwardVector = Vector3.Cross(rightVector, this.upVector).normalized;

            // Calculate angle step and tangent length
            var angleStep = Mathf.PI * 2f / this.numberOfPoints;
            var tangentLength = this.radius * Mathf.Sin(angleStep / 2f) / 1.5f;

            // Generate points on the circle
            for (var i = 0; i < this.numberOfPoints; i++)
            {
                var angle = i * angleStep;
                var pointPosition = this.radius * (Mathf.Cos(angle) * rightVector + Mathf.Sin(angle) * forwardVector);

                // Calculate rotation facing the center
                var directionToCenter = -pointPosition.normalized;
                var rotation = Quaternion.LookRotation(directionToCenter, this.upVector);

                // Calculate local tangents
                var tangentWorld = Vector3.Cross(this.upVector, directionToCenter).normalized * tangentLength;
                var tangentInLocal = Quaternion.Inverse(rotation) * -tangentWorld;
                var tangentOutLocal = Quaternion.Inverse(rotation) * tangentWorld;

                // Add the knot to the spline
                this.splineContainer.Spline.Add(new BezierKnot(pointPosition, tangentInLocal, tangentOutLocal, rotation));
            }

            // Close the spline
            this.splineContainer.Spline.Closed = true;
        }


        private void GetDependencies()
        {
            if (this.splineContainer == null)
            {
                this.splineContainer = this.GetComponent<SplineContainer>();
            }
        }
    }
}
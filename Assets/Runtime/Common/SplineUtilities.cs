using UnityEngine;
using UnityEngine.Splines;

namespace Attrition.Common
{
    public static class SplineUtilities
    {
        private struct SplineSample
        {
            public Vector3 Position;
            public Vector3 Tangent;
            public Vector3 Up;

            public SplineSample(Vector3 position, Vector3 tangent, Vector3 up)
            {
                this.Position = position;
                this.Tangent = tangent;
                this.Up = up;
            }
        }

        private static SplineSample GetSample(this Spline spline, float position)
        {
            Vector3 point = spline.EvaluatePosition(position);
            var tangent = ((Vector3)spline.EvaluateTangent(position)).normalized;
            var up = ((Vector3)spline.EvaluateUpVector(position)).normalized;
            return new SplineSample(point, tangent, up);
        }

        public static Vector3 GetForward(this Spline spline, float position)
        {
            return spline.GetSample(position).Tangent;
        }

        public static Vector3 GetUp(this Spline spline, float position)
        {
            return spline.GetSample(position).Up;
        }

        public static Vector3 GetRight(this Spline spline, float position)
        {
            var forward = spline.GetForward(position);
            var up = spline.GetUp(position);
            return Vector3.Cross(up, forward).normalized;
        }

        public static float GetSplinePositionNearestToPoint(this Spline spline, Vector3 point)
        {
            var nearestPosition = 0f;
            var smallestDistance = float.MaxValue;
            var stepSize = 0.1f;

            // First pass: Coarse sampling
            for (var t = 0f; t <= 1f; t += stepSize)
            {
                Vector3 splinePoint = spline.EvaluatePosition(t);
                var distance = Vector3.Distance(splinePoint, point);

                if (distance < smallestDistance)
                {
                    smallestDistance = distance;
                    nearestPosition = t;
                }
            }

            // Second pass: Fine-tune within the nearest range
            stepSize = 0.01f;
            for (var t = nearestPosition - stepSize; t <= nearestPosition + stepSize; t += stepSize / 10f)
            {
                if (t is < 0f or > 1f) continue;
                Vector3 splinePoint = spline.EvaluatePosition(t);
                var distance = Vector3.Distance(splinePoint, point);

                if (distance < smallestDistance)
                {
                    smallestDistance = distance;
                    nearestPosition = t;
                }
            }

            return nearestPosition;
        }

        public static float GetSplinePositionFurthestFromPoint(this Spline spline, Vector3 point)
        {
            var furthestPosition = 0f;
            var largestDistance = float.MinValue;
            var stepSize = 0.1f;

            // First pass: Coarse sampling
            for (var t = 0f; t <= 1f; t += stepSize)
            {
                Vector3 splinePoint = spline.EvaluatePosition(t);
                var distance = Vector3.Distance(splinePoint, point);

                if (distance > largestDistance)
                {
                    largestDistance = distance;
                    furthestPosition = t;
                }
            }

            // Second pass: Fine-tune within the furthest range
            stepSize = 0.01f;
            for (var t = furthestPosition - stepSize; t <= furthestPosition + stepSize; t += stepSize / 10f)
            {
                if (t is < 0f or > 1f) continue;
                Vector3 splinePoint = spline.EvaluatePosition(t);
                var distance = Vector3.Distance(splinePoint, point);

                if (distance > largestDistance)
                {
                    largestDistance = distance;
                    furthestPosition = t;
                }
            }

            return furthestPosition;
        }

        public static Vector3 GetWorldPositionFurthestFromPoint(this Spline spline, Vector3 point)
        {
            var furthestPosition = spline.GetSplinePositionFurthestFromPoint(point);
            return spline.EvaluatePosition(furthestPosition);
        }

        public static Vector3 GetWorldPositionNearestToPoint(this Spline spline, Vector3 point)
        {
            var nearestPosition = spline.GetSplinePositionNearestToPoint(point);
            return spline.EvaluatePosition(nearestPosition);
        }

        public static Vector3 GetWorldPosition(this Spline spline, float position)
        {
            return spline.GetSample(position).Position;
        }
    }
}

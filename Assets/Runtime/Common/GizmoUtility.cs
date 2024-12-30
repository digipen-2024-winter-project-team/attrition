using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Attrition.Common
{
    public static class GizmoUtility
    {
        private const int CircleSides = 16;
        private const int CircleSpokes = 4;
        private const int SphereCastSpokes = 4;
        
        private static IEnumerable<float> GetAngles(int count, float offset = 0) => Enumerable.Range(0, count)
            .Select(i => ((float)i / count * 360f + offset) * Mathf.Deg2Rad);
        
        public static void DrawCircle(Vector3 center, float radius, Vector3 axis)
        {
            var rotation = Quaternion.FromToRotation(Vector3.up, axis);

            var circlePoints = GetAngles(CircleSides)
                .Select(angle => center + rotation * new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius)
                .ToArray();
            
            Gizmos.DrawLineStrip(circlePoints, true);

            var spokePoints = GetAngles(CircleSpokes, 45)
                .SelectMany(angle => new[]
                    { center, center + rotation * new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius })
                .ToArray();
            
            Gizmos.DrawLineList(spokePoints);
        }
        
        public static void DrawSphereCast(Vector3 origin, Vector3 end, float radius)
        {
            Vector3 direction = end - origin;
            
            DrawCircle(origin, radius, direction);
            DrawCircle(end, radius, direction);

            Vector3 perpendicular = Quaternion.FromToRotation(Vector3.up, direction) * direction;
            DrawCircle(origin, radius, perpendicular);
            DrawCircle(end, radius, perpendicular);

            var rotation = Quaternion.FromToRotation(Vector3.forward, direction);

            var spokePoints = GetAngles(SphereCastSpokes, 45)
                .Select(angle => rotation * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius)
                .SelectMany(offset => new[] { origin + offset, end + offset })
                .ToArray();

            Gizmos.DrawLineList(spokePoints);
        }
    }
}

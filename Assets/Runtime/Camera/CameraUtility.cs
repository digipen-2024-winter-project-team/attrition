using UnityEngine;

namespace Attrition.Camera
{
    public static class CameraUtility
    {
        public static Vector2 GetNormalizedScreenPosition(this UnityEngine.Camera camera, Vector3 position)
            => (Vector2)camera.WorldToViewportPoint(position) - Vector2.one / 2f;
    }
}

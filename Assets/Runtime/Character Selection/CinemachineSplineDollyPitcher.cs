using Unity.Cinemachine;
using UnityEngine;

namespace Attrition.CharacterSelection
{
    public class CinemachineSplineDollyPitcher : MonoBehaviour
    {
        [SerializeField]
        private CinemachineSplineDolly dolly;
        [SerializeField]
        private float pitchAngle = 15f;

        private void LateUpdate()
        {
            this.AdjustPitchAlongSpline();
        }

        private void AdjustPitchAlongSpline()
        {
            var splineForward = ((Vector3)this.dolly.Spline.EvaluateTangent(this.dolly.CameraPosition)).normalized;
            var splineUp = Vector3.up;
            var splineRight = Vector3.Cross(splineUp, splineForward).normalized;
            var baseForward = -splineRight;
            var pitchedForward = Quaternion.AngleAxis(this.pitchAngle, splineForward) * baseForward;
            var adjustedUp = Vector3.Cross(pitchedForward, splineRight).normalized;

            this.dolly.VirtualCamera.transform.rotation = Quaternion.LookRotation(pitchedForward, adjustedUp);
        }
    }
}

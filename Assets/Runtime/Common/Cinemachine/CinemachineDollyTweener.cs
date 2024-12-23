using Attrition.Common.DOTweenParameters;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

namespace Attrition.Common.Cinemachine
{
    public class CinemachineDollyTweener : MonoBehaviour
    {
        [SerializeField] private CinemachineSplineDolly dolly;
        // [SerializeField] private float dollyDuration = 1f;
        // [SerializeField] private Ease dollyEase = Ease.InOutQuad;
        [SerializeField]
        private DOTweenParameters.DOTweenParameters dollyAnimation;
        
        public float CurrentPositionOnSpline => this.dolly.CameraPosition;
        public CinemachineSplineDolly Dolly => this.dolly;

        public void MoveToPosition(Vector3 targetPosition, bool isNavigatingLeft)
        {
            var spline = this.dolly.Spline.Spline;
            var currentPositionOnSpline = this.dolly.CameraPosition;
            var targetPositionOnSpline = spline.GetSplinePositionNearestToPoint(targetPosition);

            // Ensure movement respects the navigation direction.
            if (isNavigatingLeft && targetPositionOnSpline > currentPositionOnSpline)
            {
                targetPositionOnSpline -= 1f; // Force movement left.
            }
            else if (!isNavigatingLeft && targetPositionOnSpline < currentPositionOnSpline)
            {
                targetPositionOnSpline += 1f; // Force movement right.
            }

            DOTween.To(
                    () => this.dolly.CameraPosition,
                    x => this.dolly.CameraPosition = Mathf.Repeat(x, 1f),
                    targetPositionOnSpline,
                    this.dollyAnimation.Duration)
                .ApplyParameters(this.dollyAnimation);
        }
    }
}

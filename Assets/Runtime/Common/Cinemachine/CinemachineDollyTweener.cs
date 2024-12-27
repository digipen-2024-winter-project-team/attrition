using Attrition.Common.DOTweenParameters;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

namespace Attrition.Common.Cinemachine
{
    public class CinemachineDollyTweener : MonoBehaviour
    {
        [SerializeField]
        private CinemachineSplineDolly dolly;
        [SerializeField]
        private DOTweenParameters.DOTweenParameters dollyAnimation;
        
        public float CurrentPositionOnSpline => this.dolly.CameraPosition;
        public CinemachineSplineDolly Dolly => this.dolly;
        public DOTweenParameters.DOTweenParameters DollyAnimation => this.dollyAnimation;

        public void MoveToPosition(Vector3 targetPosition, bool isNavigatingRight)
        {
            var spline = this.dolly.Spline.Spline;
            var currentPositionOnSpline = this.dolly.CameraPosition;
            var targetPositionOnSpline = spline.GetSplinePositionNearestToPoint(targetPosition);

            // Adjust for navigation direction
            if (isNavigatingRight && targetPositionOnSpline < currentPositionOnSpline)
            {
                targetPositionOnSpline += 1f; // Move forward/right
            }
            else if (!isNavigatingRight && targetPositionOnSpline > currentPositionOnSpline)
            {
                targetPositionOnSpline -= 1f; // Move backward/left
            }

            DOTween.To(
                    () => this.dolly.CameraPosition,
                    x => this.dolly.CameraPosition = Mathf.Repeat(x, 1f),
                    targetPositionOnSpline,
                    this.dollyAnimation.Duration)
                .ApplyParameters(this.dollyAnimation);

            Debug.Log($"Dolly moving: Current = {currentPositionOnSpline}, Target = {targetPositionOnSpline}, Navigating Right = {isNavigatingRight}");
        }
    }
}

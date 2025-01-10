using Attrition.Common.DOTweenParameters;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

namespace Attrition.Common.Cinemachine
{
    public enum LoopedNavigationMode
    {
        Forward,
        Backward,
        Shortest,
        Longest
    }

    public class CinemachineDollyTweener : MonoBehaviour
    {
        [SerializeField]
        private CinemachineSplineDolly dolly;
        [SerializeField]
        private DOTweenParameters.DOTweenParameters dollyAnimation;
        [SerializeField]
        private LoopedNavigationMode loopedNavigationMode = LoopedNavigationMode.Shortest;

        public float CurrentPositionOnSpline => this.dolly.CameraPosition;
        public CinemachineSplineDolly Dolly => this.dolly;
        public DOTweenParameters.DOTweenParameters DollyAnimation => this.dollyAnimation;
        public float Duration
        {
            get => this.dollyAnimation.Duration;
            set => this.dollyAnimation.Duration = value;
        }
        public LoopedNavigationMode LoopedNavigationMode
        {
            get => this.loopedNavigationMode;
            set => this.loopedNavigationMode = value;
        }

        public void MoveToPosition(Vector3 targetPosition)
        {
            var spline = this.dolly.Spline.Spline;
            var currentPositionOnSpline = this.dolly.CameraPosition;
            var targetPositionOnSpline = spline.GetSplinePositionNearestToPoint(targetPosition);

            // Adjust based on navigation mode
            targetPositionOnSpline = this.AdjustPositionForMode(currentPositionOnSpline, targetPositionOnSpline, this.loopedNavigationMode);

            DOTween.To(
                    () => this.dolly.CameraPosition,
                    x => this.dolly.CameraPosition = Mathf.Repeat(x, 1f),
                    targetPositionOnSpline,
                    this.dollyAnimation.Duration)
                .ApplyParameters(this.dollyAnimation);
        }

        private float AdjustPositionForMode(float current, float target, LoopedNavigationMode mode)
        {
            switch (mode)
            {
                case LoopedNavigationMode.Forward:
                    // Move forward along the spline, looping if necessary
                    if (target < current)
                    {
                        target += 1f;
                    }
                    break;
                case LoopedNavigationMode.Backward:
                    // Move backward along the spline, looping if necessary
                    if (target > current)
                    {
                        target -= 1f;
                    }
                    break;
                case LoopedNavigationMode.Shortest:
                    // Take the shortest path (forward or backward)
                    if (Mathf.Abs(target - current) > 0.5f)
                    {
                        target = target > current ? target - 1f : target + 1f;
                    }
                    break;
                case LoopedNavigationMode.Longest:
                    // Take the longest path, ensuring a full loop
                    if (Mathf.Abs(target - current) <= 0.5f)
                    {
                        target = target > current ? target + 1f : target - 1f;
                    }
                    else
                    {
                        target = target > current ? target : target + 1f;
                    }
                    break;
            }

            return target;
        }
    }
}

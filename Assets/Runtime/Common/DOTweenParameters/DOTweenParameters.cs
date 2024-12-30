using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Attrition.Common.DOTweenParameters
{
    /// <summary>
    /// A serializable class for configuring DOTween animations. 
    /// This class encapsulates all common parameters for DOTween animations, 
    /// including timing, easing, looping, lifecycle, updates, and callbacks.
    /// </summary>
    [Serializable]
    public class DOTweenParameters
    {
        /// <summary>
        /// Duration of the animation in seconds.
        /// </summary>
        [Header("Timing")]
        [Tooltip("Duration of the animation in seconds.")]
        public float Duration = 1f;

        /// <summary>
        /// Delay before the animation starts, in seconds.
        /// </summary>
        [Tooltip("Delay before the animation starts, in seconds.")]
        public float Delay = 0f;

        /// <summary>
        /// Whether the tween's duration is based on speed rather than a fixed time.
        /// </summary>
        [Tooltip("Whether the tween's duration is based on speed.")]
        public bool SpeedBased = false;

        /// <summary>
        /// Use a predefined easing function or a custom animation curve.
        /// </summary>
        [Header("Easing")]
        [Tooltip("Use a predefined Ease or a custom AnimationCurve.")]
        public bool UseAnimationCurve = false;

        /// <summary>
        /// Predefined ease type for the animation.
        /// Ignored if <see cref="UseAnimationCurve"/> is true.
        /// </summary>
        [Tooltip("Ease type of the animation (ignored if 'Use AnimationCurve' is true).")]
        public Ease Ease = Ease.Linear;

        /// <summary>
        /// Custom animation curve to define the easing behavior.
        /// Used if <see cref="UseAnimationCurve"/> is true.
        /// </summary>
        [Tooltip("Custom animation curve (used if 'Use AnimationCurve' is true).")]
        public AnimationCurve AnimationCurve = AnimationCurve.Linear(0, 0, 1, 1);

        /// <summary>
        /// The loop type of the animation (Restart, Yoyo, Incremental).
        /// </summary>
        [Header("Looping")]
        [Tooltip("Loop type of the animation.")]
        public LoopType LoopType = LoopType.Restart;

        /// <summary>
        /// Number of loops for the animation. Use -1 for infinite loops.
        /// </summary>
        [Tooltip("Number of loops. Use -1 for infinite loops.")]
        public int Loops = 0;

        /// <summary>
        /// Whether the animation should start playing automatically.
        /// </summary>
        [Header("Lifecycle")]
        [Tooltip("Whether the animation should play in reverse after completing.")]
        public bool AutoPlay = true;

        /// <summary>
        /// Whether the animation should automatically be killed when it completes.
        /// </summary>
        [Tooltip("Whether the animation should auto-kill when complete.")]
        public bool AutoKill = true;

        /// <summary>
        /// Indicates whether the tween should be recycled for reuse, improving performance.
        /// </summary>
        [Tooltip("Target for recycling this tween.")]
        public bool Recycle = false;

        /// <summary>
        /// The type of update used for the animation (Normal, Late, Fixed).
        /// </summary>
        [Header("Update")]
        [Tooltip("Determines the update type (Normal, Late, or Fixed).")]
        public UpdateType UpdateType = UpdateType.Normal;

        /// <summary>
        /// Whether the animation should ignore Unity's timeScale setting.
        /// Useful for ensuring the animation plays during game pauses.
        /// </summary>
        [Tooltip("Whether the tween should ignore Unity's timeScale (e.g., unaffected by game pause).")]
        public bool IgnoreTimeScale = false;

        /// <summary>
        /// An optional identifier for the tween, used for managing and targeting it (e.g., Kill, Pause).
        /// </summary>
        [Header("Miscellaneous")]
        [Tooltip("The target object used for ID purposes in DOTween's Kill or Pause methods.")]
        public UnityEngine.Object Id;

        /// <summary>
        /// Whether the animation is relative to its starting value.
        /// </summary>
        [Tooltip("Whether the tween should be relative to its starting value.")]
        public bool IsRelative = false;

        /// <summary>
        /// Event triggered when the animation starts.
        /// </summary>
        [Header("Callbacks")]
        [Tooltip("Called when the tween starts.")]
        public UnityEvent OnStarted = new UnityEvent();

        /// <summary>
        /// Event triggered every frame while the animation is running.
        /// </summary>
        [Tooltip("Called when the tween updates.")]
        public UnityEvent OnUpdated = new UnityEvent();

        /// <summary>
        /// Event triggered when the animation completes.
        /// </summary>
        [Tooltip("Called when the tween completes.")]
        public UnityEvent OnCompleted = new UnityEvent();

        /// <summary>
        /// Event triggered when the animation is killed.
        /// </summary>
        [Tooltip("Called when the tween is killed.")]
        public UnityEvent OnKilled = new UnityEvent();

        /// <summary>
        /// Configures a DOTween Tween with these parameters.
        /// </summary>
        /// <param name="tween">The Tween to configure.</param>
        public void ApplyToTween(Tween tween)
        {
            if (tween == null) return;

            tween.SetDelay(this.Delay)
                .SetLoops(this.Loops, this.LoopType)
                .SetAutoKill(this.AutoKill)
                .SetUpdate(this.UpdateType, this.IgnoreTimeScale)
                .SetRecyclable(this.Recycle)
                .SetRelative(this.IsRelative);

            // Apply speed-based duration if enabled
            if (this.SpeedBased)
            {
                tween.SetSpeedBased();
            }

            // Apply easing based on the toggle
            if (this.UseAnimationCurve)
            {
                tween.SetEase(this.AnimationCurve);
            }
            else
            {
                tween.SetEase(this.Ease);
            }

            if (this.Id != null)
            {
                tween.SetId(this.Id);
            }

            // Set callbacks
            tween.OnStart(() => this.OnStarted.Invoke());
            tween.OnUpdate(() => this.OnUpdated.Invoke());
            tween.OnComplete(() => this.OnCompleted.Invoke());
            tween.OnKill(() => this.OnKilled.Invoke());

            // Play or pause based on AutoPlay
            if (this.AutoPlay)
            {
                tween.Play();
            }
            else
            {
                tween.Pause();
            }
        }
    }

    public static class DOTweenParametersExtensions
    {
        public static Tween ApplyParameters(this Tween tween, DOTweenParameters parameters)
        {
            parameters.ApplyToTween(tween);
            return tween;
        }
    }
}
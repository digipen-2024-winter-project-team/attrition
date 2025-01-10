using System;
using System.Collections.Generic;
using Attrition.Common.DOTweenParameters;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace Attrition.Common.Audio.AudioMixerTweener
{
    public class AudioMixerTweener : MonoBehaviour
    {
        [Serializable]
        public class AudioMixerTweenable
        {
            public string ParameterName;
            public float TargetValue;
            public DOTweenParameters.DOTweenParameters Tween;
        }
        
        [SerializeField]
        private AudioMixer audioMixer;
        [SerializeField]
        private List<AudioMixerTweenable> tweenables;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Tags.Player))
            {
                return;
            }
            
            this.PlayTweens();
        }

        public void PlayTweens()
        {
            DOTween.Pause(this);
            DOTween.Kill(this);

            var sequence = DOTween.Sequence();
            
            foreach (var tweenable in this.tweenables)
            {
                var tween = DOTween
                    .To(() => this.GetFloat(tweenable.ParameterName),
                        (value) => this.SetFloat(tweenable.ParameterName, value),
                        tweenable.TargetValue,
                        tweenable.Tween.Duration)
                    .ApplyParameters(tweenable.Tween);

                sequence.Insert(0, tween);
            }

            sequence.SetId(this);
        }
        
        private float GetFloat(string parameterName)
        {
            this.audioMixer.GetFloat(parameterName, out var value);
        
            return value;
        }
        
        private void SetFloat(string parameterName, float value)
        {
            this.audioMixer.SetFloat(parameterName, value);
        }
    }
}

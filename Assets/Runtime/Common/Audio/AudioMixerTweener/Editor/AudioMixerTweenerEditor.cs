using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

namespace Attrition.Common.Audio.AudioMixerTweener.Editor
{
    [CustomEditor(typeof(AudioMixerTweener))]
    public class AudioMixerTweenerEditor : UnityEditor.Editor
    {
        private AudioMixerTweener audioMixerTweener;
        
        [SerializeField]
        private VisualTreeAsset visualTreeAsset;
        [SerializeField]
        private StyleSheet styleSheet;
        private VisualElement root;

        private void OnEnable()
        {
            this.root = this.visualTreeAsset.CloneTree();
            this.root.styleSheets.Add(this.styleSheet);
        }

        public override VisualElement CreateInspectorGUI()
        {
            this.audioMixerTweener = target as AudioMixerTweener;
            var parameterNames = GetExposedParameterNames(this.target);

            foreach (var parameterName in parameterNames)
            {
                Debug.Log($"{parameterName}");
            }
            
            return this.root;
        }

        public static List<string> GetExposedParameterNames(object audioMixerTweener)
        {
            var parameterNames = new List<string>();

            if (audioMixerTweener == null)
            {
                Debug.LogWarning("AudioMixerTweener instance is null.");
                return parameterNames;
            }

            // Use reflection to access the private "audioMixer" field from AudioMixerTweener
            var audioMixerField = audioMixerTweener.GetType()
                .GetField("audioMixer", BindingFlags.Instance | BindingFlags.NonPublic);

            if (audioMixerField != null)
            {
                var audioMixer = audioMixerField.GetValue(audioMixerTweener) as AudioMixer;

                if (audioMixer != null)
                {
                    // Use reflection to access the private method "GetExposedParameters" from AudioMixer
                    var getParameterNamesMethod = typeof(AudioMixer)
                        .GetMethod("GetExposedParameters", BindingFlags.Instance | BindingFlags.NonPublic);

                    if (getParameterNamesMethod != null)
                    {
                        // Retrieve the exposed parameters from the AudioMixer
                        var parameters = (object[])getParameterNamesMethod.Invoke(audioMixer, null);

                        if (parameters != null)
                        {
                            foreach (var parameter in parameters)
                            {
                                // Access the "name" field of the exposed parameter
                                var nameField = parameter.GetType().GetField("name", BindingFlags.Instance | BindingFlags.Public);

                                if (nameField != null)
                                {
                                    var name = nameField.GetValue(parameter) as string;
                                    if (!string.IsNullOrEmpty(name))
                                    {
                                        parameterNames.Add(name);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("Failed to access GetExposedParameters method via reflection.");
                    }
                }
                else
                {
                    Debug.LogError("Failed to retrieve AudioMixer from AudioMixerTweener.");
                }
            }
            else
            {
                Debug.LogError("AudioMixerTweener does not have a private 'audioMixer' field.");
            }

            return parameterNames;
        }
    }
}

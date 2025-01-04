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
            
            // Was going to make a custom inspector so that the ExposedParameters could be a DropdownField but
            // Unity really likes sealed internal classes.

            return this.root;
        }
    }
}

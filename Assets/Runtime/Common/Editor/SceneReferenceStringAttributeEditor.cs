using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Attrition.Common.Editor
{
    [CustomPropertyDrawer(typeof(SceneAssetAttribute))]
    public class SceneReferenceStringAttributeEditor : PropertyDrawer
    {
        private SerializedProperty property;
        
        private class SceneAssetContainer : ScriptableObject
        {
            [SerializeField] private SceneAsset asset;
        }
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                return new HelpBox("SceneReferenceString only works on strings.", HelpBoxMessageType.Error);
            }

            this.property = property;

            // Create serialized object and property for scene asset
            var container = ScriptableObject.CreateInstance<SceneAssetContainer>();
            var containerObj = new SerializedObject(container);
            var sceneProp = containerObj.FindProperty("asset");
            
            // Update to current string scene
            string sceneName = property.stringValue;
            sceneProp.objectReferenceValue = AssetDatabase.FindAssets("t:scene")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<SceneAsset>)
                .Where(scene => scene != null)
                .FirstOrDefault(scene => scene.name == sceneName);
            
            containerObj.ApplyModifiedProperties();
            
            // Create property
            var field = new PropertyField(sceneProp, property.displayName);
            field.Bind(containerObj);
            field.RegisterValueChangeCallback(OnValueChanged);
            
            return field;
        }

        private void OnValueChanged(SerializedPropertyChangeEvent changeEvent)
        {
            property.stringValue = changeEvent.changedProperty.objectReferenceValue is SceneAsset scene 
                ? scene.name
                : "";
            
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}

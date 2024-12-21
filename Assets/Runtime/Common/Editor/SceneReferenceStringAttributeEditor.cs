using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Attrition.Common.Editor
{
    [CustomPropertyDrawer(typeof(SceneAssetAttribute))]
    public class SceneReferenceStringAttributeEditor : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                return new HelpBox("SceneReferenceString only works on strings.", HelpBoxMessageType.Error);
            }
            
            var field = new ObjectField(property.displayName)
            {
                objectType = typeof(SceneAsset),
                value = property.stringValue.StringToSceneAsset(),
            };
            field.RegisterValueChangedCallback(OnValueChanged);
            
            return field;
            
            void OnValueChanged(ChangeEvent<Object> changeEvent)
            {
                property.stringValue = changeEvent.newValue is SceneAsset scene 
                    ? scene.name
                    : "";
                
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}

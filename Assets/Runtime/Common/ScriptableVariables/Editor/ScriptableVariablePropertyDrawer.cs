using System;
using Attrition.CharacterClasses;
using Attrition.Common.ScriptableVariables;
using Attrition.Common.ScriptableVariables.DataTypes;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

namespace Attrition.Runtime.Common.ScriptableVariables.Editor
{
    /// <summary>
    /// Custom property drawers for ScriptableVariable objects.
    /// </summary>
    [CustomPropertyDrawer(typeof(ScriptableVariable<>), true)]
    public class ScriptableVariablePropertyDrawer : PropertyDrawer
    {
        /// <summary>
        /// Represents the type of field exposed in the property drawer.
        /// </summary>
        private enum ExposedType
        {
            /// <summary>
            /// Exposes the variable itself.
            /// </summary>
            Variable,

            /// <summary>
            /// Exposes the value of the variable.
            /// </summary>
            Value,
        }

        private const string EditorPath = "Assets/Runtime/Common/ScriptableVariables/Editor";
        private const string VisualTreeFilename = "ScriptableVariablePropertyDrawer.uxml";
        private const string StyleSheetFilename = "ScriptableVariablePropertyDrawer.uss";
        private const string PrefKey = "Attrition.Runtime.Common.ScriptableVariables.Editor.DropdownFieldDrawerState";

        /// <summary>
        /// Creates the property GUI for the ScriptableVariable.
        /// </summary>
        /// <param name="property">The serialized property to create the GUI for.</param>
        /// <returns>A VisualElement representing the property GUI.</returns>
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = this.LoadUIElements();
            var propertyField = container.Q<PropertyField>("property-field");
            var dropdownField = container.Q<DropdownField>("dropdown-field");
            var createButton = container.Q<Button>("create-button");

            propertyField.label = property.displayName;
            this.BindPropertyField(propertyField, property);

            dropdownField.choices = new(Enum.GetNames(typeof(ExposedType)));
            dropdownField.value = EditorPrefs.GetString(PrefKey, ExposedType.Variable.ToString());

            SetExposedType(Enum.Parse<ExposedType>(dropdownField.value));
            UpdateUIVisibility();
            
            propertyField.RegisterValueChangeCallback(OnPropertyFieldValueChanged);
            dropdownField.RegisterValueChangedCallback(evt => OnDropdownValueChanged(evt, property));
            createButton.RegisterCallback<ClickEvent>(_ => OnCreateButtonClicked(property));

            return container;

            void UpdateUIVisibility()
            {
                if (property.objectReferenceValue == null)
                {
                    dropdownField.style.display = DisplayStyle.None;
                    createButton.style.display = DisplayStyle.Flex;
                }
                else
                {
                    dropdownField.style.display = DisplayStyle.Flex;
                    createButton.style.display = DisplayStyle.None;
                }

                dropdownField.MarkDirtyRepaint();
                createButton.MarkDirtyRepaint();
            }

            void SetExposedType(ExposedType type)
            {
                if (type == ExposedType.Variable)
                {
                    this.BindPropertyField(propertyField, property);
                }
                else if (type == ExposedType.Value && property.objectReferenceValue != null)
                {
                    var serializedObject = new SerializedObject(property.objectReferenceValue);
                    var valueProperty = serializedObject.FindProperty("value");
                    this.BindPropertyField(propertyField, valueProperty);
                }
            }

            void OnPropertyFieldValueChanged(SerializedPropertyChangeEvent evt)
            {
                UpdateUIVisibility();
            }

            void OnDropdownValueChanged(ChangeEvent<string> evt, SerializedProperty serializedProperty)
            {
                if (Enum.TryParse(evt.newValue, out ExposedType newType))
                {
                    SetExposedType(newType);
                    EditorPrefs.SetString(PrefKey, newType.ToString());
                }
            }

            void OnCreateButtonClicked(SerializedProperty serializedProperty)
            {
                var newInstance = this.CreateScriptableObjectWithSavePath();
                serializedProperty.objectReferenceValue = newInstance;
                serializedProperty.serializedObject.ApplyModifiedProperties();
            }
        }

        /// <summary>
        /// Loads the UI elements defined in UXML and USS files.
        /// </summary>
        /// <returns>A VisualElement containing the loaded UI elements.</returns>
        private VisualElement LoadUIElements()
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{EditorPath}/{VisualTreeFilename}");
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{EditorPath}/{StyleSheetFilename}");

            var container = visualTree.CloneTree();

            if (styleSheet != null)
            {
                container.styleSheets.Add(styleSheet);
            }
            else
            {
                Debug.LogWarning($"StyleSheet not found at path: {EditorPath}/{StyleSheetFilename}");
            }

            return container;
        }

        /// <summary>
        /// Binds a property field to a serialized property.
        /// </summary>
        /// <param name="field">The property field to bind.</param>
        /// <param name="property">The serialized property to bind to.</param>
        private void BindPropertyField(PropertyField field, SerializedProperty property)
        {
            field.bindingPath = property.propertyPath;
            field.Bind(property.serializedObject);
            field.MarkDirtyRepaint();
        }

        /// <summary>
        /// Utility method to create a ScriptableObject and save it at a specified path.
        /// </summary>
        /// <typeparam name="TScriptableObject">The type of ScriptableObject to create.</typeparam>
        /// <returns>The created ScriptableObject instance.</returns>
        private ScriptableObject CreateScriptableObjectWithSavePath()
        {
            var path = EditorUtility.SaveFilePanelInProject(
                "Save ScriptableVariable",
                "New ScriptableVariable",
                "asset",
                "Please specify a file name and location to save the ScriptableVariable.");

            if (string.IsNullOrEmpty(path)) return null;

            var valueType = fieldInfo.FieldType.BaseType
                .GetGenericArguments()
                .First();

            var type = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .First(type => type.BaseType is { IsGenericType: true } baseType
                               && baseType.GetGenericArguments().First() == valueType
                               && baseType.GetGenericTypeDefinition() == typeof(ScriptableVariable<>));
            
            var asset = ScriptableObject.CreateInstance(type);
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            return asset;
        }
    }
}

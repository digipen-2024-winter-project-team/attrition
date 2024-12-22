using System;
using Attrition.Character_Selection;
using Attrition.Common.ScriptableVariables;
using Attrition.Common.ScriptableVariables.DataTypes;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Attrition.Runtime.Common.ScriptableVariables.Editor
{
    /// <summary>
    /// Abstract base class for creating custom property drawers for ScriptableVariable objects.
    /// </summary>
    /// <typeparam name="T">The type of the variable being drawn.</typeparam>
    public abstract class ScriptableVariablePropertyDrawer<T> : PropertyDrawer
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
                var newInstance = this.CreateInstance();
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
        /// Abstract method to create a new instance of the ScriptableVariable.
        /// Must be implemented by derived classes.
        /// </summary>
        /// <returns>A new instance of the ScriptableVariable.</returns>
        protected abstract ScriptableVariable<T> CreateInstance();

        /// <summary>
        /// Utility method to create a ScriptableObject and save it at a specified path.
        /// </summary>
        /// <typeparam name="TScriptableObject">The type of ScriptableObject to create.</typeparam>
        /// <returns>The created ScriptableObject instance.</returns>
        protected static TScriptableObject CreateScriptableObjectWithSavePath<TScriptableObject>() where TScriptableObject : ScriptableObject
        {
            var path = EditorUtility.SaveFilePanelInProject(
                "Save ScriptableVariable",
                "New ScriptableVariable",
                "asset",
                "Please specify a file name and location to save the ScriptableVariable.");

            if (string.IsNullOrEmpty(path)) return null;

            var asset = ScriptableObject.CreateInstance<TScriptableObject>();
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            return asset;
        }
    }

    // Below are concrete implementations of ScriptableVariablePropertyDrawer for different variable types.

    /// <summary>
    /// Custom property drawer for FloatVariable.
    /// </summary>
    [CustomPropertyDrawer(typeof(FloatVariable))]
    public class FloatVariablePropertyDrawer : ScriptableVariablePropertyDrawer<float>
    {
        /// <inheritdoc />
        protected override ScriptableVariable<float> CreateInstance() => CreateScriptableObjectWithSavePath<FloatVariable>();
    }

    /// <summary>
    /// Custom property drawer for BoolVariable.
    /// </summary>
    [CustomPropertyDrawer(typeof(BoolVariable))]
    public class BoolVariablePropertyDrawer : ScriptableVariablePropertyDrawer<bool>
    {
        /// <inheritdoc />
        protected override ScriptableVariable<bool> CreateInstance() => CreateScriptableObjectWithSavePath<BoolVariable>();
    }

    /// <summary>
    /// Custom property drawer for IntVariable.
    /// </summary>
    [CustomPropertyDrawer(typeof(IntVariable))]
    public class IntVariablePropertyDrawer : ScriptableVariablePropertyDrawer<int>
    {
        /// <inheritdoc />
        protected override ScriptableVariable<int> CreateInstance() => CreateScriptableObjectWithSavePath<IntVariable>();
    }

    /// <summary>
    /// Custom property drawer for StringVariable.
    /// </summary>
    [CustomPropertyDrawer(typeof(StringVariable))]
    public class StringVariablePropertyDrawer : ScriptableVariablePropertyDrawer<string>
    {
        /// <inheritdoc />
        protected override ScriptableVariable<string> CreateInstance() => CreateScriptableObjectWithSavePath<StringVariable>();
    }

    /// <summary>
    /// Custom property drawer for AnimationCurveVariable.
    /// </summary>
    [CustomPropertyDrawer(typeof(AnimationCurveVariable))]
    public class AnimationCurveVariablePropertyDrawer : ScriptableVariablePropertyDrawer<AnimationCurve>
    {
        /// <inheritdoc />
        protected override ScriptableVariable<AnimationCurve> CreateInstance() => CreateScriptableObjectWithSavePath<AnimationCurveVariable>();
    }

    /// <summary>
    /// Custom property drawer for ColorVariable.
    /// </summary>
    [CustomPropertyDrawer(typeof(ColorVariable))]
    public class ColorVariablePropertyDrawer : ScriptableVariablePropertyDrawer<Color>
    {
        /// <inheritdoc />
        protected override ScriptableVariable<Color> CreateInstance() => CreateScriptableObjectWithSavePath<ColorVariable>();
    }

    /// <summary>
    /// Custom property drawer for Vector3Variable.
    /// </summary>
    [CustomPropertyDrawer(typeof(Vector3Variable))]
    public class Vector3VariablePropertyDrawer : ScriptableVariablePropertyDrawer<Vector3>
    {
        /// <inheritdoc />
        protected override ScriptableVariable<Vector3> CreateInstance() => CreateScriptableObjectWithSavePath<Vector3Variable>();
    }

    /// <summary>
    /// Custom property drawer for Vector2Variable.
    /// </summary>
    [CustomPropertyDrawer(typeof(Vector2Variable))]
    public class Vector2VariablePropertyDrawer : ScriptableVariablePropertyDrawer<Vector2>
    {
        /// <inheritdoc />
        protected override ScriptableVariable<Vector2> CreateInstance() => CreateScriptableObjectWithSavePath<Vector2Variable>();
    }

    /// <summary>
    /// Custom property drawer for Vector4Variable.
    /// </summary>
    [CustomPropertyDrawer(typeof(Vector4Variable))]
    public class Vector4VariablePropertyDrawer : ScriptableVariablePropertyDrawer<Vector4>
    {
        /// <inheritdoc />
        protected override ScriptableVariable<Vector4> CreateInstance() => CreateScriptableObjectWithSavePath<Vector4Variable>();
    }

    /// <summary>
    /// Custom property drawer for Matrix4x4Variable.
    /// </summary>
    [CustomPropertyDrawer(typeof(Matrix4x4Variable))]
    public class Matrix4x4VariablePropertyDrawer : ScriptableVariablePropertyDrawer<Matrix4x4>
    {
        /// <inheritdoc />
        protected override ScriptableVariable<Matrix4x4> CreateInstance() => CreateScriptableObjectWithSavePath<Matrix4x4Variable>();
    }

    /// <summary>
    /// Custom property drawer for QuaternionVariable.
    /// </summary>
    [CustomPropertyDrawer(typeof(QuaternionVariable))]
    public class QuaternionVariablePropertyDrawer : ScriptableVariablePropertyDrawer<Quaternion>
    {
        /// <inheritdoc />
        protected override ScriptableVariable<Quaternion> CreateInstance() => CreateScriptableObjectWithSavePath<QuaternionVariable>();
    }

    /// <summary>
    /// Custom property drawer for RayVariable.
    /// </summary>
    [CustomPropertyDrawer(typeof(RayVariable))]
    public class RayVariablePropertyDrawer : ScriptableVariablePropertyDrawer<Ray>
    {
        /// <inheritdoc />
        protected override ScriptableVariable<Ray> CreateInstance() => CreateScriptableObjectWithSavePath<RayVariable>();
    }

    /// <summary>
    /// Custom property drawer for Ray2DVariable.
    /// </summary>
    [CustomPropertyDrawer(typeof(Ray2DVariable))]
    public class Ray2DVariablePropertyDrawer : ScriptableVariablePropertyDrawer<Ray2D>
    {
        /// <inheritdoc />
        protected override ScriptableVariable<Ray2D> CreateInstance() => CreateScriptableObjectWithSavePath<Ray2DVariable>();
    }

    /// <summary>
    /// Custom property drawer for RectVariable.
    /// </summary>
    [CustomPropertyDrawer(typeof(RectVariable))]
    public class RectVariablePropertyDrawer : ScriptableVariablePropertyDrawer<Rect>
    {
        /// <inheritdoc />
        protected override ScriptableVariable<Rect> CreateInstance() => CreateScriptableObjectWithSavePath<RectVariable>();
    }

    /// <summary>
    /// Custom property drawer for RectIntVariable.
    /// </summary>
    [CustomPropertyDrawer(typeof(RectIntVariable))]
    public class RectIntVariablePropertyDrawer : ScriptableVariablePropertyDrawer<RectInt>
    {
        /// <inheritdoc />
        protected override ScriptableVariable<RectInt> CreateInstance() => CreateScriptableObjectWithSavePath<RectIntVariable>();
    }

    /// <summary>
    /// Custom property drawer for BoundsVariable.
    /// </summary>
    [CustomPropertyDrawer(typeof(BoundsVariable))]
    public class BoundsVariablePropertyDrawer : ScriptableVariablePropertyDrawer<Bounds>
    {
        /// <inheritdoc />
        protected override ScriptableVariable<Bounds> CreateInstance() => CreateScriptableObjectWithSavePath<BoundsVariable>();
    }

    /// <summary>
    /// Custom property drawer for GradientVariable.
    /// </summary>
    [CustomPropertyDrawer(typeof(GradientVariable))]
    public class GradientVariablePropertyDrawer : ScriptableVariablePropertyDrawer<Gradient>
    {
        /// <inheritdoc />
        protected override ScriptableVariable<Gradient> CreateInstance() => CreateScriptableObjectWithSavePath<GradientVariable>();
    }

    /// <summary>
    /// Custom property drawer for LayerMaskVariable.
    /// </summary>
    [CustomPropertyDrawer(typeof(LayerMaskVariable))]
    public class LayerMaskVariablePropertyDrawer : ScriptableVariablePropertyDrawer<LayerMask>
    {
        /// <inheritdoc />
        protected override ScriptableVariable<LayerMask> CreateInstance() => CreateScriptableObjectWithSavePath<LayerMaskVariable>();
    }

    /// <summary>
    /// Custom property drawer for CharacterClass.
    /// </summary>
    [CustomPropertyDrawer(typeof(CharacterClassVariable))]
    public class CharacterClassVariablePropertyDrawer : ScriptableVariablePropertyDrawer<CharacterClass>
    {
        /// <inheritdoc />
        protected override ScriptableVariable<CharacterClass> CreateInstance() =>
            CreateScriptableObjectWithSavePath<CharacterClassVariable>();
    }
}

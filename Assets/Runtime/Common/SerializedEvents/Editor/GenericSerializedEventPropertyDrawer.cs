using Attrition.Common.SerializedEvents;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Attrition.Runtime.Common.SerializedEvents.Editor
{
    /// <summary>
    /// Custom property drawer for <see cref="SerializedEvent"/>.
    /// </summary>
    [CustomPropertyDrawer(typeof(SerializedEvent))]
    public class SerializedEventPropertyDrawer : PropertyDrawer
    {
        /// <summary>
        /// Creates the GUI for the SerializedEvent property.
        /// </summary>
        /// <param name="property">The serialized property being drawn.</param>
        /// <returns>A configured <see cref="VisualElement"/>.</returns>
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return SerializedEventEditorHelpers.CreateRoot(property);
        }
    }

    /// <summary>
    /// Custom property drawer for generic <see cref="SerializedEvent"/> types.
    /// </summary>
    [CustomPropertyDrawer(typeof(SerializedEvent<>), true)]
    public class GenericSerializedEventPropertyDrawer : PropertyDrawer
    {
        /// <summary>
        /// Creates the GUI for the SerializedEvent property of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="property">The serialized property being drawn.</param>
        /// <returns>A configured <see cref="VisualElement"/>.</returns>
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return SerializedEventEditorHelpers.CreateRoot(property);
        }
    }

    /// <summary>
    /// Helper class for creating and configuring property drawer elements for <see cref="SerializedEvent"/>.
    /// </summary>
    public static class SerializedEventEditorHelpers
    {
        /// <summary>
        /// Path to the folder where this editor is located.
        /// </summary>
        private const string EditorPath = "Assets/Runtime/Common/SerializedEvents/Editor";

        /// <summary>
        /// Filename of the UXML template asset.
        /// </summary>
        private const string VisualTreeAssetFilename = "SerializedEventPropertyDrawer.uxml";

        /// <summary>
        /// Filename of the USS stylesheet asset.
        /// </summary>
        private const string StyleSheetAssetFilename = "SerializedEventPropertyDrawer.uss";

        /// <summary>
        /// Creates the root <see cref="VisualElement"/> for a SerializedEvent property drawer.
        /// </summary>
        /// <param name="property">The serialized property being drawn.</param>
        /// <returns>A configured <see cref="VisualElement"/>.</returns>
        public static VisualElement CreateRoot(SerializedProperty property)
        {
            var visualTreeAssetFilePath = $"{EditorPath}/{VisualTreeAssetFilename}";
            var styleSheetAssetFilePath = $"{EditorPath}/{StyleSheetAssetFilename}";

            // Load UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(visualTreeAssetFilePath);
            var root = visualTree.CloneTree();

            // Load USS
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(styleSheetAssetFilePath);
            root.styleSheets.Add(styleSheet);
            
            // Configure PropertyField
            var propertyField = root.Q<PropertyField>();
            propertyField.label = property.displayName;

            return root;
        }

        /// <summary>
        /// Configures a <see cref="Label"/> element within the root <see cref="VisualElement"/>.
        /// </summary>
        /// <param name="root">The root visual element.</param>
        /// <param name="labelName">The name of the label element to configure.</param>
        /// <param name="text">The text to set for the label.</param>
        public static void ConfigureLabel(VisualElement root, string labelName, string text)
        {
            var label = root.Q<Label>(labelName);
            if (label != null)
            {
                label.text = text;
            }
        }
    }
}

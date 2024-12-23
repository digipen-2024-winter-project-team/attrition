using Attrition.Common.SerializedEvents;
using UnityEditor;
using UnityEditor.UIElements;
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
    /// Abstract base class for property drawers of generic <see cref="SerializedEvent"/> types.
    /// </summary>
    /// <typeparam name="T">The type of the SerializedEvent.</typeparam>
    public abstract class SerializedEventPropertyDrawer<T> : PropertyDrawer
    {
        /// <summary>
        /// Creates the GUI for the SerializedEvent property of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="property">The serialized property being drawn.</param>
        /// <returns>A configured <see cref="VisualElement"/>.</returns>
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = SerializedEventEditorHelpers.CreateRoot(property);
            root = this.ConfigureVisualElement(root, property);
            return root;
        }

        /// <summary>
        /// Configures the root <see cref="VisualElement"/> for the property drawer.
        /// </summary>
        /// <param name="root">The root visual element.</param>
        /// <param name="property">The serialized property being drawn.</param>
        /// <returns>A configured <see cref="VisualElement"/>.</returns>
        protected virtual VisualElement ConfigureVisualElement(VisualElement root, SerializedProperty property)
        {
            return root;
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

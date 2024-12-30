using Attrition.Common;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Attrition.ReadOnlyAttributePropertyDrawer
{
    /// <summary>
    /// A custom property drawer for the <see cref="ReadOnlyAttribute"/> that render properties as visible but
    /// non-editable in the Inspector.
    /// </summary>
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyAttributePropertyDrawer : PropertyDrawer
    {
        /// <summary>
        /// Creates the custom UI for properties marked with the <see cref="ReadOnlyAttribute"/>.
        /// </summary>
        /// <param name="property">The serialized property to render in the Inspector.</param>
        /// <returns>A <see cref="VisualElement"/> containing the read-only property field.</returns>
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();
            var propertyField = new PropertyField(property);

            propertyField.SetEnabled(false);
            container.Add(propertyField);

            return container;
        }
    }
}

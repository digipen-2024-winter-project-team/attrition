using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Attrition.CharacterClasses.Editor
{
    [CustomEditor(typeof(CharacterClassValueAssigner))]
    public class CharacterClassValueAssignerEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            
            InspectorElement.FillDefaultInspector(root, serializedObject, this);
            
            root.Insert(1, new Button(((CharacterClassValueAssigner)target).AssignData)
            {
                text = "Assign Data",
            });
            
            return root;
        }
    }
}

using System;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Attrition.Persistence.Editor
{
    [CustomEditor(typeof(TorchBehaviour)), CanEditMultipleObjects]
    public class TorchBehaviourEditor : UnityEditor.Editor
    {
        private const string IDPropertyName = "id";
        
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            var buttons = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                },
            };
            buttons.Add(new Button(GenerateID)
            {
                text = "Generate ID",
                style =
                {
                    flexGrow = 1,
                }
            });
            
            buttons.Add(new Button(ResetID)
            {
                text = "Reset ID",
                style =
                {
                    flexGrow = 1,
                }
            });
            
            root.Insert(1, buttons);
            
            return root;
        }

        private void ResetID()
        {
            foreach (var target in targets.Select(target => new SerializedObject(target)))
            {
                target.FindProperty(IDPropertyName).stringValue = string.Empty;
                target.ApplyModifiedProperties();
            }
        }

        private void GenerateID()
        {
            foreach (var target in targets.Select(target => new SerializedObject(target)))
            {
                target.FindProperty(IDPropertyName).stringValue = Guid.NewGuid().ToString();
                target.ApplyModifiedProperties();
            }
        }
    }
}

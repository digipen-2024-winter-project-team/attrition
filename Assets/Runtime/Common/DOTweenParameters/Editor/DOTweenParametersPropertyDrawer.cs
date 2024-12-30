using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Attrition.Runtime.Common.DOTweenParameters.Editor
{
    [CustomPropertyDrawer(typeof(Attrition.Common.DOTweenParameters.DOTweenParameters))]
    public class DOTweenParametersPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create the root foldout
            var foldout = new Foldout
            {
                text = property.displayName,
                value = false,
            };

            // Add the "Timing" fields
            foldout.Add(new PropertyField(property.FindPropertyRelative("Duration")));
            foldout.Add(new PropertyField(property.FindPropertyRelative("Delay")));
            foldout.Add(new PropertyField(property.FindPropertyRelative("SpeedBased")));

            // Add the "Easing" fields
            var useAnimationCurveProperty = property.FindPropertyRelative("UseAnimationCurve");
            var easeProperty = property.FindPropertyRelative("Ease");
            var animationCurveProperty = property.FindPropertyRelative("AnimationCurve");
            
            var useAnimationCurveField = new PropertyField(useAnimationCurveProperty);
            var easeField = new PropertyField(easeProperty) { name = "EaseField" };
            var animationCurveField = new PropertyField(animationCurveProperty) { name = "AnimationCurveField" };

            foldout.Add(useAnimationCurveField);
            foldout.Add(easeField);
            foldout.Add(animationCurveField);

            // Toggle visibility of Ease/AnimationCurve based on UseAnimationCurve
            void UpdateEasingFields()
            {
                var useCurve = useAnimationCurveProperty.boolValue;
                easeField.style.display = useCurve ? DisplayStyle.None : DisplayStyle.Flex;
                animationCurveField.style.display = useCurve ? DisplayStyle.Flex : DisplayStyle.None;
            }

            // Initial visibility update
            UpdateEasingFields();

            // Register callback for changes in UseAnimationCurve
            useAnimationCurveField.RegisterCallback<ChangeEvent<bool>>(_ => UpdateEasingFields());

            // Add the "Looping" fields
            foldout.Add(new PropertyField(property.FindPropertyRelative("LoopType")));
            foldout.Add(new PropertyField(property.FindPropertyRelative("Loops")));

            // Add the "Lifecycle" fields
            foldout.Add(new PropertyField(property.FindPropertyRelative("AutoPlay")));
            foldout.Add(new PropertyField(property.FindPropertyRelative("AutoKill")));
            foldout.Add(new PropertyField(property.FindPropertyRelative("Recycle")));

            // Add the "Update" fields
            foldout.Add(new PropertyField(property.FindPropertyRelative("UpdateType")));
            foldout.Add(new PropertyField(property.FindPropertyRelative("IgnoreTimeScale")));

            // Add the "Miscellaneous" fields
            foldout.Add(new PropertyField(property.FindPropertyRelative("Id")));
            foldout.Add(new PropertyField(property.FindPropertyRelative("IsRelative")));

            // Add the "Callbacks" fields
            foldout.Add(new PropertyField(property.FindPropertyRelative("OnStarted")));
            foldout.Add(new PropertyField(property.FindPropertyRelative("OnUpdated")));
            foldout.Add(new PropertyField(property.FindPropertyRelative("OnCompleted")));
            foldout.Add(new PropertyField(property.FindPropertyRelative("OnKilled")));

            return foldout;
        }
    }
}

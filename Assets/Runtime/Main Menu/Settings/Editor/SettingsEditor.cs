﻿using Attrition.Main_Menu.Settings;
using UnityEngine;
using UnityEditor;
using Attrition.MainMenu.Settings;

namespace Attrition.MainMenu.Settings.Editor
{
    [CanEditMultipleObjects, CustomEditor(typeof(Setting<>), true)]
    public class SettingsEditor : UnityEditor.Editor
    {
        private const string
            DefaultValueFieldName   = "defaultValue",
            OnValueChangedFieldName = "valueChanged",
            SavedValueFieldName     = "Saved Value",
            ResetButtonLabel        = "Reset Settings to Default Value",
            ResetAllButtonLabel     = "Reset All Settings to Default Values";

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty(DefaultValueFieldName));

            var valueProperty = target.GetType().GetProperty("Value");
            valueProperty.SetValue(target, valueProperty.GetValue(target, null) switch
            {
                float   f => EditorGUILayout.FloatField(SavedValueFieldName, f),
                int     i => EditorGUILayout.IntField(SavedValueFieldName, i),
                bool    b => EditorGUILayout.Toggle(SavedValueFieldName, b),

                _ => throw new System.ArgumentException(),
            });

            EditorGUILayout.Space(10);

            if (GUILayout.Button(ResetButtonLabel)) PlayerPrefs.DeleteKey(target.name);
            if (GUILayout.Button(ResetAllButtonLabel)) PlayerPrefs.DeleteAll();

            EditorGUILayout.PropertyField(serializedObject.FindProperty(OnValueChangedFieldName));

            serializedObject.ApplyModifiedProperties();
        }
    }
}

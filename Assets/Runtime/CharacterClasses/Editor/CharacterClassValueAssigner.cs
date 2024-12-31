using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Attrition.CharacterClasses.Editor
{
    [CreateAssetMenu(menuName = "Attrition/Characters/Class Value Assigner")]
    public class CharacterClassValueAssigner : ScriptableObject
    {
        [TextArea(3, int.MaxValue)] [SerializeField] private string values;
        [SerializeField] private List<CharacterClass> classes;
        [Space]
        [SerializeField] private ValueMapping[] valueMappings;
    
        [Serializable]
        private class ValueMapping
        {
            public string dataInputName;
            public string scriptVariableName;
        }
        
        public void AssignData()
        {
            var rows = values.Split("\n");
            var characterInstances = rows[0]
                .Split("\t")
                .Select((name, i) => (instance: this.classes.FirstOrDefault(characterClass => characterClass.name == name), index: i))
                .Where(characterClass => characterClass.instance != null)
                .ToArray();
            
            for (int row = 1; row < rows.Length; row++)
            {
                var entries = rows[row].Split("\t");

                var mapping = valueMappings.FirstOrDefault(mapping => mapping.dataInputName == entries[0]);

                if (mapping == null)
                {
                    Debug.LogWarning($"Couldn't find value mapping entry for \"{entries[0]}\"");
                    continue;
                }

                var field = typeof(CharacterClass).GetField(mapping.scriptVariableName, BindingFlags.NonPublic | BindingFlags.Instance);
                
                if (field == null)
                {
                    Debug.LogError($"Couldn't find field called \"{mapping.scriptVariableName}\" for the value map entry for \"{mapping.dataInputName}\"");
                    continue;
                }

                foreach (var character in characterInstances)
                {
                    float value = float.Parse(entries[character.index]);
                    field.SetValue(character.instance, value);
                }
            }

            foreach (var character in characterInstances)
            {
                EditorUtility.SetDirty(character.instance);
            }
            
            AssetDatabase.SaveAssets();
        }
    }
}

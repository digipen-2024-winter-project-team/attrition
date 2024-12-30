using System;
using Attrition.Common.ScriptableVariables.DataTypes;
using UnityEngine;

namespace Attrition.Names
{
    public class NameBehaviour : MonoBehaviour
    {
        [SerializeField]
        private StringVariable displayName;
        
        public string DisplayName
        {
            get => this.displayName.Value;
            set => this.displayName.Value = value;
        }

        private void Awake()
        {
            this.displayName = ScriptableObject.CreateInstance<StringVariable>();
        }
    }
}

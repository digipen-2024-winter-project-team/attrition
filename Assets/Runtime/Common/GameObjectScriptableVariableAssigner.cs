using System;
using Attrition.Common.ScriptableVariables.ComponentTypes;
using UnityEngine;

namespace Attrition
{
    public class GameObjectScriptableVariableAssigner : MonoBehaviour
    {
        [SerializeField] private GameObjectVariable variable;

        private void Awake()
        {
            variable.Value = gameObject;
        }
    }
}

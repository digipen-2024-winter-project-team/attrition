using UnityEngine;

namespace Attrition.Common.ScriptableVariables.ComponentTypes
{
    /// <summary>
    /// A ScriptableVariable representing a reference to a Unity GameObject.
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptables/Variables/GameObject", fileName = "New GameObject Variable")]
    public class GameObjectVariable : ScriptableVariable<GameObject>
    {
    }
}

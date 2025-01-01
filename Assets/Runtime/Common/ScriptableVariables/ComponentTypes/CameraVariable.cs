using UnityEngine;

namespace Attrition.Common.ScriptableVariables.ComponentTypes
{
    /// <summary>
    /// A ScriptableVariable representing a reference to a Unity Camera.
    /// </summary>
    [CreateAssetMenu(menuName = "Scriptables/Variables/Camera", fileName = "New Camera Variable")]
    public class CameraVariable : ScriptableVariable<UnityEngine.Camera>
    {
    }
}

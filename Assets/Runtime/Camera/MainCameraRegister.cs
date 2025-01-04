using Attrition.Common.ScriptableVariables.ComponentTypes;
using UnityEngine;

namespace Attrition.Camera
{
    public class MainCameraRegister : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera mainCamera;
        [SerializeField] private CameraVariable variable;

        private void Awake()
        {
            variable.Value = mainCamera;
        }
    }
}

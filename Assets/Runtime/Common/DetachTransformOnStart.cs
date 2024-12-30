using UnityEngine;

namespace Attrition
{
    public class DetachTransformOnStart : MonoBehaviour
    {
        [SerializeField] private Transform detach;

        private void Start()
        {
            detach.SetParent(null, true);
        }
    }
}

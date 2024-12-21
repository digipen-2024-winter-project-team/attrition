using UnityEngine;
using UnityEngine.Serialization;

namespace Attrition.DamageSystem
{
    public class DamageRedirect : MonoBehaviour
    {
        [SerializeField] private GameObject target;

        public GameObject Target => target;
    }
}

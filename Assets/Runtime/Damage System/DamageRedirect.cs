using UnityEngine;

namespace Attrition.DamageSystem
{
    public class DamageRedirect : MonoBehaviour
    {
        [SerializeField] private GameObject target;

        public GameObject Target => this.target;
    }
}

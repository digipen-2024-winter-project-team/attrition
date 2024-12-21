using UnityEngine;

namespace Attrition.Damge_System
{
    public class DamageRedirect : MonoBehaviour
    {
        [SerializeField] private GameObject target;

        public GameObject Target => this.target;
    }
}

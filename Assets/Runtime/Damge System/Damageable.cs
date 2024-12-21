using System;
using UnityEngine;
using UnityEngine.Events;

namespace Attrition.Damge_System
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] private UnityEvent<DamageInfo> damaged;
        public event Action<DamageInfo> Damaged;

        public DamageResult[] TakeDamage(DamageInfo damageInfo)
        {
            this.Damaged?.Invoke(damageInfo);
            this.damaged.Invoke(damageInfo);

            return damageInfo.GetResults();
        }

        public static bool Find(GameObject gameObject, out Damageable damageable)
        {
            if (gameObject.TryGetComponent(out DamageRedirect redirect))
            {
                gameObject = redirect.Target;
            }

            return gameObject.TryGetComponent(out damageable);
        }
    }
}

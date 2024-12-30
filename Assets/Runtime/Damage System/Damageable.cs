using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;

namespace Attrition.DamageSystem
{
    public class Damageable : MonoBehaviour
    {
        [Tooltip("Callback for every time damage is received, whether it was ignored or not.")]
        [SerializeField] private UnityEvent<DamageInfo> damaged;
        public event Action<DamageInfo> Damaged;

        [Tooltip("Callback for when damage is actually taken and not ignored, in case the receiver is invincible for example.")]
        [SerializeField] private UnityEvent<DamageInfo> damageTaken;
        
        public DamageResult[] TakeDamage(DamageInfo damageInfo)
        {
            Damaged?.Invoke(damageInfo);
            damaged.Invoke(damageInfo);

            if (damageInfo.GetResults().Any(result => !result.ignored))
            {
                damageTaken.Invoke(damageInfo);
            }
            
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

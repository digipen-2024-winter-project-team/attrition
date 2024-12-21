using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Attrition.DamageSystem
{
    public class Durability : MonoBehaviour
    {
        [SerializeField] private float hitpoints;
        [Space]
        [SerializeField] private Damageable damageable;
        [FormerlySerializedAs("maxHealth")]
        [Space]
        [SerializeField] private float maxHitpoints;
        [SerializeField] private bool startWithMaxHitpoints = true;
        [SerializeField] private float invincibilityDuration;
        [SerializeField] private CombatTeam team;
        [Space]
        [SerializeField] private UnityEvent<DamageInfo> destroyed;
        public event Action<DamageInfo> Destroyed;
        
        private float invincibilityExpiration;

        private void Awake()
        {
            if (startWithMaxHitpoints)
            {
                hitpoints = maxHitpoints;
            }
            
            damageable.Damaged += OnDamaged;
        }

        public bool Invincible => Time.time < invincibilityExpiration;
        
        private void OnDamaged(DamageInfo damageInfo)
        {
            if (hitpoints == 0) return;
            
            float value = damageInfo.Receive(Invincible, team, gameObject);

            if (Invincible)
            {
                return;
            }

            hitpoints = Mathf.MoveTowards(hitpoints, 0, value);
            invincibilityExpiration = Time.time + invincibilityDuration;

            if (hitpoints == 0)
            {
                Destroyed?.Invoke(damageInfo);
                destroyed.Invoke(damageInfo);
            }
        }
    }
}

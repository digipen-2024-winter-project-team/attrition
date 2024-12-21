using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Attrition.Damge_System
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
            if (this.startWithMaxHitpoints)
            {
                this.hitpoints = this.maxHitpoints;
            }
            
            this.damageable.Damaged += this.OnDamaged;
        }

        public bool Invincible => Time.time < this.invincibilityExpiration;
        
        private void OnDamaged(DamageInfo damageInfo)
        {
            if (this.hitpoints == 0) return;
            
            float value = damageInfo.Receive(this.Invincible, this.team, this.gameObject);

            if (this.Invincible)
            {
                return;
            }

            this.hitpoints = Mathf.MoveTowards(this.hitpoints, 0, value);
            this.invincibilityExpiration = Time.time + this.invincibilityDuration;

            if (this.hitpoints == 0)
            {
                this.Destroyed?.Invoke(damageInfo);
                this.destroyed.Invoke(damageInfo);
            }
        }
    }
}

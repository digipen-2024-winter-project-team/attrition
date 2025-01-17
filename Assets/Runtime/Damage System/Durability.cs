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
        private bool permanentInvincibility;

        public void SetInvincible(bool invincible)
        {
            permanentInvincibility = invincible;
        }

        public void AddInvincibilityDuration(float duration)
        {
            invincibilityExpiration = Mathf.Max(invincibilityExpiration, Time.time + duration);
        }
        
        private void Awake()
        {
            if (this.startWithMaxHitpoints)
            {
                this.hitpoints = this.maxHitpoints;
            }

            permanentInvincibility = false;
        }

        private void OnEnable()
        {
            this.damageable.Damaged += this.OnDamaged;
        }

        private void OnDisable()
        {
            damageable.Damaged -= OnDamaged;
        }

        public bool Invincible => Time.time < this.invincibilityExpiration || permanentInvincibility;
        
        private void OnDamaged(DamageInfo damageInfo)
        {
            if (this.hitpoints == 0) return;
            
            float value = damageInfo.Receive(this.Invincible, this.team, this.gameObject);

            if (this.Invincible)
            {
                return;
            }

            this.hitpoints = Mathf.MoveTowards(this.hitpoints, 0, value);
            AddInvincibilityDuration(invincibilityDuration);

            if (this.hitpoints == 0)
            {
                this.Destroyed?.Invoke(damageInfo);
                this.destroyed.Invoke(damageInfo);
            }
        }
    }
}

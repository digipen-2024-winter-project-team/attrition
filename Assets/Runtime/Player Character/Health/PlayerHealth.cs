using System;
using Attrition.Common.ScriptableVariables.DataTypes;
using Attrition.DamageSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Attrition.PlayerCharacter.Health
{
    public class PlayerHealth : Player.Component
    {
        [SerializeField] private float maxHitpoints;
        [SerializeField] private Damageable damageable;
        [SerializeField] private float invincibilityDuration;
        [SerializeField] private CombatTeam team;
        [SerializeField] private float deathTimerDuration;
        [SerializeField] private float deathTimerLossPerHitpoint;
        [SerializeField] private UnityEvent<DamageInfo> enteringDying;
        [SerializeField] private UnityEvent died;
        [Header("References")]
        [SerializeField] private FloatVariable hitpoints;
        [SerializeField] private FloatVariable deathTimer;
        [SerializeField] private BoolVariable dead;
        [SerializeField] private BoolVariable invincible;
        
        private float invincibilityExpiration;
        
        public bool Dead => dead.Value;
        
        public void AddInvincibilityTime(float duration) =>
            invincibilityExpiration = Mathf.Max(invincibilityExpiration, Time.time + duration);
        
        private void Awake()
        {
            hitpoints.Value = maxHitpoints;
            deathTimer.Value = deathTimerDuration;
            dead.Value = false;
            
            damageable.Damaged += OnDamaged;
        }

        private void OnDamaged(DamageInfo damageInfo)
        {
            if (Dead) return;
            
            float value = damageInfo.Receive(invincible.Value, team, gameObject);

            if (invincible.Value)
            {
                return;
            }

            AddInvincibilityTime(invincibilityDuration);
            
            if (hitpoints.Value > 0)
            {
                hitpoints.Value = Mathf.MoveTowards(hitpoints.Value, 0, value);

                if (hitpoints.Value == 0)
                {
                    enteringDying.Invoke(damageInfo);
                }
            }
            else
            {
                deathTimer.Value = Mathf.MoveTowards(deathTimer.Value, 0, value * deathTimerLossPerHitpoint);

                if (deathTimer.Value == 0)
                {
                    Die();
                }
            }
        }

        private void Update()
        {
            if (hitpoints.Value == 0 && deathTimer.Value > 0)
            {
                deathTimer.Value = Mathf.MoveTowards(deathTimer.Value, 0, Time.deltaTime);

                if (deathTimer.Value == 0)
                {
                    Die();
                }
            }

            invincible.Value = Time.time < invincibilityExpiration;
        }

        public void Die()
        {
            hitpoints.Value = 0;
            deathTimer.Value = 0;
            dead.Value = true;
            
            died.Invoke();
        }
    }
}

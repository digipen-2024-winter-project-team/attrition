using System;
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
        [Header("Debug UI")]
        [SerializeField] private RectTransform uiParent;
        [SerializeField] private TextMeshProUGUI healthUI;
        [SerializeField] private Transform uiAnchor;
        
        private float invincibilityExpiration;
        private float hitpoints;
        private float deathTimer;
        
        public bool Invincible => Time.time < invincibilityExpiration;

        public void AddInvincibilityTime(float duration) =>
            invincibilityExpiration = Mathf.Max(invincibilityExpiration, Time.time + duration);
        
        private void Awake()
        {
            hitpoints = maxHitpoints;
            deathTimer = deathTimerDuration;
            
            damageable.Damaged += OnDamaged;
        }

        private void OnDamaged(DamageInfo damageInfo)
        {
            float value = damageInfo.Receive(Invincible, team, gameObject);

            if (Invincible)
            {
                return;
            }

            AddInvincibilityTime(invincibilityDuration);
            
            if (hitpoints > 0)
            {
                hitpoints = Mathf.MoveTowards(hitpoints, 0, value);
            }
            else
            {
                deathTimer = Mathf.MoveTowards(deathTimer, 0, value * deathTimerLossPerHitpoint);
            }
        }

        private void Update()
        {
            if (hitpoints == 0)
            {
                deathTimer = Mathf.MoveTowards(deathTimer, 0, Time.deltaTime);
            }

            if (deathTimer == 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            string health = hitpoints > 0
                ? $"{hitpoints:0} HP"
                : $"{deathTimer:0.00}s";
            
            healthUI.text = $"<mspace=0em>{health}\n{(Invincible ? "Invincible" : "Vulnerable")}";
            healthUI.color = Invincible
                ? Color.red
                : Color.white;
        }

        private void LateUpdate()
        {
            uiParent.localPosition = GetUVPosition(uiAnchor.position) * ((RectTransform)uiParent.parent).rect.size;
        }
    }
}

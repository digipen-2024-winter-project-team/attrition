using System.Collections.Generic;
using System.Linq;
using Attrition.Common.Physics;
using UnityEngine;

namespace Attrition.DamageSystem
{
    public class SimpleDamageSource : MonoBehaviour
    {
        [SerializeField] private CollisionAggregate collision;
        [SerializeField] private float damageValue;
        [SerializeField] private CombatTeam combatTeam;
        [SerializeField] private bool logHits;

        public void SetActive(bool active)
        {
            hits.Clear();
            this.active = active;
        }
        
        [ContextMenu("Activate Simple Damage Source")]
        public void Activate()
        {
            SetActive(true);
        }

        [ContextMenu("Deactivate Simple Damage Source")]
        public void Deactivate()
        {
            SetActive(false);
        }
        
        private List<Damageable> hits;
        
        private bool active;

        private void Awake()
        {
            hits = new();
        }

        private void Update()
        {
            if (!active) return;
            
            foreach (var collider in collision.Colliders)
            {
                if (!Damageable.Find(collider.gameObject, out var damageable)
                    || hits.Contains(damageable))
                {
                    continue;
                } 
                    
                var damageInfo = new DamageInfo(damageValue, gameObject, combatTeam);
                var results = damageable.TakeDamage(damageInfo);

                if (logHits)
                {
                    Debug.Log(damageInfo);
                }
                    
                if (results.Any(result => !result.ignored))
                {
                    hits.Add(damageable);
                }
            }
        }
    }
}

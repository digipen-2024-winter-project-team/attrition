using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Attrition.Common.Physics;
using Attrition.DamageSystem;

namespace Attrition.PlayerCharacter
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private CollisionAggregate hitboxCollision;
        [SerializeField] private CombatTeam team;
        [SerializeField] private float damageValue;
        [SerializeField] private InputActionReference attackAction;
        
        private void Update()
        {
            if (attackAction.action.WasPerformedThisFrame())
            {
                Attack();
            }
        }

        private void Attack()
        {
            foreach (var collision in hitboxCollision.Colliders)
            {
                if (Damageable.Find(collision.gameObject, out var damageable))
                {
                    var results = damageable.TakeDamage(new(damageValue, gameObject, team));
                }
            }
        }
    }
}

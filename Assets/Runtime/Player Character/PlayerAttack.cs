using Attrition.Common.Physics;
using Attrition.DamageSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Attrition.Player_Character
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private CollisionAggregate hitboxCollision;
        [SerializeField] private CombatTeam team;
        [SerializeField] private float damageValue;
        [SerializeField] private InputActionReference attackAction;
        
        private void Update()
        {
            if (this.attackAction.action.WasPerformedThisFrame())
            {
                this.Attack();
            }
        }

        private void Attack()
        {
            foreach (var collision in this.hitboxCollision.Colliders)
            {
                if (Damageable.Find(collision.gameObject, out var damageable))
                {
                    var results = damageable.TakeDamage(new(this.damageValue, this.gameObject, this.team));
                }
            }
        }
    }
}

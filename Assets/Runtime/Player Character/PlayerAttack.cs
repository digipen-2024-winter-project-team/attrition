using Attrition.Common.Events.SerializedEvents;
using UnityEngine;
using UnityEngine.InputSystem;
using Attrition.Common.Physics;
using Attrition.DamageSystem;
using UnityEngine.VFX;

namespace Attrition.PlayerCharacter
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private CollisionAggregate hitboxCollision;
        [SerializeField] private CombatTeam team;
        [SerializeField] private float damageValue;
        [SerializeField] private InputActionReference attackAction;
        [SerializeField] private VisualEffect attackParticle;
        [SerializeField] private SerializedEvent attacked;

        public IReadOnlySerializedEvent Attacked => attacked;
        
        private void Update()
        {
            if (attackAction.action.WasPerformedThisFrame())
            {
                Attack();
            }
        }

        private void Attack()
        {
            attacked.Invoke();
            
            attackParticle.Play();
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

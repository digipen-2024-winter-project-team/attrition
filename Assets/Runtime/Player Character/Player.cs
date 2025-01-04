using Attrition.Common.ScriptableVariables.DataTypes;
using Unity.Cinemachine;
using UnityEngine;
using Attrition.PlayerCharacter.Health;
using Attrition.PlayerCharacter.Targeting;

namespace Attrition.PlayerCharacter
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private PlayerTargeting targeting;
        [SerializeField] private CinemachineBrain cinemachineBrain;
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private new CapsuleCollider collider;
        [SerializeField] private PlayerHealth health;
        [SerializeField] private BoolVariable paused;
        [SerializeField] private PlayerAttack attack;
        
        public class Component : MonoBehaviour
        {
            [SerializeField] private Player player;

            protected Player Player => player;
            protected PlayerMovement Movement => player.movement;
            protected PlayerTargeting Targeting => player.targeting;
            protected CinemachineBrain CinemachineBrain => player.cinemachineBrain;
            protected Rigidbody Rigidbody => player.rigidbody;
            protected Collider Collider => player.collider;
            protected PlayerHealth Health => player.health;
            protected PlayerAttack Attack => player.attack;
            protected BoolVariable Paused => player.paused;
            
            protected Vector2 GetUVPosition(Vector3 position) =>
                (Vector2)CinemachineBrain.OutputCamera.WorldToViewportPoint(position) - Vector2.one / 2f;
        }
    }
}

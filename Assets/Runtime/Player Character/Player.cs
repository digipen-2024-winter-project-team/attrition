using Unity.Cinemachine;
using UnityEngine;

namespace Attrition.Player_Character
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private PlayerTargeting targeting;
        [SerializeField] private CinemachineBrain cinemachineBrain;
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private new CapsuleCollider collider;
        
        public class Component : MonoBehaviour
        {
            [SerializeField] private Player player;

            protected Player Player => this.player;
            protected PlayerMovement Movement => this.player.movement;
            protected PlayerTargeting Targeting => this.player.targeting;
            protected CinemachineBrain CinemachineBrain => this.player.cinemachineBrain;
            protected Rigidbody Rigidbody => this.player.rigidbody;
            protected Collider Collider => this.player.collider;
        }
    }
}

using Unity.Cinemachine;
using UnityEngine;

namespace Attrition.PlayerCharacter
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

            protected Player Player => player;
            protected PlayerMovement Movement => player.movement;
            protected PlayerTargeting Targeting => player.targeting;
            protected CinemachineBrain CinemachineBrain => player.cinemachineBrain;
            protected Rigidbody Rigidbody => player.rigidbody;
            protected Collider Collider => player.collider;
        }
    }
}

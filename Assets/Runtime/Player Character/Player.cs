using Attrition.Camera;
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
            protected BoolVariable Paused => player.paused;
            
            protected Vector2 GetUVPosition(Vector3 position) 
                => CinemachineBrain.OutputCamera.GetNormalizedScreenPosition(position);
        }
    }
}

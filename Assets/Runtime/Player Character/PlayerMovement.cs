using System.Collections.Generic;
using Attrition.Common.Physics;
using UnityEngine;
using UnityEngine.InputSystem;
using Attrition.Common;
using Attrition.Common.Events.SerializedEvents;

namespace Attrition.PlayerCharacter
{
    public class PlayerMovement : Player.Component
    {
        [SerializeField] private float turnSpeed;
        [SerializeField] private float minInputAngleCameraSwitch;
        [Header("Walking")] 
        [SerializeField] private InputActionReference movement;
        [SerializeField] private float walkSpeed;
        [SerializeField] private float walkAcceleration;
        [SerializeField] private float slopeCheckWidth;
        [SerializeField] private float groundSuckSpeed;
        [Header("Dodging")]
        [SerializeField] private InputActionReference dodge;
        [SerializeField] private int dodgeDirections = 8;
        [SerializeField] private float dodgeDistance;
        [SerializeField] private SmartCurve dodgeSpeedCurve;
        [SerializeField] private float dodgeInvincibilityDuration;
        [SerializeField] private float dodgeCooldownDuration;
        [Header("Falling")]
        [SerializeField] private float gravity;
        [SerializeField] private float maxFallSpeed;
        [SerializeField] private CollisionAggregate ground;
        [SerializeField] private float maxFallWalkSpeed;
        [SerializeField] private float fallWalkAcceleration;
        [Header("State Machine")]
        [SerializeField] private SerializedEvent<ValueChangeArgs<MoveState>> stateChanged;
        
        private Vector3 cameraForward;
        private Vector2 previousMoveInput;
        private float previousInputAngle;
        private float dodgeCooldownExpiration;

        private Vector3 moveDirection;
        private Quaternion slopeRotation;

        #region Helpers
        
        
        public Vector3 MoveDirection => moveDirection;

        private Vector3 Velocity
        {
            get => Rigidbody.linearVelocity;
            set => Rigidbody.linearVelocity = value;
        }

        private Vector3 SlopeVelocity
        {
            get => Quaternion.Inverse(slopeRotation) * Velocity;
            set => Velocity = slopeRotation * value;
        }

        private Vector3 LocalSlopeVelocity
        {
            get => transform.InverseTransformDirection(SlopeVelocity);
            set => SlopeVelocity = transform.TransformDirection(value);
        }
        
        public float SpeedPercent => new Vector2(this.Velocity.x, this.Velocity.z).magnitude / this.walkSpeed;

        public Vector2 DirectionalSpeedPercent
        {
            get
            {
                Vector3 localSlopeVelocity = LocalSlopeVelocity;
                return new Vector2(localSlopeVelocity.x, localSlopeVelocity.z) / walkSpeed;
            }
        }

        private Dictionary<IState, MoveState> moveStates;
        public enum MoveState
        {
            Grounded,
            Falling,
            Dodging,
        }

        public IReadOnlySerializedEvent<ValueChangeArgs<MoveState>> MoveStateChanged => stateChanged;
        
        public MoveState CurrentMoveState => moveStates[stateMachine.currentState];
        
        #endregion
        
        #region Behavior
        
        private void Start()
        {
            InitializeStateMachine();
        }

        private void Update()
        {
            if (Health.Dead)
            {
                Velocity = Vector3.zero;
                return;
            }
            
            UpdatePhysicalState();
            UpdateInput();

            stateMachine.Update(Time.deltaTime);

            UpdateRotation();
        }

        private void UpdatePhysicalState()
        {
            slopeRotation = Physics.BoxCast(transform.position, new(slopeCheckWidth / 2f, 0.05f, slopeCheckWidth / 2f), Vector3.down, out var hit,
                transform.rotation, Mathf.Infinity, GameInfo.Ground.Mask) 
                ? Quaternion.FromToRotation(Vector3.up, hit.normal)
                : Quaternion.identity;
        }

        private void UpdateInput()
        {
            Vector2 moveInput = movement.action.ReadValue<Vector2>().normalized;

            float inputAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            
            if (Mathf.DeltaAngle(inputAngle, previousInputAngle) > minInputAngleCameraSwitch
                || previousMoveInput != moveInput)
            {
                cameraForward = CinemachineBrain.transform.forward;
                cameraForward.y = 0;
                cameraForward.Normalize();
                
                previousInputAngle = inputAngle;
                previousMoveInput = moveInput;
            }

            if (moveInput != Vector2.zero)
            {
                moveDirection = Quaternion.FromToRotation(Vector3.forward, cameraForward) *
                                new Vector3(moveInput.x, 0, moveInput.y);
            }
        }
        
        private void UpdateRotation()
        {
            Vector3 targetDirection = Targeting.Target != null
                ? Targeting.Target.position - transform.position
                : moveDirection;
            
            float rotation = Rigidbody.rotation.eulerAngles.y;
            float targetRotation = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;
            rotation = Mathf.MoveTowardsAngle(rotation, targetRotation, turnSpeed * Time.deltaTime);

            if (moveDirection != Vector3.zero || Targeting.Target != null)
            {
                Rigidbody.rotation = Quaternion.Euler(0, rotation, 0);
            }
        }
        
        #endregion
        
        #region State Machine
        
        private Grounded grounded;
        private Falling falling;
        private Dodging dodging;
        private StateMachine stateMachine;

        private void InitializeStateMachine()
        {
            grounded = new(this);
            falling = new(this);
            dodging = new(this);

            moveStates = new()
            {
                { grounded, MoveState.Grounded },
                { falling, MoveState.Falling },
                { dodging, MoveState.Dodging },
            };

            TransitionDelegate

                canGround = () => ground.Touching,
                canFall = () => !ground.Touching,

                canDodge = () => ground.Touching 
                                 && dodge.action.WasPerformedThisFrame() && !Paused.Value
                                 && Time.time > dodgeCooldownExpiration,
                canEndDodge = () => dodgeSpeedCurve.Done;

            stateMachine = new(grounded, new()
            {
                { grounded, new() 
                {
                    new(falling, canFall),
                    new(dodging, canDodge),
                }},
                
                { falling, new()
                {
                    new(grounded, canGround),
                }},
                
                { dodging, new()
                {
                    new(grounded, canEndDodge),
                    new(falling, canFall),
                }},
            });

            stateMachine.OnTransition += (from, to) =>
            {
                var args = new ValueChangeArgs<MoveState>
                {
                    From = moveStates[from],
                    To = moveStates[to],
                };
                
                stateChanged.Invoke(args);
            };
        }
        
        private class State : State<PlayerMovement>
        {
            protected State(PlayerMovement context) : base(context) { }

            protected Transform Target => context.Targeting.Target;

            protected void Fall()
            {
                Vector3 velocity = context.Velocity;
                velocity.y = Mathf.MoveTowards(velocity.y, -context.maxFallSpeed, context.gravity * Time.deltaTime);
                context.Velocity = velocity;
            }

            protected void Walk(float moveSpeed, float acceleration)
            {
                Vector3 velocity = context.SlopeVelocity;
                Vector2 velocity2 = new(velocity.x, velocity.z);
                Vector2 input = new Vector2(context.moveDirection.x, context.moveDirection.z) *
                                (context.movement.action.IsPressed() && !context.Paused.Value ? 1 : 0);
                
                velocity2 = Vector2.MoveTowards(velocity2, input * moveSpeed, acceleration * Time.deltaTime);

                context.SlopeVelocity = new(velocity2.x, velocity.y, velocity2.y);
            }
        }

        private class Grounded : State
        {
            public Grounded(PlayerMovement context) : base(context) { }

            public override void Update()
            {
                Walk(context.walkSpeed, context.walkAcceleration);

                Vector3 velocity = context.SlopeVelocity;
                velocity.y = -context.groundSuckSpeed;
                context.SlopeVelocity = velocity;
                
                base.Update();
            }
        }

        private class Falling : State
        {
            public Falling(PlayerMovement context) : base(context) { }

            public override void Update()
            {
                Walk(context.maxFallWalkSpeed, context.walkAcceleration);
                Fall();
                
                base.Update();
            }
        }

        private class Dodging : State
        {
            public Dodging(PlayerMovement context) : base(context) { }

            private Vector3 dodgeDirection;
            
            public override void Enter()
            {
                base.Enter();

                context.Health.AddInvincibilityTime(context.dodgeInvincibilityDuration);
                
                context.dodgeSpeedCurve.Start();

                Vector3 moveDirection = context.moveDirection;
                dodgeDirection = moveDirection;

                if (context.movement.action.WasPressedThisFrame())
                {
                    dodgeDirection = moveDirection;
                }
                else if (Target != null)
                {                
                    Vector3 toTarget = Target.position - context.transform.position;
                    float toTargetAngle = Mathf.Atan2(toTarget.z, toTarget.x) * Mathf.Rad2Deg;

                    float bestDot = -1;
                    for (int i = 0; i < context.dodgeDirections; i++)
                    {
                        float angle = (toTargetAngle + (float)i / context.dodgeDirections * 360f) * Mathf.Deg2Rad;
                        Vector3 vec = new(Mathf.Cos(angle), 0, Mathf.Sin(angle));
                        float dot = Vector3.Dot(moveDirection, vec);

                        if (dot > bestDot)
                        {
                            bestDot = dot;
                            dodgeDirection = vec;
                        }
                    }
                }
            }

            public override void Update()
            {
                Vector3 dodgeVelocity = dodgeDirection * (context.dodgeSpeedCurve.Evaluate(1) * context.dodgeDistance);
                context.SlopeVelocity = new(dodgeVelocity.x, -context.groundSuckSpeed, dodgeVelocity.z);
                
                base.Update();
            }

            public override void Exit()
            {
                context.dodgeCooldownExpiration = Time.time + context.dodgeCooldownDuration;
                
                base.Exit();
            }
        }
        
        #endregion
    }
}

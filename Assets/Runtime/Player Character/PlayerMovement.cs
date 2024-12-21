using Attrition.Common;
using Attrition.Common.Physics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Attrition.Player_Character
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
        [Header("Falling")] [SerializeField] private float gravity;
        [SerializeField] private float maxFallSpeed;
        [SerializeField] private CollisionAggregate ground;
        [SerializeField] private float maxFallWalkSpeed;
        [SerializeField] private float fallWalkAcceleration;
        
        private Vector3 cameraForward;
        private Vector2 previousMoveInput;
        private float previousInputAngle;

        private Vector3 moveDirection;
        public Vector3 MoveDirection => this.moveDirection;

        private Quaternion slopeRotation;

        private Vector3 Velocity
        {
            get => this.Rigidbody.linearVelocity;
            set => this.Rigidbody.linearVelocity = value;
        }

        private Vector3 SlopeVelocity
        {
            get => Quaternion.Inverse(this.slopeRotation) * this.Velocity;
            set => this.Velocity = this.slopeRotation * value;
        }

        private void Start()
        {
            this.InitializeStateMachine();
        }

        private void Update()
        {
            this.UpdatePhysicalState();
            this.UpdateInput();

            this.stateMachine.Update(Time.deltaTime);

            this.UpdateRotation();
        }

        private void UpdatePhysicalState()
        {
            this.slopeRotation = Physics.BoxCast(this.transform.position, new(this.slopeCheckWidth / 2f, 0.05f, this.slopeCheckWidth / 2f), Vector3.down, out var hit,
                this.transform.rotation, Mathf.Infinity, GameInfo.Ground.Mask) 
                ? Quaternion.FromToRotation(Vector3.up, hit.normal)
                : Quaternion.identity;
            
            Debug.DrawRay(hit.point, hit.normal, Color.yellow);
            
            Debug.DrawRay(this.transform.position, this.SlopeVelocity, Color.green);
        }

        private void UpdateInput()
        {
            Vector2 moveInput = this.movement.action.ReadValue<Vector2>().normalized;

            float inputAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            
            if (Mathf.DeltaAngle(inputAngle, this.previousInputAngle) > this.minInputAngleCameraSwitch
                || this.previousMoveInput != moveInput)
            {
                this.cameraForward = this.CinemachineBrain.transform.forward;
                this.cameraForward.y = 0;
                
                this.previousInputAngle = inputAngle;
                this.previousMoveInput = moveInput;
            }

            if (moveInput != Vector2.zero)
            {
                this.moveDirection = Quaternion.FromToRotation(Vector3.forward, this.cameraForward) *
                                     new Vector3(moveInput.x, 0, moveInput.y);
            }
        }
        
        private void UpdateRotation()
        {
            Vector3 targetDirection = this.Targeting.Target != null
                ? this.Targeting.Target.position - this.transform.position
                : this.moveDirection;
            
            float rotation = this.Rigidbody.rotation.eulerAngles.y;
            float targetRotation = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;
            rotation = Mathf.MoveTowardsAngle(rotation, targetRotation, this.turnSpeed * Time.deltaTime);

            if (this.moveDirection != Vector3.zero || this.Targeting.Target != null)
            {
                this.Rigidbody.rotation = Quaternion.Euler(0, rotation, 0);
            }
        }
        
        #region State Machine
        
        private Grounded grounded;
        private Falling falling;
        private Dodging dodging;
        private StateMachine stateMachine;

        private void InitializeStateMachine()
        {
            this.grounded = new(this);
            this.falling = new(this);
            this.dodging = new(this);

            TransitionDelegate

                canGround = () => this.ground.Touching,
                canFall = () => !this.ground.Touching,

                canDodge = () => this.ground.Touching && this.dodge.action.WasPerformedThisFrame(),
                canEndDodge = () => this.dodgeSpeedCurve.Done;

            this.stateMachine = new(this.grounded, new()
            {
                { this.grounded, new() 
                {
                    new(this.falling, canFall),
                    new(this.dodging, canDodge),
                }},
                
                { this.falling, new()
                {
                    new(this.grounded, canGround),
                }},
                
                { this.dodging, new()
                {
                    new(this.grounded, canEndDodge),
                    new(this.falling, canFall),
                }},
            });
        }
        
        private class State : State<PlayerMovement>
        {
            protected State(PlayerMovement context) : base(context) { }

            protected Transform Target => this.context.Targeting.Target;

            protected void Fall()
            {
                Vector3 velocity = this.context.Velocity;
                velocity.y = Mathf.MoveTowards(velocity.y, -this.context.maxFallSpeed, this.context.gravity * Time.deltaTime);
                this.context.Velocity = velocity;
            }

            protected void Walk(float moveSpeed, float acceleration)
            {
                Vector3 velocity = this.context.SlopeVelocity;
                Vector2 velocity2 = new(velocity.x, velocity.z);
                Vector2 input = new Vector2(this.context.moveDirection.x, this.context.moveDirection.z) *
                                (this.context.movement.action.IsPressed() ? 1 : 0);
                
                velocity2 = Vector2.MoveTowards(velocity2, input * moveSpeed, acceleration * Time.deltaTime);

                this.context.SlopeVelocity = new(velocity2.x, velocity.y, velocity2.y);
            }
        }

        private class Grounded : State
        {
            public Grounded(PlayerMovement context) : base(context) { }

            public override void Update()
            {
                this.Walk(this.context.walkSpeed, this.context.walkAcceleration);

                Vector3 velocity = this.context.SlopeVelocity;
                velocity.y = -this.context.groundSuckSpeed;
                this.context.SlopeVelocity = velocity;
                
                base.Update();
            }
        }

        private class Falling : State
        {
            public Falling(PlayerMovement context) : base(context) { }

            public override void Update()
            {
                this.Walk(this.context.maxFallWalkSpeed, this.context.walkAcceleration);
                this.Fall();
                
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

                this.context.dodgeSpeedCurve.Start();

                Vector3 moveDirection = this.context.moveDirection;
                this.dodgeDirection = this.context.transform.forward;

                if (this.context.movement.action.WasPressedThisFrame())
                {
                    this.dodgeDirection = moveDirection;
                }
                else if (this.Target != null)
                {                
                    Vector3 toTarget = this.Target.position - this.context.transform.position;
                    float toTargetAngle = Mathf.Atan2(toTarget.z, toTarget.x) * Mathf.Rad2Deg;

                    float bestDot = -1;
                    for (int i = 0; i < this.context.dodgeDirections; i++)
                    {
                        float angle = (toTargetAngle + (float)i / this.context.dodgeDirections * 360f) * Mathf.Deg2Rad;
                        Vector3 vec = new(Mathf.Cos(angle), 0, Mathf.Sin(angle));
                        float dot = Vector3.Dot(moveDirection, vec);

                        if (dot > bestDot)
                        {
                            bestDot = dot;
                            this.dodgeDirection = vec;
                        }
                    }
                }
            }

            public override void Update()
            {
                Vector3 dodgeVelocity = this.dodgeDirection * (this.context.dodgeSpeedCurve.Evaluate(1) * this.context.dodgeDistance);
                this.context.SlopeVelocity = new(dodgeVelocity.x, -this.context.groundSuckSpeed, dodgeVelocity.z);
                
                base.Update();
            }
        }
        
        #endregion
    }
}

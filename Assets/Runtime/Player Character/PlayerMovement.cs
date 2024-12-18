using UnityEngine;
using UnityEngine.InputSystem;

namespace Attrition.PlayerCharacter
{
    public class PlayerMovement : Player.Component
    {
        [SerializeField] private InputActionReference movement;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private float turnSpeed;
        [SerializeField] private InputActionReference dodge;
        
        private Vector2 pressedLast;
        private (Vector3 x, Vector3 y) cameraForward;

        private Vector3 moveDirection;
        public Vector3 MoveDirection => moveDirection;

        private void Update()
        {
            // input
            
            Vector2 moveInput = movement.action.ReadValue<Vector2>().normalized;
            
            Vector3 InputDirection(float input, ref float pressedLast, ref Vector3 cameraForward, Vector3 axis)
            {
                if (input != pressedLast)
                {
                    cameraForward = CinemachineBrain.transform.forward;
                    cameraForward.y = 0;
                }

                pressedLast = input;

                if (cameraForward == Vector3.zero) return Vector3.zero;

                return Quaternion.FromToRotation(Vector3.forward, cameraForward) * axis * input;
            }

            // world direction input
            moveDirection
                = InputDirection(moveInput.x, ref pressedLast.x, ref cameraForward.x, Vector3.right)
                  + InputDirection(moveInput.y, ref pressedLast.y, ref cameraForward.y, Vector3.forward);

            Rigidbody.linearVelocity = Vector3.MoveTowards(Rigidbody.linearVelocity, moveDirection * moveSpeed, acceleration * Time.deltaTime);
            
            if (Targeting.Target != null)
            {
                Vector3 toTarget = (Targeting.Target.position - transform.position).normalized;
                RotateTowards(toTarget);

                Vector2 toTargetFlat = new(toTarget.x, toTarget.z);

                Vector2 toTargetFlatPerp = Vector2.Perpendicular(toTargetFlat);
                Vector3 totTargetPerp = new(toTargetFlatPerp.x, 0, toTargetFlatPerp.y);
                
                Debug.DrawRay(transform.position, toTarget * 3, Color.green);
                Debug.DrawRay(transform.position, -toTarget * 3, Color.green);
                Debug.DrawRay(transform.position, totTargetPerp * 3, Color.green);
                Debug.DrawRay(transform.position, -totTargetPerp * 3, Color.green);

                float toTargetAngle = Mathf.Atan2(toTarget.z, toTarget.x) * Mathf.Rad2Deg;
                float selfAngle = Rigidbody.rotation.eulerAngles.y;
                float moveDirectionAngle = Mathf.Atan2(moveDirection.z, moveDirection.x) * Mathf.Rad2Deg;

                DrawAngle(toTargetAngle, Color.yellow);
                DrawAngle(moveDirectionAngle, Color.cyan);
                
                void DrawAngle(float angle, Color color, float length = 1) => Debug.DrawRay(transform.position, 
                    new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * length, 0, Mathf.Sin(angle * Mathf.Deg2Rad)), color);
                
                float angleIncrements = 90;
                float offset = 360 - selfAngle / 2f;
                moveDirectionAngle = Mathf.Round((moveDirectionAngle + selfAngle) / angleIncrements) * angleIncrements - selfAngle;
                DrawAngle(moveDirectionAngle, Color.red, 2);

                Vector3 dodgeDirection = toTarget * moveInput.y + totTargetPerp * moveInput.x;
                Debug.DrawRay(transform.position, dodgeDirection * 3, Color.red);
                
                moveDirectionAngle *= Mathf.Deg2Rad;

                Vector3 quantizedMoveDirection = new(Mathf.Cos(moveDirectionAngle), 0, Mathf.Sin(moveDirectionAngle));
            }
            else if (moveInput != Vector2.zero)
            {
                RotateTowards(moveDirection);
            }
            
            void RotateTowards(Vector3 targetDirection)
            {
                float rotation = Rigidbody.rotation.eulerAngles.y;
                float targetRotation = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;
                rotation = Mathf.MoveTowardsAngle(rotation, targetRotation, turnSpeed * Time.deltaTime);

                Rigidbody.rotation = Quaternion.Euler(0, rotation, 0);
            }
        }
    }
}

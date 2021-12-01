using Components.Logger;
using KinematicCharacterController;
using Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Components.Player
{
    public class PlayerCharacterController : DamageReceiver, ICharacterController
    {
#pragma warning disable 649
        [Header("References")] [SerializeField]
        private KinematicCharacterMotor motor;

        [SerializeField] private Transform head;

        [Title("Parameters")] [SerializeField] private float health;
        [SerializeField] private float jumpPower;
        [SerializeField] private float jumpGraceTime;
        [SerializeField] private float maxMovementSpeed;
        [SerializeField] private float groundAcceleration;
        [SerializeField] private Vector2 lookAngles;
        [SerializeField] private float frictionRate;
        #pragma warning restore 649

        private float _health;
        private float _groundSpeed;

        private bool _jumpRequested;
        private float _timeSinceJumpRequested;
        private float _frictionRate;
        private float _verticalVelocity;

        private Vector2 _lookInput;
        private Vector3 _movementInput;

        public float GroundSpeed => _groundSpeed;

        public Transform Head => head;

        public float FrictionRate
        {
            get => frictionRate;
            set => frictionRate = value;
        }

        private void Awake()
        {
            motor.CharacterController = this;
            _health = health;
        }

        public void SetInput(PlayerInput input)
        {
            _lookInput += input.LookInput;
            _movementInput = transform.forward * input.MovementInput.y +
                             transform.right * input.MovementInput.x;

            if (input.JumpPressed)
            {
                _jumpRequested = true;
                _timeSinceJumpRequested = 0;
            }
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            // Scale with deltaTime
            var lookInput = _lookInput * deltaTime;

            // Head up/down rotation
            var headRotation = Quaternion.Euler(lookInput.y, 0, 0) * head.localRotation;
            head.localRotation = ClampRotationAroundXAxis(headRotation);

            // Right/left rotation
            currentRotation = Quaternion.Euler(0, lookInput.x, 0) * currentRotation;

            // Reset look delta
            _lookInput = Vector2.zero;
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            var projectedVel = Vector3.Dot(currentVelocity, _movementInput);
            var currentSpeed = currentVelocity.magnitude;

            if (motor.GroundingStatus.IsStableOnGround)
            {
                _verticalVelocity = 0;

                // friction/drag speed reduction
                if (currentSpeed > 0)
                {
                    SceneManager.CameraFeedback.HeadBob(currentSpeed / maxMovementSpeed);
                    currentVelocity *= Mathf.Max((currentSpeed - maxMovementSpeed * FrictionRate * deltaTime), 0) / currentSpeed;
                }
            }
            else
            {
                _verticalVelocity += Physics.gravity.y;
            }

            var nextGroundVel = currentVelocity
                                + _movementInput * (groundAcceleration + projectedVel) * deltaTime;

            currentVelocity = Vector3.ClampMagnitude(new(nextGroundVel.x, 0, nextGroundVel.z), maxMovementSpeed)
                              + _verticalVelocity * Vector3.up * deltaTime;

            if (_jumpRequested && motor.GroundingStatus.IsStableOnGround && _timeSinceJumpRequested < jumpGraceTime)
            {
                // in order to jump disable ground snapping for next update
                motor.ForceUnground();
                _jumpRequested = false;
                _verticalVelocity = jumpPower;
            }

#if UNITY_EDITOR
            _gizmo_currentVelocity = currentVelocity;
#endif
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
        }

        public void PostGroundingUpdate(float deltaTime)
        {
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
            // Cancel jump if grace period has passed
            _timeSinceJumpRequested += deltaTime;
            if (_timeSinceJumpRequested >= jumpGraceTime)
            {
                _jumpRequested = false;
            }
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport)
        {
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition,
            Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
        }

        public override void DealDamage(int damage)
        {
            var maxDamage = Mathf.Min(damage, _health);

            GameLogger.LogWarning($"PLAYER: Taken {maxDamage} damage.");

            if (_health > 0)
            {
                _health -= maxDamage;

                if (_health <= 0)
                {
                    RunOutOfDurability();
                }
            }
        }

        public override void RunOutOfDurability()
        {
            GameLogger.LogDebug("Destroying player object.");
            EventManager.RaisePlayerDied();
        }

        private Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
            angleX = Mathf.Clamp(angleX, lookAngles.x, lookAngles.y);
            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }

#if UNITY_EDITOR

        private Vector3 _gizmo_currentVelocity;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;

            var position = this.transform.position;

            Gizmos.DrawLine(position, position + _gizmo_currentVelocity);
        }
#endif
    }
}
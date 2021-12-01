using System;
using System.Collections;
using Models;
using UnityEngine;
using UnityEngine.AI;

namespace Components.Enemies
{
    public class TPoser : DamageReceiver, IAiCharacter
    {
        public static event Action Killed;
        
        #pragma warning disable 649
        [SerializeField] private Transform eyeTransform;
        #pragma warning restore 649
        
        private int _damage;
        private int _health;
        private float _movementSpeed;
        private float _turningSpeed;
        private float _attackCooldown;

        private bool _attackInProgress;
        private float _currentAttackCooldown;
        private NavMeshAgent _agent;

        private AIController _controller;

        public Vector3 Forward => transform.forward;
        
        public Vector3 Position => transform.position;
        
        public Vector3 EyePosition => eyeTransform.position;

        public bool CanAttack => _currentAttackCooldown <= 0;
        
        public bool Moving { get; set; }
        
        public bool Attacking { get; set; }
        
        public bool OverrideLookDirection { get; set; }
        
        public Vector3 MoveDestination { get; set; }
        
        public Vector3 DesiredLookDirection { get; set; }

        public void Initialize(AIController controller, Stats stats)
        {
            _agent = GetComponent<NavMeshAgent>();

            _damage = stats.damage;
            _health = stats.health;
            _movementSpeed = stats.movementSpeed;
            _turningSpeed = stats.turningSpeed;
            _attackCooldown = 1f / stats.attackSpeed;

            _agent.speed = _movementSpeed;
            _agent.angularSpeed = _turningSpeed;
            _agent.acceleration = _movementSpeed * 3.5f;

            _controller = controller;
        }

        private void Update()
        {
            if (_health <= 0) return;
            
            HandleAgentControl();
            HandleOverriddenLookDirection();
            HandleAttackCommand();
        }

        public override void DealDamage(int damage)
        {
            if (_health >= 0)
            {
                _health -= damage;

                if (_health <= 0)
                {
                    RunOutOfDurability();
                }
            }
        }

        public override void RunOutOfDurability()
        {
            Debug.Log("Died");
            
            Killed?.Invoke();
            
            _agent.enabled = false;
            _agent.updatePosition = false;
            _agent.updateRotation = false;
            _controller.enabled = false;
            StartCoroutine(DestroyTPoser());


            IEnumerator DestroyTPoser()
            {
                yield return new WaitForSeconds(2);
                Destroy(gameObject);
            }
        }

        private void HandleAgentControl()
        {
            if (_attackInProgress) return;
            
            _agent.isStopped = !Moving;
            _agent.updateRotation = !OverrideLookDirection;

            _agent.SetDestination(MoveDestination);
        }

        private void HandleOverriddenLookDirection()
        {
            if (_attackInProgress) return;
            
            if (OverrideLookDirection)
            {
                var forward = transform.forward;
                forward = Vector3.RotateTowards(
                    forward,
                    DesiredLookDirection,
                    _turningSpeed * Mathf.Deg2Rad * Time.deltaTime,
                    0
                );
                transform.forward = forward;
            }
        }

        private void HandleAttackCommand()
        {
            if (!_attackInProgress && CanAttack && Attacking)
            {
                _attackInProgress = true;
                _currentAttackCooldown = _attackCooldown;
                _controller.Context.enemy.DealDamage(_damage);
            }
            else if (_attackInProgress)
            {
                _currentAttackCooldown -= Time.deltaTime;

                if (_currentAttackCooldown <= 0)
                {
                    _attackInProgress = false;
                }
            }
        }
    }
}
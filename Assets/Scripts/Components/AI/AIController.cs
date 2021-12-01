using System;
using Components.AI.StateMachine;
using Models;
using UnityEngine;

namespace Components.Enemies
{
    public interface IAiCharacter
    {
        void Initialize(AIController controller, Stats stats);
        
        Vector3 Forward { get; }
        Vector3 Position { get; }
        Vector3 EyePosition { get; }
        bool CanAttack { get; }
        bool Moving { get; set; }
        bool Attacking { get; set; }
        bool OverrideLookDirection { get; set; }
        Vector3 MoveDestination { get; set; }
        Vector3 DesiredLookDirection { get; set; }
    }
    
    public class AIController : MonoBehaviour
    {
        #pragma warning disable 649
        [SerializeField] private Stats stats;
        #pragma warning restore 649
        
        private State _currentState;

        private AIContext _aiContext;
        private IAiCharacter _aiCharacter;
        private int _tickIndex;
        private int _ticks;
        
        public AIContext Context => _aiContext;

        public IAiCharacter Character => _aiCharacter;

        public float LosAngle => stats.losAngle;

        public float HearRange => stats.hearRange;

        public float ChaseRange => stats.chaseRange;

        public float AttackRange => stats.attackRange;

        public float Aggressiveness => stats.aggressiveness;

        private void OnDrawGizmos()
        {
            _currentState?.DrawGizmos();
        }

        private void Awake()
        {
            _aiCharacter = GetComponent<IAiCharacter>();
            _aiContext = FindObjectOfType<AIContext>();
            
            _aiCharacter.Initialize(this, stats);
        }

        private void OnEnable()
        {
            SwitchState(new PatrolState(this));
        }

        private void Update()
        {
            Think();
            Act();
        }

        private void OnDisable()
        {
            _currentState?.Exit();
            _currentState = null;
            _aiCharacter.Moving = false;
            _aiCharacter.Attacking = false;
        }

        public void Think()
        {
            _currentState?.Think();
        }

        public void Act()
        {
            _currentState?.Act();
        }

        public void SwitchState(State newState)
        {
            if (newState == _currentState) return;
            
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }
    }
}
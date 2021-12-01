using Components.Enemies;
using UnityEngine;
using Utilities;

namespace Components.AI.StateMachine
{
    public class ChaseSoundState : State
    {
        private bool _waiting;
        private float _waitDuration;
        
        public ChaseSoundState(AIController controller) 
            : base(controller)
        { }

        public override void DrawGizmos()
        {
            _waiting = false;
        }

        public override void Enter()
        {
            Controller.Character.OverrideLookDirection = false;
        }

        public override void Think()
        {
            // If enemy is not in chase target detection range
            if (AIUtility.IsEnemyInChaseRange(Controller))
            {
                // Switch to chase target state
                Controller.SwitchState(new ChaseTargetState(Controller));
            }
        }

        public override void Act()
        {
            Controller.Character.Moving = !_waiting;
            Controller.Character.MoveDestination = Controller.Context.enemy.transform.position;
        }

        public override void Exit()
        {
            _waiting = false;
            _waitDuration = 0;
        }
    }
}
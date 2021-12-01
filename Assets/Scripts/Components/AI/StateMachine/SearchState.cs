using Components.Enemies;
using UnityEngine;
using Utilities;

namespace Components.AI.StateMachine
{
    public class SearchState : State
    {
        private Vector3 _lastKnownPosition;
        private float _waitDuration;
        private bool _waiting;
        
        public SearchState(AIController controller) 
            : base(controller)
        { }

        public override void DrawGizmos()
        {
            #if UNITY_EDITOR
            
            #endif
        }

        public override void Enter()
        {
            _lastKnownPosition = Controller.Context.enemy.transform.position;
            Controller.Character.Moving = true;
            Controller.Character.MoveDestination = _lastKnownPosition;
        }

        public override void Think()
        {
            // Wait till character reaches target position (last known position of the enemy)
            if (Vector3.Distance(_lastKnownPosition, Controller.Character.Position) > 0.5f) return;
            
            // Then wait for a few seconds, looking around
            if (!_waiting)
            {
                _waiting = true;
                _waitDuration = Random.Range(1.5f, 3f);
            }

            if (_waiting)
            {
                Controller.Character.Moving = false;
            }

            if (AIUtility.IsEnemyInChaseSoundRange(Controller))
            {
                Controller.SwitchState(new ChaseSoundState(Controller));
            }
            else
            {
                Controller.SwitchState(new PatrolState(Controller));
            }
        }

        public override void Act()
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}
using Components.Enemies;
using UnityEngine;
using Utilities;

namespace Components.AI.StateMachine
{
    public class PatrolState : State
    {
        private bool _idling;
        private float _idleTimeRemaining;
        
        private bool _haveWaypoint;
        private bool _reachedWaypoint;
        private int _waypoint;

        public PatrolState(AIController controller) 
            : base(controller)
        { }

        public override void DrawGizmos()
        {
            AIUtility.DrawRangeGizmo(Controller.Character.Position, Controller.HearRange, Color.green);
            AIUtility.DrawRangeGizmo(Controller.Character.Position, Controller.ChaseRange, Color.blue);
            AIUtility.DrawRangeGizmo(Controller.Character.Position, Controller.AttackRange, Color.red);

            if (_haveWaypoint)
            {
                var destination = Controller.Context.waypoints[_waypoint].position;
                AIUtility.DrawDestinationGizmo(Controller.Character.Position, destination, Color.green);
            }
        }
        
        public override void Enter()
        {
            _idleTimeRemaining = 0;
            _waypoint = 0;
            
            // choose new waypoint
            _idling = false;
            _haveWaypoint = true;
            _waypoint = Random.Range(0, Controller.Context.waypoints.Length);
            _reachedWaypoint = ReachedWaypoint(Controller.Context.waypoints[_waypoint]);
                        
            // Set character moving
            Controller.Character.Moving = true;

            Controller.Character.OverrideLookDirection = false;
        }
        
        public override void Think()
        {
            // If enemy is in chase range
            if (AIUtility.IsEnemyInChaseRange(Controller))
            {
                if (AIUtility.IsEnemyInSight(Controller))
                {
                    Controller.SwitchState(new ChaseTargetState(Controller));
                    return;
                }
            }
            
            if (AIUtility.IsEnemyInChaseSoundRange(Controller))
            {
                Controller.SwitchState(new ChaseSoundState(Controller));
                return;
            }
                    
                    
            // If idling
            if (_idling)
            {
                // Cancel idling if time runs out
                if (_idleTimeRemaining <= 0)
                {
                    _idling = false;
                    _haveWaypoint = true;
                    _waypoint = Random.Range(0, Controller.Context.waypoints.Length); 
                }
            }
            else
            {
                // If waypoint is reached
                if (_reachedWaypoint)
                {
                    // Roll random number
                    var randomNo = Random.Range(0f, 1f);
                    // If number < aggressiveness
                    if (randomNo > Controller.Aggressiveness)
                    {
                        // Stay idle for Random seconds (1.5f, 4f)
                        _idling = true;
                        _haveWaypoint = false;
                        _idleTimeRemaining = Random.Range(1.5f, 4f);
                        
                        // Set character idle
                        Controller.Character.Moving = false;
                    }
                    else
                    {
                        // choose new waypoint
                        _idling = false;
                        _haveWaypoint = true;
                        _waypoint = Random.Range(0, Controller.Context.waypoints.Length);
                        _reachedWaypoint = ReachedWaypoint(Controller.Context.waypoints[_waypoint]);
                        
                        // Set character moving
                        Controller.Character.Moving = true;
                    }
                }
                else
                {
                    _reachedWaypoint = ReachedWaypoint(Controller.Context.waypoints[_waypoint]);
                }
            }
        }

        public override void Act()
        {
            // If waypoint is set
            if (_haveWaypoint)
            {
                var waypoint = Controller.Context.waypoints[_waypoint];
                if (!ReachedWaypoint(waypoint))
                {
                    // Update destination
                    Controller.Character.MoveDestination = waypoint.position;
                }
            }
            else if (_idling)
            {
                _idleTimeRemaining -= Time.deltaTime;
            }
        }

        public override void Exit()
        {
            
        }

        private bool ReachedWaypoint(Transform waypoint) 
            => Vector3.Distance(waypoint.position, Controller.Character.Position) < 0.5f;
    }
}
using Components.Enemies;
using Utilities;

namespace Components.AI.StateMachine
{
    public class ChaseTargetState : State
    {
        public ChaseTargetState(AIController controller) 
            : base(controller)
        { }

        public override void DrawGizmos()
        {
            
        }
        
        public override void Enter()
        {
            Controller.Character.OverrideLookDirection = false;
        }

        public override void Think()
        {
            // If target is out of chase range or not in los
            if (!AIUtility.IsEnemyInChaseRange(Controller) || !AIUtility.IsEnemyInSight(Controller))
                // Switch to search state
                Controller.SwitchState(new SearchState(Controller));
        }

        public override void Act()
        {
            Controller.Character.Attacking = AIUtility.IsEnemyInAttackRange(Controller);

            if (!Controller.Character.Attacking)
            {
                Controller.Character.Moving = true;
                Controller.Character.MoveDestination = Controller.Context.enemy.transform.position;
            }
            else
            {
                Controller.Character.Moving = false;
            }
        }

        public override void Exit()
        {
            
        }
    }
}
using Components.Enemies;

namespace Components.AI.StateMachine
{
    public abstract class State
    {
        protected AIController Controller { get; private set; }

        protected State(AIController controller)
        {
            Controller = controller;
        }

        public abstract void DrawGizmos();
        public abstract void Enter();
        public abstract void Think();
        public abstract void Act();
        public abstract void Exit();
    }
}
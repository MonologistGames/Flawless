using System.Collections.Generic;

namespace Monologist.Patterns.State
{
    public abstract class StateMachine
    {
        public IState CurrentState { get; private set; }
        protected Dictionary<string, IState> StatePool;

        public StateMachine()
        {
            StatePool = new Dictionary<string, IState>();
        }

        public virtual void Initialize()
        { }
        
        public virtual void Enable()
        { }
        public virtual void Disable()
        { }
        
        public void TransitTo(IState nextState)
        {
            CurrentState?.OnExit();
            CurrentState = nextState;
            CurrentState.OnEnter();
        }
        
        public void TransitTo(string nextStateName)
        {
            TransitTo(StatePool[nextStateName]);
        }

        public void Update()
        {
            CurrentState?.Update();
        }

        public void FixedUpdate()
        {
            CurrentState?.FixedUpdate();
        }
        
#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            CurrentState?.OnDrawGizmos();
        }
#endif
    }
}

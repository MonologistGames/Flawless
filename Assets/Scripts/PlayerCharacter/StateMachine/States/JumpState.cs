using Monologist.Patterns.State;

namespace Flawless.PlayerCharacter
{
    public class JumpState : IState
    {
        private PlayerStateMachine _stateMachine;
        private PlanetController _planetController;
        
        public JumpState(PlayerStateMachine playerStateMachine)
        {
            _stateMachine = playerStateMachine;
            _planetController = playerStateMachine.PlanetController;
        }
        
        public void OnEnter()
        {
            
        }

        public void Update()
        {
            throw new System.NotImplementedException();
        }

        public void FixedUpdate()
        {
            throw new System.NotImplementedException();
        }

        public void OnExit()
        {
            throw new System.NotImplementedException();
        }

        public void OnDrawGizmos()
        {
            throw new System.NotImplementedException();
        }

        #region Bind Input Actions

        private void BindInputActions()
        {
            
        }

        #endregion
    }
}
using Monologist.Patterns.State;
using UnityEngine;

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
        }

        public void FixedUpdate()
        {
        }

        public void OnExit()
        {
        }

        public void OnDrawGizmos()
        {
        }

        #region Bind Input Actions

        private void BindInputActions()
        {
        }

        #endregion
    }
}
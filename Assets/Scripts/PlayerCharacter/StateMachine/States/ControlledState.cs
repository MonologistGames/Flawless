using Monologist.Patterns.State;
using UnityEngine;

namespace Flawless.PlayerCharacter
{
    public class ControlledState : IState
    {
        private PlayerStateMachine _stateMachine;
        private PlayerController _playerController;

        public ControlledState(PlayerStateMachine playerStateMachine)
        {
            _stateMachine = playerStateMachine;
            _playerController = playerStateMachine.PlayerController;
        }

        public void OnEnter()
        {
            _playerController.Rigidbody.velocity = Vector3.zero;
            _playerController.Life.enabled = false;
        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
            _playerController.Rigidbody.velocity = Vector3.zero;
        }

        public void OnExit()
        {
            _playerController.Life.enabled = true;
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
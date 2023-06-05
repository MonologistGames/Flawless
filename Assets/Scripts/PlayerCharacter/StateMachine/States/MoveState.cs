using Monologist.Patterns.State;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Flawless.PlayerCharacter
{
    public class MoveState : IState
    {
        private readonly PlayerStateMachine _stateMachine;
        private readonly PlayerController _playerController;
        
        private bool _isAccelerating;

        public MoveState(PlayerStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _playerController = stateMachine.PlayerController;
        }

        public void OnEnter()
        {
            AddInputCallbacks();
        }

        public void Update()
        { }

        public void FixedUpdate()
        {
            var accelerateVector = Vector3.zero;
            // Apply motivation and Gravitation
            if (_isAccelerating)
                accelerateVector = _playerController.MoveDir * _playerController.Acceleration;

            _playerController.Rigidbody.AddForce(accelerateVector, ForceMode.Acceleration);

            _playerController.Rigidbody.velocity = _playerController.Velocity.normalized * 
                                                   Mathf.Min(_playerController.IsOverDriving ?_playerController.MaxOverDriveSpeed : _playerController.MaxSpeed, 
                                                       _playerController.Velocity.magnitude);
        }

        public void OnExit()
        {
            RemoveInputCallbacks();
        }

        public void OnDrawGizmos()
        { }

        #region Input Bindings

        private void AddInputCallbacks()
        {
            _playerController.SpeedUpButton.started += OnSpeedUpStart;
            _playerController.SpeedUpButton.canceled += OnSpeedUpCancel;

            _playerController.LeapButton.performed += OnLeap;
        }

        private void RemoveInputCallbacks()
        {
            _playerController.SpeedUpButton.started -= OnSpeedUpStart;
            _playerController.SpeedUpButton.canceled -= OnSpeedUpCancel;

            _playerController.LeapButton.performed -= OnLeap;
        }

        #endregion

        #region Input Callbacks

        /// <summary>
        /// Start accelerating when speed up button is pressed.
        /// </summary>
        private void OnSpeedUpStart(InputAction.CallbackContext context)
        {
            _isAccelerating = true;
        }

        /// <summary>
        /// Stop accelerating when speed up button is released.
        /// </summary>
        private void OnSpeedUpCancel(InputAction.CallbackContext context)
        {
            _isAccelerating = false;
        }

        private void OnLeap(InputAction.CallbackContext context)
        {
            if (!_playerController.IsLeapReady) return;
            
            _playerController.LeapTimer.ResetTime();
            _playerController.LeapTimer.IsPaused = false;
            _playerController.IsLeapReady = false;
            
            _playerController.OverDriveTimer.AddTime(_playerController.OverDriveDuration);
            _playerController.OverDriveTimer.IsPaused = false;
            _playerController.IsOverDriving = true;
            
            _playerController.Rigidbody.AddForce(_playerController.LeapAcceleration * _playerController.MoveDir,
                ForceMode.Impulse);
        }

        #endregion
    }
}
using Monologist.Patterns.State;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Flawless.PlayerCharacter
{
    public class MoveState : IState
    {
        private readonly PlayerStateMachine _stateMachine;
        private readonly PlanetController _planetController;
        
        private bool _isAccelerating;

        public MoveState(PlayerStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _planetController = stateMachine.PlanetController;
        }

        public void OnEnter()
        {
            AddInputCallbacks();
        }

        public void Update()
        { }

        public void FixedUpdate()
        {
            var accelerateVector = _planetController.Gravitation;
            // Apply motivation and Gravitation
            if (_isAccelerating)
                accelerateVector += _planetController.MoveDir * _planetController.Acceleration;

            _planetController.Rigidbody.AddForce(accelerateVector, ForceMode.Acceleration);

            _planetController.Rigidbody.velocity = _planetController.Velocity.normalized * 
                                                   Mathf.Min(_planetController.IsOverDriving ?_planetController.MaxDashSpeed : _planetController.MaxSpeed, 
                                                       _planetController.Velocity.magnitude);
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
            _planetController.SpeedUpButton.started += OnSpeedUpStart;
            _planetController.SpeedUpButton.canceled += OnSpeedUpCancel;

            _planetController.LeapButton.performed += OnLeap;
        }

        private void RemoveInputCallbacks()
        {
            _planetController.SpeedUpButton.started -= OnSpeedUpStart;
            _planetController.SpeedUpButton.canceled -= OnSpeedUpCancel;

            _planetController.LeapButton.performed -= OnLeap;
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
            if (!_planetController.IsLeapReady) return;
            
            _planetController.LeapTimer = _planetController.LeapDuration;
            _planetController.SetOverDrive(_planetController.OverDriveDuration + _planetController.OverDriveTimer);
            _planetController.Rigidbody.AddForce(_planetController.LeapAcceleration * _planetController.MoveDir,
                ForceMode.Impulse);
        }

        #endregion
    }
}
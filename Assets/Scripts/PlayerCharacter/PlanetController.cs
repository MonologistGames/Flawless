using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Flawless.PlayerCharacter
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlanetController : MonoBehaviour
    {
        /// <summary>
        /// Rigidbody of the character planet.
        /// </summary>
        private Rigidbody _rigidbody;
        
        /// <summary>
        /// Gravitation that the player planet get.
        /// </summary>
        private Vector3 _gravitation;
        
        private Vector3 _moveDir;
        
        /// <summary>
        /// Desired move direction of the player planet.
        /// </summary>
        public Vector3 MoveDir => _moveDir;

        private bool _isAccelerating;
        private float _motivation;
        
        /// <summary>
        /// Acceleration of the player planet.
        /// </summary>
        public float Acceleration = 2f;
        
        /// <summary>
        /// Max Motivation of the character.
        /// </summary>
        [FormerlySerializedAs("MaxMotivation")] 
        public float MaxSpeed = 5f;

        /// <summary>
        /// Gravitation the player planet get.
        /// </summary>
        public Vector3 Gravitation
        {
            get => _gravitation;
            set => _gravitation = value;
        }

        /// <summary>
        /// Current Velocity of the player planet.
        /// </summary>
        public Vector3 Velocity => _rigidbody.velocity;

        private PlayerInput _playerInput;

        /// <summary>
        /// Target camera that film the character and move along with.
        /// </summary>
        public Camera TargetCamera;

        #region Input Actions

        private InputAction _moveStick;
        private InputAction _speedUpButton;

        #endregion

        #region MonoBehaviours

        void OnEnable()
        {
            _playerInput = GetComponent<PlayerInput>();
            _rigidbody = GetComponent<Rigidbody>();

            // Bind input actions
            _moveStick = _playerInput.actions["MoveDirection"];
            _speedUpButton = _playerInput.actions["SpeedUp"];

            if (_moveStick != null)
            {
                _moveStick.performed += OnMoveInput;
            }

            if (_speedUpButton != null)
            {
                _speedUpButton.started += OnSpeedUpStart;
                _speedUpButton.canceled += OnSpeedUpCancel;
            }
        }

        private void FixedUpdate()
        {
            // Apply motivation and Gravitation
            if (_isAccelerating)
                _rigidbody.AddForce(
                    Acceleration * _moveDir + _gravitation,
                    ForceMode.Acceleration);

            _rigidbody.velocity = Velocity.normalized * Mathf.Min(MaxSpeed, Velocity.magnitude);
        }

        #endregion

        /// <summary>
        /// Get move direction input.
        /// </summary>
        /// <param name="context">Input action callbacks. (Where to read value from)</param>
        private void OnMoveInput(InputAction.CallbackContext context)
        {
            Vector2 inputDir = context.ReadValue<Vector2>();
            _moveDir = new Vector3(inputDir.x, 0, inputDir.y);

            // If using mouse, get relative vector
            if (_playerInput.currentControlScheme == "Keyboard&Mouse")
            {
                Vector3 screenPos = TargetCamera.WorldToScreenPoint(this.transform.position);
                _moveDir.x -= screenPos.x;
                _moveDir.z -= screenPos.y;
            }

            _moveDir = _moveDir.normalized;
        }
        
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
    }
}
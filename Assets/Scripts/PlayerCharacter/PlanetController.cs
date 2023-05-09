using System;
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
        public Rigidbody Rigidbody { get; private set; }

        public PlayerInput PlayerInput { get; private set; }

        /// <summary>
        /// Target camera that film the character and move along with.
        /// </summary>
        public Camera TargetCamera;

        public PlayerStateMachine StateMachine { get; private set; }

        #region Input Actions

        public InputAction MoveStick { get; set; }
        public InputAction SpeedUpButton { get; private set; }
        public InputAction LeapButton { get; private set; }

        #endregion

        #region Movement

        /// <summary>
        /// Desired move direction of the player planet.
        /// </summary>
        public Vector3 MoveDir { get; private set; }
        
        /// <summary>
        /// Acceleration of the player planet.
        /// </summary>
        [Header("Movement")] public float Acceleration = 2f;

        /// <summary>
        /// Max Motivation of the character.
        /// </summary>
        public float MaxSpeed = 3f;

        public float LeapAcceleration = 10f;

        public float MaxDashSpeed = 5f;

        /// <summary>
        /// Gravitation the player planet get.
        /// </summary>
        public Vector3 Gravitation { get; set; }

        /// <summary>
        /// Current Velocity of the player planet.
        /// </summary>
        public Vector3 Velocity => Rigidbody.velocity;

        #endregion

        #region MonoBehaviours
        /// <summary>
        /// Do the initialization work, including:
        /// - Get components: Rigidbody, PlayerInput
        /// - Bind input actions
        /// - Initialize state machine
        /// </summary>
        void OnEnable()
        {
            PlayerInput = GetComponent<PlayerInput>();
            Rigidbody = GetComponent<Rigidbody>();

            // Bind input actions
            MoveStick = PlayerInput.actions["MoveDirection"];
            SpeedUpButton = PlayerInput.actions["SpeedUp"];
            LeapButton = PlayerInput.actions["Dash"];
            
            // Add input callbacks
            MoveStick.performed += OnMoveInput;
            
            // Initialize state machine
            StateMachine = new PlayerStateMachine(this);
            StateMachine.Initialize();
            
            // Start state machine
            StateMachine.TransitTo("MoveState");
        }

        private void Update()
        {
            StateMachine.Update();
        }

        private void FixedUpdate()
        {
            StateMachine.FixedUpdate();
        }

        #endregion

        #region Input Handling

        /// <summary>
        /// Get move direction input.
        /// </summary>
        /// <param name="context">Input action callbacks. (Where to read value from)</param>
        private void OnMoveInput(InputAction.CallbackContext context)
        {
            Vector2 inputDir = context.ReadValue<Vector2>();
            MoveDir = new Vector3(inputDir.x, 0, inputDir.y);

            // If using mouse, get relative vector
            if (PlayerInput.currentControlScheme == "Keyboard&Mouse")
            {
                Vector3 screenPos = TargetCamera.WorldToScreenPoint(transform.position);
                MoveDir -= new Vector3(screenPos.x,0,0);
                MoveDir -= new Vector3(0,0,screenPos.y);
            }

            MoveDir = MoveDir.normalized;
        }

        #endregion
    }
}
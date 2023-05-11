using Flawless.LifeSys;
using UnityEngine;
using UnityEngine.InputSystem;

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

        private PlayerStateMachine StateMachine { get; set; }
        private PlayerLifeAmount LifeAmount { get; set; }

        #region Input Actions

        private InputAction MoveStick { get; set; }
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

        [Header("Leap")] public float LeapAcceleration = 10f;

        public float MaxDashSpeed = 5f;

        #region Leap

        public float LeapDuration = 5f;
        public float LeapTimer { get; set; }

        /// <summary>
        /// Whether player is ready to leap.
        /// </summary>
        public bool IsLeapReady => LeapTimer <= 0;

        #endregion

        #region OverDrive

        public float OverDriveDuration = 0.5f;
        public float OverDriveTimer { get; set; }
        public bool IsOverDriving => OverDriveTimer > 0;

        #endregion


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
        void Start()
        {
            PlayerInput = GetComponent<PlayerInput>();
            Rigidbody = GetComponent<Rigidbody>();

            LifeAmount = GetComponentInChildren<PlayerLifeAmount>();

            // Bind input actions
            MoveStick = PlayerInput.actions["MoveDirection"];
            SpeedUpButton = PlayerInput.actions["SpeedUp"];
            LeapButton = PlayerInput.actions["Leap"];

            // Add input callbacks
            MoveStick.performed += OnMoveInput;

            // Initialize state machine
            StateMachine = new PlayerStateMachine(this);
            StateMachine.Initialize();

            // Start state machine
            StateMachine.TransitTo("Move");
        }

        private void Update()
        {
            StateMachine.Update();

            // Update leap timer
            if (LeapTimer >= 0)
                LeapTimer -= Time.deltaTime;

            if (OverDriveTimer >= 0)
                OverDriveTimer -= Time.deltaTime;
        }

        private void FixedUpdate()
        {
            StateMachine.FixedUpdate();
        }

        #region Collision Events

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Planet")) return;

            LifeAmount.CollideAndDamageLife(other, Rigidbody);
        }

        #endregion

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
                MoveDir -= new Vector3(screenPos.x, 0, 0);
                MoveDir -= new Vector3(0, 0, screenPos.y);
            }

            MoveDir = MoveDir.normalized;
        }

        #endregion

        #region APIs

        public void SetOverDrive(float overDriveTime)
        {
            OverDriveTimer = overDriveTime;
        }
        public void Jump()
        {
            Rigidbody.velocity = Vector3.zero;
            StateMachine.TransitTo("Jump");
        }

        public void EndJump()
        {
            StateMachine.TransitTo("Move");
        }

        #endregion
    }
}
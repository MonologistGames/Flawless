using Flawless.LifeSys;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Utilities;

namespace Flawless.PlayerCharacter
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        #region Internal Components Refs
        /// <summary>
        /// Rigidbody of the character planet.
        /// </summary>
        public Rigidbody Rigidbody { get; private set; }
        
        /// <summary>
        /// Player Input component.
        /// </summary>
        private PlayerInput PlayerInput { get; set; }

        /// <summary>
        /// Target camera that film the character and move along with.
        /// </summary>
        public Camera TargetCamera;
        
        /// <summary>
        /// State machine to control player movement state.
        /// </summary>
        private PlayerStateMachine StateMachine { get; set; }
        /// <summary>
        /// Life system component of player character.
        /// </summary>
        private PlayerLife Life { get; set; }
        
        #endregion

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
        /// Bonus acceleration for obtaining much life amount.
        /// </summary>
        public float BonusAcceleration = 0.6f;
        
        /// <summary>
        /// Max Motivation of the character.
        /// </summary>
        public float MaxSpeed = 3f;
        
        /// <summary>
        /// Bonus max speed for obtaining much life amount.
        /// </summary>
        public float BonusMaxSpeed = 1f;
        
        [FormerlySerializedAs("MaxHyperSpeed")]
        [Header("OverDrive Mode")]
        [FormerlySerializedAs("MaxDashSpeed")] public float MaxOverDriveSpeed = 5f;
        public bool IsOverDriving;
        #region Leap
        
        [Header("Leap")] public float LeapAcceleration = 10f;

        public float LeapDuration = 5f;
        /// <summary>
        /// Timer instance for leap count down.
        /// </summary>
        [HideInInspector]
        public Timer LeapTimer;

        /// <summary>
        /// Whether player is ready to leap.
        /// </summary>
        public bool IsLeapReady;

        #endregion

        #region OverDrive

        public float OverDriveDuration = 0.5f;
        [HideInInspector]
        public Timer OverDriveTimer;

        #endregion

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

            Life = GetComponentInChildren<PlayerLife>();

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
            
            
            // Initialize Timers
            LeapTimer = TimerManager.Instance.AddTimer( LeapDuration,"LeapTimer");
            LeapTimer.SetTime(0);
            IsLeapReady = true;
            
            LeapTimer.OnTimerEnd += () => IsLeapReady = true;
            OverDriveTimer = TimerManager.Instance.AddTimer("OverDriveTimer");
            OverDriveTimer.OnTimerEnd += () => IsOverDriving = false;
        }
        
        private void Update()
        {
            StateMachine.Update();
        }

        private void FixedUpdate()
        {
            StateMachine.FixedUpdate();
        }

        #region Collision Events
        
        /// <summary>
        /// Automatically called when player planet collide with other objects.
        /// Will call planet's CollideAndDamageLife method to react differently from planet to planet.
        /// </summary>
        /// <param name="other"></param>
        private void OnCollisionEnter(Collision other)
        {
            var planet = other.gameObject.GetComponent<PlanetLife>();
            if (planet == null) return;
            
            var normalDir = other.GetContact(0).normal;
            planet.CollideAndDamageLife(Rigidbody, Life, normalDir);
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
        public void Jump()
        {
            StateMachine.TransitTo("Controlled");
        }

        public void EndJump()
        {
            StateMachine.TransitTo("Move");
        }
        
        /// <summary>
        /// Set the player planet to controlled state.
        /// Will not be affected by gravity and player input.
        /// </summary>
        public void SetControlled()
        {
            StateMachine.TransitTo("Controlled");
        }
        
        /// <summary>
        /// Recover from controlled state.
        /// </summary>
        public void SetPlaying()
        {
            StateMachine.TransitTo("Move");
        }

        #endregion
    }
}
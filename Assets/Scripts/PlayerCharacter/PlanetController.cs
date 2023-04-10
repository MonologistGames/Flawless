using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlanetController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vector3 _gravitation;
    private Vector3 _moveDir;

    private bool isAccelerating;
    private float _motivation;
    private float motivation
    {
        get => _motivation;
        set
        {
            if (value >= maxMotivation)
            {
                _motivation = maxMotivation;
                return;
            }

            if (value <= 0)
            {
                _motivation = 0;
                return;
            }

            _motivation = value;
        }
    }
    public float accleration = 2f;
    public float maxMotivation = 5f;

    /// <summary>
    /// Gravitation the player planet get.
    /// </summary>
    public Vector3 gravitation
    {
        get => _gravitation; 
        set => _gravitation = value;
    }
    
    /// <summary>
    /// Current velocity of the player planet.
    /// </summary>
    public Vector3 velocity => _rigidbody.velocity;

    private PlayerInput _playerInput;
    
    /// <summary>
    /// Target camera that film the character and move along with.
    /// </summary>
    public Camera targetCamera;
    /// <summary>
    /// Direction pointer for moving Direction.
    /// </summary>
    public Transform pointer;

    #region Input Actions

    private InputAction _moveStick;
    private InputAction _speedUpButton;

    #endregion

    #region MonoBehaviours

    // Start is called before the first frame update
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
            _moveStick.canceled += OnMoveInput;
        }
        if (_speedUpButton != null)
        {
            _speedUpButton.started += OnSpeedUpStart;
            _speedUpButton.canceled += OnSpeedUpCancel;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Set acceleration vector magnitude
        if (isAccelerating)
        {
            motivation += accleration * Time.deltaTime;
        }
        else
        {
            motivation =0f;
        }
    }

    private void FixedUpdate()
    {
        // Apply motivation and gravitation
        _rigidbody.AddForce(_motivation * _moveDir + _gravitation, ForceMode.Acceleration);
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
            Vector3 screenPos = targetCamera.WorldToScreenPoint(this.transform.position);
            _moveDir.x -= screenPos.x;
            _moveDir.z -= screenPos.y;
        }

        _moveDir = _moveDir.normalized;
    }

    private void OnSpeedUpStart(InputAction.CallbackContext context)
    {
        isAccelerating = true;
    }

    private void OnSpeedUpCancel(InputAction.CallbackContext context)
    {
        isAccelerating = false;
    }
}
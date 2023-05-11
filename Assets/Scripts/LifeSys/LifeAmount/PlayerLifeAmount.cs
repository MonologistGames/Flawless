using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Flawless.LifeSys
{
    [RequireComponent(typeof(SphereCollider))]
    public class PlayerLifeAmount : LifeAmount
    {
        public float MaxAmount = 100000f;
        
        [Header("Life Decrease Speed")]
        public float BasePlantDecreaseSpeed = 10f;
        public float BaseAnimalDecreaseSpeed = 10f;

        [Header("Absorb")]
        public float AbsorbSpeed = 100f;
        public float AbsorbRange = 2f;

        [Header("Collide")] public float CollideForce = 10f;
        public float CollideDamage = 100f;

        private PlayerInput _playerInput;
        private InputAction _absorbButton;

        private PlanetLifeAmount _otherPlanetLifeAmount;
        private float _otherPlanetLifeAmountRatio;
        private bool _isAbsorbing;
        private bool _isAbsorbBegun;
        
        private CinemachineImpulseSource _impulseSource;

        #region Editor

#if UNITY_EDITOR
        private void OnValidate()
        {
            var sphereCollider = GetComponent<SphereCollider>();
            if (sphereCollider)
            {
                sphereCollider.isTrigger = true;
                sphereCollider.radius = AbsorbRange;
            }
        }
#endif

        #endregion

        #region MonoBehaviours

        private void Start()
        {
            _playerInput = GetComponentInParent<PlayerInput>();
            _impulseSource = GetComponent<CinemachineImpulseSource>();

            // Input Actions and bind callbacks
            _absorbButton = _playerInput.actions["Absorb"];
            _absorbButton.started += OnAbsorbStart;
            _absorbButton.canceled += OnAbsorbCancel;
        }

        private void Update()
        {
            // Life amount fade with time
            if (!_isAbsorbing || !_otherPlanetLifeAmount)
            {
                PlantAmount -= BasePlantDecreaseSpeed * Time.deltaTime;
                AnimalAmount -= BaseAnimalDecreaseSpeed * Time.deltaTime;
            }

            //Debug.Log("is absorbing: " + _isAbsorbing + " " + _otherPlanetLifeAmount.gameObject.name);
            // Absorb other planets
            if (_isAbsorbing)
            {
                if (!Absorb(Time.deltaTime))
                    EndAbsorb();
                else
                {
                    _otherPlanetLifeAmount.IsAbsorbed = true;
                }
            }
        }

        #endregion
        
        #region Trigger Events

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Planet") || other.isTrigger) return;

            _otherPlanetLifeAmount = other.GetComponent<PlanetLifeAmount>();
            if (_otherPlanetLifeAmount.IsAbsorbed)
            {
                _otherPlanetLifeAmount = null;
                return;
            }

            _otherPlanetLifeAmountRatio = _otherPlanetLifeAmount.AnimalPlanetRatio /
                                          (1 + _otherPlanetLifeAmount.AnimalPlanetRatio);
            Debug.Log("Ready to absorb " + _otherPlanetLifeAmount.transform.parent.name);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Planet")) return;

            if (_otherPlanetLifeAmount == other.GetComponent<PlanetLifeAmount>())
            {
                EndAbsorb();
                _otherPlanetLifeAmount = null;
            }
        }

        #endregion

        #region Input Callbacks

        #region Absorb

        private void OnAbsorbStart(InputAction.CallbackContext context)
        {
            _isAbsorbing = true;
        }

        private void OnAbsorbCancel(InputAction.CallbackContext context)
        {
            _isAbsorbing = false;
        }

        #endregion

        #endregion

        #region Internal Methods

        /// <summary>
        /// End the absorb process, and set the absorbing planet to absorbed.
        /// </summary>
        private void EndAbsorb()
        {
            if (!_isAbsorbBegun) return;
            _isAbsorbBegun = false;
            if (!_otherPlanetLifeAmount) return;
        
            _otherPlanetLifeAmount.AnimalAmount = 0;
            _otherPlanetLifeAmount.PlantAmount = 0;
            
            // Planet die effects
            _otherPlanetLifeAmount.SetPlanetDead();
        }

        /// <summary>
        /// Absorbing life on other planets.
        /// </summary>
        /// <param name="deltaTime">Delta time between two frames.</param>
        /// <returns>Whether absorbing is successful.</returns>
        private bool Absorb(float deltaTime)
        {
            if (!_otherPlanetLifeAmount)
            {
                return false;
            }

            if (_otherPlanetLifeAmount.PlantAmount + _otherPlanetLifeAmount.AnimalAmount
                < AbsorbSpeed * deltaTime)
            {
                return false;
            }
            
            _isAbsorbBegun = true;

            var animalAbsorbAmount =
                AbsorbSpeed * _otherPlanetLifeAmountRatio * deltaTime;
            var plantAbsorbAmount = deltaTime * AbsorbSpeed - animalAbsorbAmount;

            _otherPlanetLifeAmount.AnimalAmount -= animalAbsorbAmount;
            _otherPlanetLifeAmount.PlantAmount -= plantAbsorbAmount;

            this.AnimalAmount += animalAbsorbAmount;
            this.PlantAmount += plantAbsorbAmount;

            return true;
        }

        #endregion

        #region APIs

        public void CollideAndDamageLife(Collision other, Rigidbody player)
        {
            // TODO: To make more detailed damage calculation and effects.
            Vector3 velocityDir = player.velocity.normalized;
            Vector3 normalDir = other.GetContact(0).normal;
            Vector3 boundDirection = velocityDir - 2 * Vector3.Dot(velocityDir, normalDir) * normalDir;
            player.AddForce(boundDirection * CollideForce,
                ForceMode.VelocityChange);
            
            PlantAmount -= CollideDamage;
            AnimalAmount -= CollideDamage;
            
            // Camera shake
            _impulseSource.GenerateImpulse();
        }

        #endregion
    }
}
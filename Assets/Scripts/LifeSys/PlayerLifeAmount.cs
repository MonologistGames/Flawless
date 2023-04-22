using UnityEngine;
using UnityEngine.InputSystem;

namespace Flawless.LifeSys
{
    [RequireComponent(typeof(SphereCollider))]
    public class PlayerLifeAmount : LifeAmount
    {
        [Header("Life Decrease Speed")] public float BasePlantDecreaseSpeed = 10f;
        public float BaseAnimalDecreaseSpeed = 10f;

        [Header("Absorb")] public float AbsorbSpeed = 100f;
        public float AbsorbRange = 2f;

        private PlayerInput _playerInput;
        private InputAction _absorbButton;

        private PlanetLifeAmount _otherPlanetLifeAmount;
        private float _otherPlanetLifeAmountRatio;
        private bool _isAbsorbing;

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

        private void OnEnable()
        {
            _playerInput = GetComponentInParent<PlayerInput>();

            // Input Actions and bind callbacks
            _absorbButton = _playerInput.actions["Absorb"];
            _absorbButton.started += OnAbsorbStart;
            _absorbButton.canceled += OnAbsorbCancel;
        }

        private void Update()
        {
            // Life amount fade with time
            PlantAmount -= BasePlantDecreaseSpeed * Time.deltaTime;
            AnimalAmount -= BaseAnimalDecreaseSpeed * Time.deltaTime;

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

        #region Collision Events

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Planet")) return;
            Debug.Log("Player Hit Planet");
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

        #region Life Amount Methods

        /// <summary>
        /// End the absorb process, and set the absorbing planet to absorbed.
        /// </summary>
        private void EndAbsorb()
        {
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
    }
}
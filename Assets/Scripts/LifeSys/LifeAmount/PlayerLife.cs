using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Flawless.LifeSys
{
    [RequireComponent(typeof(SphereCollider))]
    public class PlayerLife : MonoBehaviour
    {
        #region Life Amount

        // Life Units Settings
         public static readonly float LifeUnit = 1000f;

        [Header("Life Units")] public int MaxLifeUnits = 6;

        [FormerlySerializedAs("_lifeUnitsCountCount")] [SerializeField]
        private int _lifeUnitsCount = 2;

        public int LifeUnitsCount
        {
            get => _lifeUnitsCount;
            set
            {
                if (value < 0)
                {
                    _lifeUnitsCount = 0;
                    return;
                }

                if (value > MaxLifeUnits)
                {
                    _lifeUnitsCount = MaxLifeUnits;
                    return;
                }

                _lifeUnitsCount = value;
            }
        }

        /// <summary>
        /// Player‘s max life amount
        /// </summary>
        public float MaxLifeAmount => LifeUnitsCount * LifeUnit;

        // Life amount property
        [Header("Life Amount")] [SerializeField]
        private float _lifeAmount;

        public float LifeAmount
        {
            get => _lifeAmount;
            set
            {
                if (value < 0)
                {
                    _lifeAmount = 0;
                }
                else
                {
                    _lifeAmount = value > MaxLifeAmount ? MaxLifeAmount : value;
                }

                OnLifeAmountChanged?.Invoke(LifeAmount, LifeUnit, LifeUnitsCount);
            }
        }

        public event Action<float, float, int> OnLifeAmountChanged;
        public event Action OnAbsorbed; 

        public float BaseDecreaseSpeed = 10f;

        #endregion

        #region Absorb

        [Header("Absorb")] public float AbsorbSpeed = 100f;
        public float AbsorbRange = 2f;

        #endregion

        #region Internal Components

        private PlayerInput _playerInput;
        private InputAction _absorbButton;

        private readonly List<PlanetLife> _otherPlanetLifeAmount = new List<PlanetLife>();
        private bool _isAbsorbing;
        private bool _isAbsorbBegun;

        public event Action<bool, PlanetLife> OnAbsorbStateChanged;

        private CinemachineImpulseSource _impulseSource;

        #endregion

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
            if (!_isAbsorbing || _otherPlanetLifeAmount.Count != 0)
            {
                LifeAmount -= Time.deltaTime * BaseDecreaseSpeed;
            }

            // Absorb other planets
            if (_isAbsorbing)
            {
                foreach (var planetLifeAmount in _otherPlanetLifeAmount)
                {
                    if (!Absorb(Time.deltaTime, planetLifeAmount))
                        EndAbsorb(planetLifeAmount);
                }
            }
        }

        #endregion

        #region Trigger Events

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Planet") || other.isTrigger) return;

            var planetLifeAmount = other.GetComponent<PlanetLife>();
            if (planetLifeAmount != null && !planetLifeAmount.IsAbsorbed)
            {
                _otherPlanetLifeAmount.Add(planetLifeAmount);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Planet")) return;

            var planetLifeAmount = other.GetComponent<PlanetLife>();
            if (planetLifeAmount != null && !planetLifeAmount.IsAbsorbed)
            {
                _otherPlanetLifeAmount.Remove(planetLifeAmount);
            }

            EndAbsorb(planetLifeAmount);
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
            foreach (var planetLifeAmount in _otherPlanetLifeAmount)
            {
                OnAbsorbStateChanged?.Invoke(false, planetLifeAmount);
            }
        }

        #endregion

        #endregion

        #region Internal Methods

        /// <summary>
        /// End the absorb process, and set the absorbing planet to absorbed.
        /// </summary>
        private void EndAbsorb(PlanetLife planetLife)
        {
            if (!planetLife) return;

            OnAbsorbStateChanged?.Invoke(false, planetLife);
            if (!planetLife.IsAbsorbed) return;
            OnAbsorbed?.Invoke();

            planetLife.LifeAmount = 0;
            planetLife.SetPlanetDead();
        }

        /// <summary>
        /// Absorbing life on other planets.
        /// </summary>
        /// <param name="deltaTime">Delta time between two frames.</param>
        /// <param name="planetLife">Planet life amount to absorb.</param>
        /// <returns>Whether absorbing is successful.</returns>
        private bool Absorb(float deltaTime, PlanetLife planetLife)
        {
            if (planetLife.LifeAmount < AbsorbSpeed * deltaTime)
            {
                return false;
            }

            if (Math.Abs(LifeAmount - MaxLifeAmount) < AbsorbSpeed * deltaTime)
            {
                LifeAmount = MaxLifeAmount;
                return false;
            }

            planetLife.IsAbsorbed = true;
            var absorbAmount = deltaTime * AbsorbSpeed;
            planetLife.LifeAmount -= AbsorbSpeed * deltaTime;

            this.LifeAmount += absorbAmount;

            OnAbsorbStateChanged?.Invoke(true, planetLife);

            return true;
        }

        #endregion

        #region APIs

        public void CollideAndDamageLife(float damage)
        {
            // TODO: To make more detailed damage calculation and effects.
            _lifeAmount -= damage;
            // Camera shake
            _impulseSource.GenerateImpulse();
        }

        public void DecreaseLifeByUnit(int units)
        {
            LifeAmount -= LifeUnit * units;
        }

        #endregion
    }
}
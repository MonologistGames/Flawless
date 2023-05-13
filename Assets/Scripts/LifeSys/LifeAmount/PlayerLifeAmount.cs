using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

namespace Flawless.LifeSys
{
    [RequireComponent(typeof(SphereCollider))]
    public class PlayerLifeAmount : MonoBehaviour
    {
        [Header("Life Units")] 
        public int MaxLifeUnits = 6;
        [SerializeField]
        private int _lifeUnitsCountCount = 2;

        public int LifeUnitsCount
        {
            get => _lifeUnitsCountCount;
            set
            {
                if (value < 0)
                {
                    _lifeUnitsCountCount = 0;
                    return;
                }

                if (value > MaxLifeUnits)
                {
                    _lifeUnitsCountCount = MaxLifeUnits;
                    return;
                }

                _lifeUnitsCountCount = value;
            }
        }

        public static float LifeUnit = 1000f;
        public float MaxLifeAmount => LifeUnitsCount * LifeUnit;

        public event Action<float, float, int> OnLifeAmountChanged;
        
        [Header("Life Amount")]
        [SerializeField]
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
                    if (value > MaxLifeAmount)
                    {
                        _lifeAmount = MaxLifeAmount;
                        return;
                    }
                    else
                        _lifeAmount = value;
                }
                OnLifeAmountChanged?.Invoke(LifeAmount, LifeUnit, LifeUnitsCount);
            }
        }

        public float BaseDecreaseSpeed = 10f;

        [Header("Absorb")] public float AbsorbSpeed = 100f;
        public float AbsorbRange = 2f;

        [Header("Collide")] 
        public float CollideDamage = 100f;

        private PlayerInput _playerInput;
        private InputAction _absorbButton;

        private PlanetLifeAmount _otherPlanetLifeAmount;
        public VisualEffect AbsorbEffect;
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
            
            AbsorbEffect.Stop();
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
                LifeAmount -= Time.deltaTime * BaseDecreaseSpeed;
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

            AbsorbEffect.transform.position = other.transform.position;
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

            _otherPlanetLifeAmount.LifeAmount = 0;
            AbsorbEffect.Stop();

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

            if (_otherPlanetLifeAmount.LifeAmount < AbsorbSpeed * deltaTime)
            {
                return false;
            }

            if (!_isAbsorbBegun)
            {
                _isAbsorbBegun = true;
                AbsorbEffect.Play();
            }
            
            var absorbAmount = deltaTime * AbsorbSpeed;
            _otherPlanetLifeAmount.LifeAmount -= AbsorbSpeed * deltaTime;
            AbsorbEffect.transform.position = _otherPlanetLifeAmount.transform.position;

            this.LifeAmount += absorbAmount;

            return true;
        }

        #endregion

        #region APIs

        public void CollideAndDamageLife()
        {
            // TODO: To make more detailed damage calculation and effects.

            _lifeAmount -= CollideDamage;

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
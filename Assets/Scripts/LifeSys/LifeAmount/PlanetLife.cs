using System;
using UnityEngine;


namespace Flawless.LifeSys
{
    [RequireComponent(typeof(Collider))]
    public class PlanetLife : MonoBehaviour
    {
        /// <summary>
        /// A maximum life amount of a planet.
        /// All planet shares the same max life amount,
        /// in order to make process bar more clear.
        /// </summary>
        public static readonly float MaxLifeAmount = 8000f;
        
        [SerializeField]
        private float _lifeAmount = 1000f;
        
        public event Action<float> OnLifeAmountChanged; 

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
                    _lifeAmount = value;
                }
                
                OnLifeAmountChanged?.Invoke(LifeAmount);
            }
        }

        [Tooltip("Whether this planet can be absorbed by other planets.")]
        public bool IsAbsorbed;

        /// <summary>
        /// Set the planet dead effects.
        /// - Sound Effects
        /// - Particle Effects
        /// - Shader Effects
        /// - etc.
        /// </summary>4
        public virtual void SetPlanetDead()
        {
            // TODO: Set the planet dead effects.
        }
        
        [Header("Collide Result")]
        public float CollideDamage = 200f;
        public float CollideForce = 10f;
        
        /// <summary>
        /// Defines how a planet reacts to a player's collision.
        /// Can be overridden by child classes to make different effects.
        /// This function will automatically be called by <see cref="PlayerLife"/> when a collision happens.
        /// </summary>
        /// <param name="playerRigidbody">Player character's rigidbody.</param>
        /// <param name="playerLife">Player's life component.</param>
        /// <param name="normal">Normal on the contact.</param>
        public virtual void CollideAndDamageLife(Rigidbody playerRigidbody, PlayerLife playerLife,
            Vector3 normal)
        {
            var velocityDir = playerRigidbody.velocity.normalized;
            var boundDirection = velocityDir +
                                 Mathf.Abs(2 * Vector3.Dot(velocityDir, normal)) * normal;
            
            playerRigidbody.AddForce(boundDirection * CollideForce - playerRigidbody.velocity,
                ForceMode.VelocityChange);
            
            playerLife.CollideAndDamageLife(CollideDamage);
        }
    }
}
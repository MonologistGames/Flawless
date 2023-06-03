using System;
using UnityEngine;


namespace Flawless.LifeSys
{
    [RequireComponent(typeof(Collider))]
    public class PlanetLifeAmount : MonoBehaviour
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
        public void SetPlanetDead()
        {
            // TODO: Set the planet dead effects.
        }
    }
}
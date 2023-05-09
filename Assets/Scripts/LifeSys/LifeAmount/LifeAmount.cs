using UnityEngine;

namespace Flawless.LifeSys
{
    public abstract class LifeAmount : MonoBehaviour
    {
        [SerializeField]
        private float _plantAmount;
        [SerializeField]
        private float _animalAmount;

        #region Properties

        /// <summary>
        /// Ratio of animal to planet.
        /// </summary>
        public float AnimalPlanetRatio 
            => _animalAmount / _plantAmount;

        /// <summary>
        /// Plant Amount of this planet.
        /// </summary>
        public float PlantAmount
        {
            get => _plantAmount;
            set
            {
                if (value <= 0)
                {
                    _plantAmount = 0;
                    return;
                }
                
                _plantAmount = value;
            }
        }

        /// <summary>
        /// Animal Amount of this planet.
        /// </summary>
        public float AnimalAmount
        {
            get => _animalAmount;
            set
            {
                if (value <= 0)
                {
                    _animalAmount = 0;
                    return;
                }
                
                _animalAmount = value;
            }
        }
        
        #endregion
    }
}
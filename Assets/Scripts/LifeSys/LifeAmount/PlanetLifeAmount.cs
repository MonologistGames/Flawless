using UnityEngine;


namespace Flawless.LifeSys
{
    [RequireComponent(typeof(Collider))]
    public class PlanetLifeAmount : LifeAmount
    {
        [Tooltip("Whether this planet can be absorbed by other planets.")]
        public bool IsAbsorbed;
        
        /// <summary>
        /// Set the planet dead effects.
        /// - Sound Effects
        /// - Particle Effects
        /// - Shader Effects
        /// - etc.
        /// </summary>
        public void SetPlanetDead()
        {
            // TODO: Set the planet dead effects.
        }
    }
}
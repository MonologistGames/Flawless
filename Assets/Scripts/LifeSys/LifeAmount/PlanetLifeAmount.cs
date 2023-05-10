using UnityEngine;


namespace Flawless.LifeSys
{
    [RequireComponent(typeof(Collider))]
    public class PlanetLifeAmount : LifeAmount
    {
        /// <summary>
        /// A maximum life amount of a planet.
        /// All planet shares the same max life amount,
        /// in order to make process bar more clear.
        /// </summary>
        public static readonly float MaxLifeAmount = 10000;
        
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
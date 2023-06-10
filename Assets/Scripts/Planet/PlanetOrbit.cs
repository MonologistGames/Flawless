using UnityEngine;

namespace Flawless.Planet
{
    public class PlanetOrbit : MonoBehaviour
    {
        private float _orbitRadius;

        public Transform PlanetGroup;
        public Transform OrbitCenter;
        public float OrbitAngleSpeed = 1f;
        

        #region MonoBehavioursß

        private void FixedUpdate()
        {
            // Update planet position
            PlanetGroup.RotateAround(OrbitCenter.position, OrbitCenter.up, OrbitAngleSpeed * Time.deltaTime);
            // PlanetGroup.localEulerAngles = Vector3.zero;
        }

        #endregion
    }
}
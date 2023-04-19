using System;
using Flawless.Utilities;
using UnityEngine;

namespace Flawless.Planet
{
    public class PlanetOrbit : MonoBehaviour
    {
        private CircleDrawer _orbitTrail;
        private float _orbitRadius;

        public Transform Planet;
        public Transform OrbitCenter;
        public float OrbitAngleSpeed = 1f;

        #region MonoBehaviours

        private void OnEnable()
        {
            _orbitRadius = Vector3.Distance(Planet.position, OrbitCenter.position);
            
            _orbitTrail = GetComponentInChildren<CircleDrawer>();

            if (!_orbitTrail) return;
            _orbitTrail.transform.position = OrbitCenter.position;
            _orbitTrail.Radius = _orbitRadius;
        }

        private void FixedUpdate()
        {
            // Update planet position
            Planet.RotateAround(OrbitCenter.position, Vector3.up, OrbitAngleSpeed * Time.deltaTime);
            Planet.eulerAngles = Vector3.zero;
        }

        #endregion
    }
}
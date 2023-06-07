using System;
using System.Collections;
using System.Collections.Generic;
using Flawless.PlayerCharacter;
using Flawless.LifeSys;
using UnityEngine;

namespace Flawless.Planet.PlanetsFunctions
{
    public class PlanetLava : PlanetLife
    {
        [Header("Speed&Acceleration")]
        public float acceleration = 1f;
        [Header("Speed&AccelerationAfterDead")]
        public float accelerationDead = 1.8f;

        [Header("SatelliteRotateSpeed")] 
        public float rotateSpeed;
        public float rotateSpeedDead;


        public Satellite Satellite;
        
        private float _originAcceleration;

        private void Start()
        {
            Satellite.rotateSpeed = rotateSpeed;
        }

        public override void SetPlanetDead()
        {
            
            acceleration = accelerationDead;
            Satellite.rotateSpeed = rotateSpeedDead;
        }

        #region TriggerEvent
        /// <summary>
        /// Change the acceleration
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var playerController = other.GetComponent<PlayerController>();
                _originAcceleration = playerController.Acceleration;
                playerController.Acceleration = acceleration;
            }
        }

        /// <summary>
        /// Regain the acceleration
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().Acceleration = _originAcceleration;
            }
        }
        #endregion
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Flawless.PlayerCharacter;
using Flawless.LifeSys;
using UnityEngine;

namespace Flawless.Planet.PlanetsFunctions
{
    public class PlanetFrozen : PlanetLife
    {
        [Header("Speed&Acceleration")]
        public float maxSpeed = 2f;
        public float acceleration = 1.6f;
        [Header("Speed&AccelerationAfterDead")]
        public float maxSpeedDead = 1.5f;
        public float accelerationDead = 0.8f;
        
        private float _originMaxSpeed;
        private float _originAcceleration;

        public override void SetPlanetDead()
        {
            maxSpeed = maxSpeedDead;
            acceleration = accelerationDead;
        }

        #region TriggerEvent
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var playerController = other.GetComponent<PlayerController>();
                _originMaxSpeed = playerController.MaxSpeed;
                _originAcceleration = playerController.Acceleration;
                playerController.MaxSpeed = maxSpeed;
                playerController.Acceleration = acceleration;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().MaxSpeed = _originMaxSpeed;
                other.GetComponent<PlayerController>().Acceleration = _originAcceleration;
            }
        }
        #endregion
    }
}
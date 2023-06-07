using System;
using System.Collections;
using System.Collections.Generic;
using Flawless.PlayerCharacter;
using Flawless.LifeSys;
using UnityEngine;

namespace Flawless.Planet.PlanetsFunctions
{
    public class PlanetBroken : PlanetLife
    {
        [Header("Speed")]
        public float maxSpeed = 4.5f;
        private float _originMaxSpeed;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var playerController = other.GetComponent<PlayerController>();
                _originMaxSpeed = playerController.MaxSpeed;
                playerController.MaxSpeed = maxSpeed;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Player"))
                other.GetComponent<PlayerController>().MaxSpeed = _originMaxSpeed;
        }
    }
}

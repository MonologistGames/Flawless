using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flawless.LifeSys;
using Unity.VisualScripting;

namespace Flawless.Planet.PlanetsFunctions
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlanetAir : PlanetLife
    {
        private Rigidbody _rb;
        private float _mass;
        public void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _mass = _rb.mass;
        }

        public override void SetPlanetDead()
        {
            _rb.mass = _mass * 2;
        }

        public override void CollideAndDamageLife(Rigidbody playerRigidbody, PlayerLife playerLife, Vector3 normal)
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

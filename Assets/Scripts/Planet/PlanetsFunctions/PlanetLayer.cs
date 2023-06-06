using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flawless.LifeSys;
using Unity.VisualScripting;

namespace Flawless.Planet.PlanetsFunctions
{
    [RequireComponent(typeof(GravitationField))]
    public class PlanetLayer : PlanetLife
    {
        private GravitationField _gravitationField;
        private float _mass;
        
        
        public void Start()
        {
            _gravitationField = GetComponent<GravitationField>();
            _mass = _gravitationField.Mass;
        }

        

        public override void SetPlanetDead()
        {
            _gravitationField.Mass = _mass * 1.5f;
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

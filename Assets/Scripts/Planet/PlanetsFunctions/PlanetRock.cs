using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flawless.LifeSys;
using Unity.VisualScripting;

namespace Flawless.Planet.PlanetsFunctions
{
    [RequireComponent(typeof(GravitationField))]
    public class PlanetRock : PlanetLife
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
        
        
    }
}
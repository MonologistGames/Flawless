using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flawless.LifeSys;
using Unity.VisualScripting;

namespace Flawless.Planet.PlanetsFunctions
{
    public class PlanetSkin : PlanetLife
    {
        public float CollideForceAfterDead = 100f;
        public override void SetPlanetDead()
        {
            CollideForce = CollideForceAfterDead;
        }
        
    }
}
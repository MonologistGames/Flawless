using System.Collections;
using System.Collections.Generic;
using Flawless.Planet;
using UnityEngine;

namespace Flawless
{
    public class MapElementManager : MonoBehaviour
    {
        public string AreaName;
        
        public List<Vector3> JumpGatePositions;
        public List<PlanetOrbit> PlanetOrbits;
    }
}

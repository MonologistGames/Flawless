using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flawless.BriefMap
{
    [CreateAssetMenu(fileName = "MapIconRef", menuName = "Map Icon Reference", order = 0)]
    public class MapIconRef : ScriptableObject
    {
        public GameObject SunIcon;
        public GameObject PlanetIcon;
        public GameObject BlackIcon;
        public GameObject PlayerIcon;
        public GameObject JumpGateIcon;
        public GameObject ViewGateIcon;
    }
}

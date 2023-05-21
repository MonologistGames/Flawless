using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flawless.BriefMap
{
    [CreateAssetMenu(fileName = "MapIconRef", menuName = "Map Icon Reference", order = 0)]
    public class MapIconRef : ScriptableObject
    {
        public Sprite SunIcon;
        public Sprite PlanetIcon;
        public Sprite PlayerIcon;
        public Sprite JumpGateIcon;
        public Sprite ViewGateIcon;
    }
}

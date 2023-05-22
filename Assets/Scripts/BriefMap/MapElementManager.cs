using System;
using System.Collections;
using System.Collections.Generic;
using Flawless.BriefMap;
using Flawless.Planet;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace Flawless
{
    public class MapElementManager : MonoBehaviour
    {
        public MapIconRef MapIconReference;
        public float UnitRatio = 10f;
        public string AreaName;

        public Transform PlayerPosition;
        public Transform BlackPosition;
        public List<Transform> JumpGatePositions;
        public List<PlanetOrbit> PlanetOrbits;
        public List<Transform> SunPositions;

        private List<Image> _jumpGateIcons;
        private List<Image> _planetIcons;
        private List<Image> _sunIcons;

        public Image PlayerIcon;
        public Image BlackIcon;

        private void Start()
        {
            // Initialize Jump Gates
            _jumpGateIcons = new List<Image>();
            foreach (var jumpGatePosition in JumpGatePositions)
            {
                var jumpGateIcon = Instantiate(MapIconReference.JumpGateIcon, transform);
                _jumpGateIcons.Add(jumpGateIcon.GetComponent<Image>());
            }
            
            _planetIcons = new List<Image>();
            // Initialize Planets
            foreach (var planetOrbit in PlanetOrbits)
            {
                var planetIcon = Instantiate(MapIconReference.PlanetIcon, transform);
                _planetIcons.Add(planetIcon.GetComponent<Image>());
            }

            _sunIcons = new List<Image>();
            // Initialize Suns
            foreach (var sunPosition in SunPositions)
            {
                var sunIcon = Instantiate(MapIconReference.SunIcon, transform);
                _sunIcons.Add(sunIcon.GetComponent<Image>());
            }
        }

        private void Update()
        {
            // Update Black Position
            var distVector = (BlackPosition.position - PlayerPosition.position) / UnitRatio;
            BlackIcon.rectTransform.anchoredPosition = PlayerIcon.rectTransform.anchoredPosition +
                                               new Vector2(distVector.x, distVector.z);
            // Update Jump Gates Position
            for (int i = 0; i < JumpGatePositions.Count; i++)
            {
                var jumpGatePosition = JumpGatePositions[i].transform.position;
                var jumpGateIcon = _jumpGateIcons[i];
                distVector = (jumpGatePosition - PlayerPosition.position) / UnitRatio;
                jumpGateIcon.rectTransform.anchoredPosition = PlayerIcon.rectTransform.anchoredPosition +
                                                      new Vector2(distVector.x, distVector.z);
            }

            // Update Planet Position
            for (int i = 0; i < PlanetOrbits.Count; i++)
            {
                var planetPosition = PlanetOrbits[i].Planet.position;
                var planetIcon = _planetIcons[i];
                distVector = (planetPosition - PlayerPosition.position) / UnitRatio;
                planetIcon.rectTransform.anchoredPosition = PlayerIcon.rectTransform.anchoredPosition +
                                                            new Vector2(distVector.x, distVector.z);
            }

            // Update Sun Position
            for (int i = 0; i < SunPositions.Count; i++)
            {
                var sunPosition = SunPositions[i].transform.position;
                var sunIcon = _sunIcons[i];
                distVector = (sunPosition - PlayerPosition.position) / UnitRatio;
                sunIcon.rectTransform.anchoredPosition = PlayerIcon.rectTransform.anchoredPosition +
                                                 new Vector2(distVector.x, distVector.z);
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Flawless.BriefMap;
using TMPro;
using UnityEngine;
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

        private List<Transform> _jumpGatePositions = new List<Transform>();
        private List<Transform> _planetOrbits = new List<Transform>();
        private List<Transform> _sunPositions = new List<Transform>();

        private List<Image> _jumpGateIcons = new List<Image>();
        private List<Image> _planetIcons = new List<Image>();
        private List<Image> _sunIcons = new List<Image>();

        public Image PlayerIcon;
        public Image BlackIcon;
        public TextMeshProUGUI AreaNameText;

        private void Start()
        {
            Debug.Log(FindObjectsOfType<MapIconMarker>().Length);
            foreach (var iconElement in GameObject.FindObjectsOfType<MapIconMarker>())
            {
                switch (iconElement.MapTag)
                {
                    case "JumpGate":
                        _jumpGatePositions.Add(iconElement.transform);
                        break;
                    case "Planet":
                        _planetOrbits.Add(iconElement.transform);
                        break;
                    case "Sun":
                        _sunPositions.Add(iconElement.transform);
                        break;
                }
            }
            
            // Initialize Jump Gates
            foreach (var jumpGateIcon in _jumpGatePositions.Select(jumpGatePosition => Instantiate(MapIconReference.JumpGateIcon, transform)))
            {
                _jumpGateIcons.Add(jumpGateIcon.GetComponent<Image>());
            }
            

            // Initialize Planets
            foreach (var planetIcon in _planetOrbits.Select(planetOrbit => Instantiate(MapIconReference.PlanetIcon, transform)))
            {
                _planetIcons.Add(planetIcon.GetComponent<Image>());
            }


            // Initialize Suns
            foreach (var sunIcon in _sunPositions.Select(sunPosition => Instantiate(MapIconReference.SunIcon, transform)))
            {
                _sunIcons.Add(sunIcon.GetComponent<Image>());
            }
        }

        
        private void Update()
        {
            AreaNameText.text = AreaName;
            
            // Update Black Position
            var distVector = (BlackPosition.position - PlayerPosition.position) / UnitRatio;
            BlackIcon.rectTransform.anchoredPosition = PlayerIcon.rectTransform.anchoredPosition +
                                               new Vector2(distVector.x, distVector.z);
            // Update Jump Gates Position
            for (int i = 0; i < _jumpGatePositions.Count; i++)
            {
                var jumpGatePosition = _jumpGatePositions[i].transform.position;
                var jumpGateIcon = _jumpGateIcons[i];
                distVector = (jumpGatePosition - PlayerPosition.position) / UnitRatio;
                jumpGateIcon.rectTransform.anchoredPosition = PlayerIcon.rectTransform.anchoredPosition +
                                                      new Vector2(distVector.x, distVector.z);
            }

            // Update Planet Position
            for (int i = 0; i < _planetOrbits.Count; i++)
            {
                var planetPosition = _planetOrbits[i].position;
                var planetIcon = _planetIcons[i];
                distVector = (planetPosition - PlayerPosition.position) / UnitRatio;
                planetIcon.rectTransform.anchoredPosition = PlayerIcon.rectTransform.anchoredPosition +
                                                            new Vector2(distVector.x, distVector.z);
            }

            // Update Sun Position
            for (int i = 0; i < _sunPositions.Count; i++)
            {
                var sunPosition = _sunPositions[i].transform.position;
                var sunIcon = _sunIcons[i];
                distVector = (sunPosition - PlayerPosition.position) / UnitRatio;
                sunIcon.rectTransform.anchoredPosition = PlayerIcon.rectTransform.anchoredPosition +
                                                 new Vector2(distVector.x, distVector.z);
            }
        }
    }
}
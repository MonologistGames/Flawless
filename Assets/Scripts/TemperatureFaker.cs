using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Flawless
{
    public class TemperatureFaker : MonoBehaviour
    {
        public Transform Player;
        public Transform Sun;

        private TextMeshProUGUI _text;

        public float StandardTemperature;
        public float DistanceFactor;
        public float DistanceThreshold;

        private void OnEnable()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            var dist = Vector3.Distance(Player.position, Sun.position);

            var temperature = StandardTemperature - (dist - DistanceThreshold) / DistanceFactor;
            
            _text.text = temperature.ToString("F1") + "℃";
        }
    }
}

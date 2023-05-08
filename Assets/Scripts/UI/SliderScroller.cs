using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Flawless.UI
{
    [RequireComponent(typeof(Slider))]
    public class SliderScroller : MonoBehaviour
    {
        private float _lastValue;
        private Slider _slider;

        public float ScrollSpeed = 720f;
        public RectTransform Scroller;


        // Start is called before the first frame update
        void Start()
        {
            _slider = GetComponent<Slider>();
            _lastValue = _slider.value;
        }

        public void ScrollWithSlider(float value)
        {
            //if (Math.Abs(_lastValue - _slider.value) < 0.001f) return;

            float valueChange = value - _lastValue;
            valueChange /= _slider.maxValue;
            _lastValue = value;

            Scroller.Rotate(Vector3.back, valueChange * ScrollSpeed);
        }
    }
}
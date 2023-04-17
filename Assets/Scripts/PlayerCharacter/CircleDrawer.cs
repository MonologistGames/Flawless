using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flawless.PlayerCharacter
{
    [RequireComponent(typeof(LineRenderer))]
    public class CircleDrawer : MonoBehaviour
    {
        public float Radius = 1f;
        public int Segments = 64;
        public float Width = 0.1f;
        public Color Color = Color.white;
        public bool AutoUpdate = true;

        private LineRenderer _lineRenderer;

        private void OnValidate()
        {
            if (_lineRenderer == null)
                _lineRenderer = GetComponent<LineRenderer>();
            
            DrawCircle();
        }

        private void OnEnable()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            if (!AutoUpdate) return;
            DrawCircle();
        }

        private void DrawCircle()
        {
            _lineRenderer.positionCount = Segments + 1;
            
            _lineRenderer.startWidth = Width;
            _lineRenderer.endWidth = Width;
            _lineRenderer.startColor = Color;
            _lineRenderer.endColor = Color;

            float deltaTheta = (2f * Mathf.PI) / Segments;
            float theta = 0f;

            for (int i = 0; i < Segments + 1; i++)
            {
                float x = Radius * Mathf.Cos(theta);
                float z = Radius * Mathf.Sin(theta);
                Vector3 pos = new Vector3(x, 0, z);
                _lineRenderer.SetPosition(i, pos);
                theta += deltaTheta;
            }
        }
    }
}


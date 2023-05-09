using UnityEngine;

namespace Flawless.Utilities
{
    [RequireComponent(typeof(LineRenderer))]
    public class CircleDrawer : MonoBehaviour
    {
        private const float SegRadiusRatio = 8f;

        /// <summary>
        /// Radius of the circle.
        /// </summary>
        [Tooltip("Radius of the circle.")]
        public float Radius = 1f;

        /// <summary>
        /// Number of segments used to draw the circle.
        /// More segments = smoother circle.
        /// </summary>
        [Tooltip("Number of segments used to draw the circle.\nMore segments = smoother circle.")]
        public int Segments = 64;

        /// <summary>
        /// Width of the line to draw the circle.
        /// </summary>
        [Tooltip("Width of the line to draw the circle.")]
        public float Width = 0.1f;

        /// <summary>
        /// Color of the circle.
        /// </summary>
        [Tooltip("Color of the circle.")]
        public Color Color = Color.white;

        /// <summary>
        /// Decides whether to update the circle every frame.
        /// </summary>
        [Tooltip("Decides whether to update the circle every frame.")]
        public bool AutoUpdate = true;

        private LineRenderer _lineRenderer;

        #region Editor

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_lineRenderer == null)
                _lineRenderer = GetComponent<LineRenderer>();
            
            DrawCircle();
        }
#endif

        #endregion

        #region MonoBehaviours

        private void OnEnable()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            if (!AutoUpdate) return;
            
            DrawCircle();
        }

        #endregion

        /// <summary>
        /// Draw a circle using a LineRenderer.
        /// </summary>
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
using UnityEngine;

namespace Flawless.PlayerCharacter
{
    public class VelocityIndicator : MonoBehaviour
    {
        private Vector3 _velocityStart;
        private Vector3 _velocityEnd;
        private Vector3 _velocity;
        private float _velocityMagnitudePercent;

        [Min(0f)] public float DistanceFromOrigin = 0.5f;
        [Min(0f)]public float IndicatorLength = 0.5f;
        public LineRenderer VelocityLineRenderer;
        
        public CircleDrawer IndicatorRim;

        private PlanetController _planet;

#if UNITY_EDITOR

        #region Editor
        
        private void OnValidate()
        {
            if (IndicatorRim)
            {
                IndicatorRim.Radius = DistanceFromOrigin;
            }
        }
        
        #endregion

#endif

        #region MonoBehaviours

        private void OnEnable()
        {
            _planet = GetComponentInParent<PlanetController>();
        }

        private void Update()
        {
            UpdateVelocityIndicator();   
        }

        private void UpdateVelocityIndicator()
        {
            _velocity = _planet.Velocity;
            _velocityMagnitudePercent = _velocity.magnitude / _planet.MaxMotivation;

            // Calculate start and end position of the velocity indicator
            var position = _planet.transform.position;
            _velocityStart = position + _velocity.normalized * DistanceFromOrigin;
            _velocityEnd = position +
                           _velocity.normalized * (_velocityMagnitudePercent * IndicatorLength + DistanceFromOrigin);

            // Set LineRenderer start and end position
            VelocityLineRenderer.SetPosition(0, _velocityStart);
            VelocityLineRenderer.SetPosition(1, _velocityEnd);


            // Set the rim size of the velocity indicator
            IndicatorRim.Radius = DistanceFromOrigin + _velocityMagnitudePercent * IndicatorLength;
        }

        #endregion
    }
}
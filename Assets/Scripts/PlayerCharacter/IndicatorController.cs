using Flawless.Utilities;
using UnityEngine;

namespace Flawless.PlayerCharacter
{
    public class IndicatorController : MonoBehaviour
    {
        private PlayerController _player;
        
        // Trail
        [Header("Trail")]
        public float TrailDistanceFromOrigin = 0.1f;
        public float TrailLengthThreshold = 0.2f;
        public TrailRenderer Trail;
        public Gradient OverDriveColor;
        public Gradient NormalColor;

        // Motivation
        [Header("Motivation")]
        public Transform Indicator;
        
        // Velocity
        private Vector3 _velocityStart;
        private Vector3 _velocityEnd;
        private Vector3 _velocity;
        private float _velocityMagnitudePercent;
        [Header("Velocity")]
        [Min(0f)] public float VelocityDistanceFromOrigin = 0.5f;
        [Min(0f)]public float IndicatorLength = 0.5f;
        public LineRenderer VelocityLineRenderer;
        public float ScaleFacter = 0.03f;

        #region MonoBehaviours

        private void OnEnable()
        {
            _player = GetComponentInParent<PlayerController>();
        }

        private void Update()
        {
            this.transform.localPosition = - _player.Velocity * ScaleFacter;
            
            UpdateIndicatorDirection();
            
            UpdateTrailPosition();
            UpdateTrailColor();
            
            UpdateVelocityIndicator();
        }

        #endregion
        
        // Update the direction of the motivation indicator
        private void UpdateIndicatorDirection()
        {
            float angleY = Mathf.Asin(_player.MoveDir.z) / Mathf.PI * 180f - 90f;
            if (_player.MoveDir.x > 0)
                angleY = -angleY;

            Indicator.eulerAngles = new Vector3(90, angleY, 0);
        }
        
        // Update the position of the trail renderer
        private void UpdateTrailPosition()
        {
            // Set TrailRenderer enabled or disabled depending on the velocity of the planet
            Trail.enabled = !(_player.Velocity.sqrMagnitude < TrailLengthThreshold);
            
            // Set the TrailRenderer at the opposite position of velocity
            Trail.transform.position = _player.transform.position - _player.Velocity.normalized * TrailDistanceFromOrigin;
        }
        
        private void UpdateTrailColor()
        {
            if (_player.IsOverDriving) Trail.colorGradient = OverDriveColor;
            else Trail.colorGradient = NormalColor;
        }
        
        // Update the position and size of the velocity indicator
        private void UpdateVelocityIndicator()
        {
            _velocity = _player.Velocity;
            _velocityMagnitudePercent = _velocity.magnitude / _player.MaxSpeed;

            // Calculate start and end position of the velocity indicator
            //var position = _planet.transform.position;
            _velocityStart = _velocity.normalized * VelocityDistanceFromOrigin;
            _velocityEnd = _velocity.normalized * (_velocityMagnitudePercent * IndicatorLength + VelocityDistanceFromOrigin);

            // Set LineRenderer start and end position
            VelocityLineRenderer.SetPosition(0, _velocityStart);
            VelocityLineRenderer.SetPosition(1, _velocityEnd);
        }
    }
}


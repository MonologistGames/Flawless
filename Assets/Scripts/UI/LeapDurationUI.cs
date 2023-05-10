using Flawless.LifeSys;
using Flawless.PlayerCharacter;
using UnityEngine;
using UnityEngine.UI;

namespace Flawless.UI
{
    public class LeapDurationUI : MonoBehaviour
    {
        private Image _leapIndicator;
        private Color _originalColor;
        
        public PlanetController PlayerController;
        public Color ChargingColor;

        private void Start()
        {
            _leapIndicator = GetComponent<Image>();
            _originalColor = _leapIndicator.color;
        }

        private void Update()
        {
            _leapIndicator.fillAmount = 1 - PlayerController.LeapTimer / PlayerController.LeapDuration;

            _leapIndicator.color = !PlayerController.IsLeapReady ? ChargingColor : _originalColor;
        }
    }
}
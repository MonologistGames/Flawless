using Flawless.PlayerCharacter;
using UnityEngine;
using UnityEngine.UI;

namespace Flawless.UI
{
    public class LeapDurationUI : MonoBehaviour
    {
        // Internal Components
        private Image _leapIndicator;
        private Color _originalColor;
        
        [Tooltip("PlayerController component of the player")]
        public PlayerController PlayerController;
        [Tooltip("Color when charging")]
        public Color ChargingColor;
        
        // Set up references
        private void Start()
        {
            _leapIndicator = GetComponent<Image>();
            _originalColor = _leapIndicator.color;
        }
        
        // Update UI color and process.
        private void Update()
        {
            _leapIndicator.fillAmount = PlayerController.LeapTimer.GetProcess(true);
            _leapIndicator.color = !PlayerController.IsLeapReady ? ChargingColor : _originalColor;
        }
    }
}
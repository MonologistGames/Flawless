using Flawless.PlayerCharacter;
using UnityEngine;

namespace Flawless.Planet
{
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlanetGravitationField : MonoBehaviour
    {
        private const float GravitationFactor = 5f;
        private const float GravitationPower = 1.5f;

        private float _mass;

        private Vector3 _prevGrav;

        private PlanetController _planet;

        #region MonoBehaviours

        private void OnEnable()
        {
            _mass = GetComponent<Rigidbody>().mass;
        }

        #endregion

        #region Trigger Events

        private void OnTriggerStay(Collider other)
        {
            // Returns if the object is not a player
            if (!other.CompareTag("Player")) return;

            // Get the PlanetController Component of the other object
            _planet = other.GetComponent<PlanetController>();
            if (!_planet) return; // Return if null

            Vector3 gravitationVector =
                this.transform.position - _planet.transform.position; //Vector from player to this planet

            // TODO: Adjust Gravitation Calculation to have a more interesting movement controller
            Vector3 gravitation = (gravitationVector.normalized) *
                                  (GravitationFactor * _mass /
                                   Mathf.Pow(gravitationVector.magnitude, GravitationPower)); //Calculate Gravitation

            _planet.Gravitation += gravitation - _prevGrav; //Set Gravitation
            _prevGrav = gravitation;
        }

        private void OnTriggerExit(Collider other)
        {
            // Returns if the object is not a player
            if (!other.CompareTag("Player")) return;

            _planet = other.GetComponent<PlanetController>();
            if (!_planet) return; // Return if null

            _planet.Gravitation -= _prevGrav;
            _prevGrav = Vector3.zero;
        }

        #endregion
    }
}
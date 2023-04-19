using System.Linq;
using Flawless.PlayerCharacter;
using UnityEngine;

namespace Flawless.Planet
{
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlanetGravitationField : MonoBehaviour
    {
        public static readonly float FieldRatio = 8f;

        private const float GravitationFactor = 3f;

        private float _mass;

        private Vector3 _prevGrav;

        private PlanetController _planet;


        #region Editor

#if UNITY_EDITOR
        private void OnValidate()
        {
            _mass = GetComponent<Rigidbody>().mass;
            SphereCollider planetCollider = GetComponents<SphereCollider>()
                .First(sphereCollider => !sphereCollider.isTrigger);
            SphereCollider gravitationFieldTrigger = GetComponents<SphereCollider>()
                .First(sphereCollider => sphereCollider.isTrigger);

            gravitationFieldTrigger.radius = planetCollider.radius * FieldRatio;
        }
#endif

        #endregion

        #region MonoBehaviours

        private void OnEnable()
        {
            _mass = GetComponent<Rigidbody>().mass;
        }

        #endregion

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
                                   Mathf.Pow(gravitationVector.magnitude, 1.5f)); //Calculate Gravitation

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
    }
}
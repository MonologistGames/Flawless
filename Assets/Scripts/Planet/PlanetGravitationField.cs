using UnityEngine;

namespace Flawless.Planet
{
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlanetGravitationField : MonoBehaviour
    {
        private const float GravitationFactor = 5f;
        private const float GravitationPower = 1.5f;

        private Rigidbody _rigidbody;

        #region MonoBehaviours

        private void OnEnable()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        #endregion

        #region Trigger Events

        private void OnTriggerStay(Collider other)
        {
            var inFieldRigidbody = other.gameObject.GetComponent<Rigidbody>();
            if (inFieldRigidbody == null || inFieldRigidbody.isKinematic || inFieldRigidbody.gameObject == this.gameObject)
                return;
            
            Vector3 gravitationVector =
                this.transform.position - other.transform.position; //Vector from player to this planet

            // TODO: Adjust Gravitation Calculation to have a more interesting movement controller
            Vector3 gravitation = (gravitationVector.normalized) *
                                  (GravitationFactor * _rigidbody.mass /
                                   Mathf.Pow(gravitationVector.magnitude, GravitationPower)); //Calculate Gravitation
            
            inFieldRigidbody.AddForce(gravitation, ForceMode.Acceleration); //Apply Gravitation
        }

        #endregion
    }
}
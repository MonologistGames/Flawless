using UnityEngine;

namespace Flawless.Planet
{
    [RequireComponent(typeof(SphereCollider))]
    public class GravitationField : MonoBehaviour
    {
        private const float GravitationFactor = 5f;
        private const float GravitationPower = 1.5f;

        public float Mass;

        #region Trigger Events

        private void OnTriggerStay(Collider other)
        {
            var inFieldRigidbody = other.gameObject.GetComponent<Rigidbody>();
            // Exclude gravity source itself and kinematic rigidbodies
            if (inFieldRigidbody == null || inFieldRigidbody.isKinematic || inFieldRigidbody.gameObject == this.gameObject)
                return;
            
            Vector3 gravitationVector =
                this.transform.position - other.transform.position; //Vector from player to this planet

            // TODO: Adjust Gravitation Calculation to have a more interesting movement controller
            Vector3 gravitation = (gravitationVector.normalized) *
                                  (GravitationFactor * Mass /
                                   Mathf.Pow(gravitationVector.magnitude, GravitationPower)); //Calculate Gravitation
            
            inFieldRigidbody.AddForce(gravitation, ForceMode.Acceleration); //Apply Gravitation
        }

        #endregion
    }
}
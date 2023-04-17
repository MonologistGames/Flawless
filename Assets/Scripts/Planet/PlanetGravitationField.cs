using System.Linq;
using UnityEngine;

namespace Flawless.PlayerCharacter
{

    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlanetGravitationField : MonoBehaviour
    {
        public static readonly float FieldRatio = 8f;

        private const float GravitationFactor = 3f;

        private float _mass;

        private Vector3 _prevGrav;

        private PlanetController planet;

        private void OnEnable()
        {
            _mass = GetComponent<Rigidbody>().mass;
        }
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


        private void OnTriggerStay(Collider other)
        {
            planet = other.GetComponent<PlanetController>();
            if (planet)
            {
                Vector3 gravitationVector =
                    this.transform.position - planet.transform.position; //Vector from player to this planet
                Vector3 gravitation = (gravitationVector.normalized) *
                                      (GravitationFactor * _mass /
                                       Mathf.Pow(gravitationVector.magnitude, 1.5f)); //Calculate Gravitation

                planet.Gravitation += gravitation - _prevGrav; //Set Gravitation
                _prevGrav = gravitation;
                return;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            planet = other.GetComponent<PlanetController>();
            if (planet)
            {
                planet.Gravitation -= _prevGrav;
                _prevGrav = Vector3.zero;

                return;
            }
        }
    }
}
using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlanetGravitationField : MonoBehaviour
{
    public static readonly float FieldRatio = 8f;

    private const float GravitationFactor = 0.1f;

    private Vector3 _previousGravitation;
    private float _mass;

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
        PlanetController planet = other.GetComponent<PlanetController>();
        if (planet) { //Calculate gravitation
            Vector3 gravitationVector = planet.transform.position - this.transform.position;
            Vector3 currentGravitation = (gravitationVector.normalized) *
                                         (GravitationFactor * _mass * Mathf.Pow(gravitationVector.magnitude, 2f));
             planet.gravitation -= (currentGravitation - _previousGravitation);
             _previousGravitation = currentGravitation;
             return;}
    }

    private void OnTriggerExit(Collider other)
    {
        PlanetController planet = other.GetComponent<PlanetController>();
         if (planet) { 
             planet.gravitation += _previousGravitation;
             return;}
    }
}
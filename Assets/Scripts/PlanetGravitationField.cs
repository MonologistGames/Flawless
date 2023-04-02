using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlanetGravitationField : MonoBehaviour
{
    public static readonly float FieldRatio = 8f;

    private const float GravitationFactor = 3f;
    
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
        if (planet) { 
            Vector3 gravitationVector = -(planet.transform.position - this.transform.position);//Vector from player to this planet
            Vector3 gravitation = (gravitationVector.normalized) *
                                         (GravitationFactor * _mass / Mathf.Pow(gravitationVector.magnitude, 2f));//Calculate gravitation
             
             planet.gravitation = gravitation; //Set gravitation
             
             return;}
    }
}
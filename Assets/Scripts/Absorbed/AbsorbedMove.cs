using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorbedMove : MonoBehaviour
{
    public float moveSpeed = 30f;

    public PlanetController fatherObj;

    private Vector3 _moveVector;
    
    void FixedUpdate()
    {
        _moveVector = fatherObj.transform.position - this.transform.position;
        this.transform.Translate(_moveVector*moveSpeed*Time.deltaTime,Space.World);
        if (_moveVector.magnitude <=0.4f)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetFather(PlanetController obj)
    {
        fatherObj = obj;
    }

    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         Destroy(this.gameObject);
    //     }
    // }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         Destroy(this.gameObject);
    //     }
    // }
}

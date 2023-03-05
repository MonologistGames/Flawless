using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlanet : MonoBehaviour
{
    public float spinSpeed=100;
    private float _pGravity;
    private Vector3 _pos;
    

    private void Start()
    {
        _pos = this.transform.position;
        _pGravity = spinSpeed / Mathf.Pow(Vector3.Distance(_pos, Vector3.zero), 1.5f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.RotateAround(Vector3.zero,Vector3.up,_pGravity
                                                            *Time.deltaTime);
    }
}

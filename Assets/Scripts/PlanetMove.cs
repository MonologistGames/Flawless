using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlanetMove : MonoBehaviour
{
    private Rigidbody _rb;
    private float _gravity;
    private Transform _camera;
    private Transform _point;//指示器
    
    private Vector3 _V;//指向吸引星球的向量
    
    private Vector3 _vthis;//自己星球的位置
    private Vector3 _vp;//其他星球的位置
    private Vector3 _forceDir;//控制时力的方向
    
    [SerializeField] Collider c1;
    [SerializeField] Collider c2;
    [SerializeField] Collider c3;

    public float gravityForce = 5.0f;

    public float moveForce = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        _camera = this.transform.Find("Main Camera");
        _point = this.transform.Find("Point");
        _rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        OnTriggerStay(c1);
        OnTriggerStay(c2);
        OnTriggerStay(c3);
    }

    private void OnTriggerStay(Collider other)
    {
        _vp = other.transform.position;
        _vthis = this.transform.position;
        _V = Vector3.Normalize(_vp - _vthis);
        _gravity=gravityForce /Mathf.Pow(Vector3.Distance(_vthis, _vp),2);
        if (_gravity > 20)
        {
            _gravity = 20;
        }
        _rb.AddForce(_V*_gravity,ForceMode.Force);
    }
    
}

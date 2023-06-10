using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Flawless
{
    public class GetLightDirection : MonoBehaviour
    {
        public Transform LightPos;

        private Material _material;

        private int id;

        private void Start()
        {
            LightPos = GameObject.FindWithTag("Sun").transform;
            _material = GetComponent<Renderer>().material;
            id = Shader.PropertyToID("_LightDir");
        }

        // Update is called once per frame
        void Update()
        {
            _material.SetVector(id, Vector4.Normalize(LightPos.position - this.transform.position));
        }
    }
}
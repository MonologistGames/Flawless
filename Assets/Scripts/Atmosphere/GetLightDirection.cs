using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Flawless
{
    public class GetLightDirection : MonoBehaviour
    {
        public Transform lightPos;

        private Material material;

        private int id;

        private void Start()
        {
            
            material= GetComponent<Renderer>().material;
            id = Shader.PropertyToID("_LightDir");
        }

        // Update is called once per frame
        void Update()
        {
            material.SetVector(id, Vector4.Normalize(lightPos.position-this.transform.position));
        }
    }
}

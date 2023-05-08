using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Flawless
{
    public class GetLightDirection : MonoBehaviour
    {
        private Transform lightPos;

        private Material material;

        private void Start()
        {
            lightPos= GameObject.Find("Sun").transform;
            material= GetComponent<Renderer>().material;
        }

        // Update is called once per frame
        void Update()
        {
            material.SetVector("_LightDir", Vector4.Normalize(lightPos.position-this.transform.position));
        }
    }
}

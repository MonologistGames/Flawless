using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flawless.Planet.PlanetsFunctions
{
    public class Satellite : MonoBehaviour
    {
        public float rotateSpeed;
       

        // Update is called once per frame
        void Update()
        {
            transform.RotateAround(transform.parent.position, Vector3.up, rotateSpeed * Time.deltaTime);
        }
    }
}

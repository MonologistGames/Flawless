using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flawless.Utilities
{
    public class Billboard : MonoBehaviour
    {
        private Transform _targetCamera;

        private void Start()
        {
            _targetCamera = Camera.main.transform;
            transform.forward = _targetCamera.forward;
        }
        
        // Update is called once per frame
        void Update()
        {
            transform.forward = _targetCamera.forward;
        }
    }
}
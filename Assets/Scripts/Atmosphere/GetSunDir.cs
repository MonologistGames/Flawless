using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flawless
{
    public class GetSunDir : MonoBehaviour
    {
        public Transform lightPos;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            this.transform.right = Vector3.Normalize(lightPos.position - this.transform.position);
        }
    }
}

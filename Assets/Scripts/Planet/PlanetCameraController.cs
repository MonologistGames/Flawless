using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Serialization;

namespace Flawless.Planet
{
    [RequireComponent(typeof(Collider))]
    public class PlanetCameraController : MonoBehaviour
    {
        public CinemachineVirtualCamera VirtualCamera;

        #region MonoBehaviours

        #endregion
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            VirtualCamera.enabled = true;
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            VirtualCamera.enabled = false;
        }
    }
}
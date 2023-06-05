using System;
using Cinemachine;
using UnityEngine;
using Flawless.UI;
using Flawless.UI.LifeAmount;

namespace Flawless.Planet
{
    public class GroupTrigger : MonoBehaviour
    {
        public CinemachineVirtualCamera PlanetVC;
        public GameObject Planets;

        private PlanetLifeUI[] _worldUIList;

        private void OnEnable()
        {
            _worldUIList = Planets.GetComponentsInChildren<PlanetLifeUI>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            PlanetVC.enabled = true;

            for (int i = 0; i < _worldUIList.Length; i++)
            {
                _worldUIList[i].TurnCanvasOn();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            PlanetVC.enabled = false;

            for (int i = 0; i < _worldUIList.Length; i++)
            {
                _worldUIList[i].TurnCanvasOff();
            }
        }
    }
}
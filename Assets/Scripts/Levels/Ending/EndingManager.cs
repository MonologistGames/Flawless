using System;
using Flawless.LifeSys;
using UnityEngine;
using UnityEngine.Video;

namespace Flawless
{
    public class EndingManager : MonoBehaviour
    {
        public float RequiredLife;
        public PlayerLife PlayerLife;
        
        public GameObject Ending1;
        private void Update()
        {
            if (PlayerLife.LifeAmount < RequiredLife)
            {
                Ending1.SetActive(false);
            }
            else
            {
                Ending1.SetActive(true);
            }
        }
    }
}
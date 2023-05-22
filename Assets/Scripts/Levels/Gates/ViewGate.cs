using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flawless
{
    public class ViewGate : MonoBehaviour
    {
        public Animator Animator;
        private static readonly int Triggered = Animator.StringToHash("Triggered");

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            {
                Animator.SetBool(Triggered, true);
            }
        }
    }
}

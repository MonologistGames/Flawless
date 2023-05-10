using System;
using UnityEngine;

namespace Flawless.Levels.Gates
{
    public class JumpGate : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
        }
    }
}
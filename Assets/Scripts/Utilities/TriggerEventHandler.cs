using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Flawless
{
    [RequireComponent(typeof(Collider))]
    public class TriggerEventHandler : MonoBehaviour
    {
        public bool IsTagCompareOn;
        public List<string> Tags;
        
        public UnityEvent<Collider> TriggerEnterEvent;
        public UnityEvent<Collider> TriggerStayEvent;
        public UnityEvent<Collider> TriggerExitEvent;
        void OnTriggerEnter(Collider other)
        {
            if (IsTagCompareOn)
            {
                for (int i = 0; i < Tags.Count; i++)
                {
                    if (other.CompareTag(Tags[i]))
                    {
                        TriggerEnterEvent.Invoke(other);
                        return;
                    }
                }
            }
            else
            {
                TriggerEnterEvent.Invoke(other);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (IsTagCompareOn)
            {
                for (int i = 0; i < Tags.Count; i++)
                {
                    if (other.CompareTag(Tags[i]))
                    {
                        TriggerStayEvent.Invoke(other);
                        return;
                    }
                }
            }
            else
            {
                TriggerStayEvent.Invoke(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (IsTagCompareOn)
            {
                for (int i = 0; i < Tags.Count; i++)
                {
                    if (other.CompareTag(Tags[i]))
                    {
                        TriggerExitEvent.Invoke(other);
                        return;
                    }
                }
            }
            else
            {
                TriggerExitEvent.Invoke(other);
            }
        }
    }
}

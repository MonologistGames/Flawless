using System;
using Flawless.PlayerCharacter;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Flawless.Levels.Gates
{
    public class LifeGate : MonoBehaviour
    {
        private Animator _animator;
        [FormerlySerializedAs("isTriggered")] public bool IsTriggered;
        public GameObject ShellObj;
        public FMODUnity.StudioEventEmitter OpenSound;
        public FMODUnity.StudioEventEmitter RejectSound;
        public FMODUnity.StudioEventEmitter IdleSound;

        private void OnEnable()
        {
            _animator = GetComponentInChildren<Animator>();
            if (IsTriggered)
                ShellObj.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (IsTriggered) return;
            
            var playerController = other.GetComponent<PlayerController>();
            if (playerController == null) return;

            if (!playerController.IsOverDriving)
            {
                var a = playerController.Velocity.normalized * playerController.MaxSpeed;
                RejectSound.Play();
                
                playerController.Rigidbody.velocity = Vector3.zero;
                playerController.Rigidbody.AddForce(- a, ForceMode.VelocityChange);
                _animator.SetTrigger("Fail");
                return;
            }

            IsTriggered = true;
            IdleSound.SetParameter("Parameter 1", 0);
            IdleSound.Stop();
            OpenSound.Play();
            playerController.Life.LifeUnitsCount++;
            _animator.SetTrigger("Success");
        }
    }
}
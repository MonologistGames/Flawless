using System;
using Flawless.PlayerCharacter;
using UnityEngine;

namespace Flawless.Levels.Gates
{
    public class JumpGate : MonoBehaviour
    {
        public float JumpTimeFactor = 0.5f;
        public float JumpForce = 15f;
        public float JumpLapse = 0.5f;
        public float OverDriveTime = 2f;
        
        private PlanetController Player { get; set; }
        private float JumpTimer { get; set; }
        
        #region MonoBehaviours

        private void Update()
        {
            if (!Player) return;

            if (JumpTimer > 0)
            {
                JumpTimer -= Time.unscaledDeltaTime;
            }
            else
            {
                EndJump();
            }
        }

        #endregion
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            
            PlanetController planetController = other.gameObject.GetComponentInParent<PlanetController>();
            if (planetController == null) return;
            if (planetController.IsOverDriving)
            {
                BeginJump(planetController);
            }
        }

        private void BeginJump(PlanetController player)
        {
            Player = player;
            Player.Jump();
            Player.SetOverDrive(OverDriveTime);
            
            JumpTimer = JumpLapse;
            Time.timeScale *= JumpTimeFactor;
            Time.fixedDeltaTime = JumpTimeFactor * 0.02f;
        }
        
        private void EndJump()
        {
            Player.Rigidbody.AddForce(Player.MoveDir * JumpForce, ForceMode.Impulse);
            Player.EndJump();
            Player = null;
            
            JumpTimer = 0;
            Time.timeScale /= JumpTimeFactor;
            Time.fixedDeltaTime = 0.02f;
        }
    }
}


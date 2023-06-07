using Flawless.PlayerCharacter;
using UnityEngine;
using Utilities;

namespace Flawless.Levels.Gates
{
    public class JumpGate : MonoBehaviour
    {
        public float JumpTimeFactor = 0.1f;
        public float JumpForce = 15f;
        public float JumpLapse = 0.5f;
        public float OverDriveTime = 2f;
        public Animator Animator;
        public float ControlledForce = 20f;
        private static readonly int Launch = Animator.StringToHash("Launch");

        private PlayerController Player { get; set; }
        private Timer _jumpTimer;

        #region MonoBehaviours

        private void OnEnable()
        {
            _jumpTimer = TimerManager.Instance.AddTimer(JumpLapse, $"JumpTimer{this.gameObject.GetInstanceID()}",
                Timer.TimeType.Unscaled);
            _jumpTimer.IsPaused = true;
            _jumpTimer.OnTimerEnd += EndJump;
        }

        private void Update()
        {
            if (!Player) return;
        }

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            var planetController = other.gameObject.GetComponent<PlayerController>();
            if (planetController == null) return;
            
            if (planetController.IsOverDriving)
            {
                BeginJump(planetController);
            }
        }

        private void BeginJump(PlayerController player)
        {
            Player = player;
            Player.Jump();
            Player.Rigidbody.velocity =
                (transform.position - Player.transform.position).normalized * ControlledForce;

            Player.OverDriveTimer.SetTime(OverDriveTime);
            Player.IsOverDriving = true;

            _jumpTimer.ResetTime();
            _jumpTimer.IsPaused = false;
            
            Time.timeScale *= JumpTimeFactor;
            Time.fixedDeltaTime = JumpTimeFactor * 0.02f;
        }

        private void EndJump()
        {
            transform.forward = Player.MoveDir;
            Animator.SetTrigger(Launch);

            Player.Rigidbody.AddForce(Player.MoveDir * JumpForce - Player.Velocity, ForceMode.VelocityChange);
            Player.EndJump();
            Player = null;

            Time.timeScale /= JumpTimeFactor;
            Time.fixedDeltaTime = 0.02f;
        }
    }
}
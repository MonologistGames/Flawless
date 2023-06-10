using System;
using UnityEngine;

namespace Utilities
{
    public class Timer
    {
        public enum TimeType
        {
            Scaled,
            Unscaled
        }
        
        /// <summary>
        /// The count down type of the timer. Defines whether affects by Time.timeScale.
        /// </summary>
        public TimeType Type;
        
        /// <summary>
        /// Is the timer paused?
        /// </summary>
        public bool IsPaused;
        
        public float DesiredTime;
        private float _timeLeft;
        public float TimeLeft
        {
            get => _timeLeft;
            private set
            {
                if (value >= 0) _timeLeft = value;
                else _timeLeft = 0;
            }
            
        }
        public event Action OnTimerEnd;

        #region Constructors

        public Timer(float desiredTime)
        {
            DesiredTime = desiredTime;
            TimeLeft = desiredTime;
        }

        public Timer(float desiredTime, TimeType type) : this(desiredTime)
        {
            Type = type;
        }

        public Timer(TimeType type)
        {
            Type = type;
        }

        #endregion

        #region Callbacks
        /// <summary>
        /// Update the timer and check if it ends.
        /// </summary>
        public void Update()
        {
            if (IsPaused)
            {
                return;
            }
            // Check if timer ends.
            if (TimeLeft <= 0)
            {
                IsPaused = true;
                OnTimerEnd?.Invoke();
                return;
            }
            
            // Update left time.
            switch (Type)
            {
                case TimeType.Scaled:
                    TimeLeft -= Time.deltaTime;
                    break;
                case TimeType.Unscaled:
                    TimeLeft -= Time.unscaledDeltaTime;
                    break;
            }
        }


        #endregion
        
        #region APIs
        /// <summary>
        /// Reset the timer to original time value.
        /// </summary>
        public void ResetTime()
        {
            TimeLeft = DesiredTime;
        }
        
        /// <summary>
        /// Add Time to the timer.
        /// </summary>
        /// <param name="time">Time amount to add.</param>
        public void AddTime(float time)
        {
            TimeLeft += time;
        }
        
        /// <summary>
        /// Set timer directly to a time value.
        /// </summary>
        /// <param name="time">Time value to set.</param>
        public void SetTime(float time)
        {
            TimeLeft = time;
        }
        
        /// <summary>
        /// Get timer process. Returns how far the timer goes.
        /// </summary>
        /// <param name="isClamped">Is this process clamped to 0 to 1.</param>
        /// <returns>The process the timer goes.</returns>
        public float GetProcess(bool isClamped)
        {
            var process = (1 - TimeLeft / DesiredTime);
            if (isClamped) return Mathf.Clamp01(process);
            return process;
        }
        #endregion
    }
}
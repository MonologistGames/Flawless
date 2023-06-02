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

        public TimeType Type;

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

        public void Update()
        {
            if (IsPaused)
            {
                return;
            }
            
            if (TimeLeft <= 0)
            {
                IsPaused = true;
                OnTimerEnd?.Invoke();
                return;
            }
            
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
        
        public void ResetTime()
        {
            TimeLeft = DesiredTime;
        }
        
        public void AddTime(float time)
        {
            TimeLeft += time;
        }

        public void SetTime(float time)
        {
            TimeLeft = time;
        }

        public float GetProcess(bool isClamped)
        {
            var process = (1 - TimeLeft / DesiredTime);
            if (isClamped) return Mathf.Clamp01(process);
            return process;
        }
    }
}
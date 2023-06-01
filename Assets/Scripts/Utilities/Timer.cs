using System;
using UnityEngine;

namespace Utilities
{
    public class Timer
    {
        public enum TimeType
        {
            Scaled, Unscaled
        }
        
        public TimeType Type;
        public float DesiredTime;
        public float TimeLeft;
        public event Action OnTimerEnd;

        public Timer(float timeLeft)
        {
            TimeLeft = timeLeft;
        }
        
        public void Update()
        {
            switch (Type)
            {
                case TimeType.Scaled:
                    TimeLeft -= Time.deltaTime;
                    break;
                case TimeType.Unscaled:
                    TimeLeft -= Time.unscaledDeltaTime;
                    break;
            }

            if (TimeLeft <= 0)
            {
                OnTimerEnd?.Invoke();
            }
        }
    }
}
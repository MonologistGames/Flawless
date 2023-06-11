using System;
using System.Collections.Generic;
using Monologist.Patterns;
using Monologist.Patterns.Singleton;

namespace Utilities
{
    public class TimerManager : Singleton<TimerManager>
    {
        private Dictionary<string, Timer> _timers = new Dictionary<string, Timer>();

        #region MonoBehaviour Callbacks
        private void Update()
        {
            foreach (var pair in _timers)
            {
                pair.Value.Update();
            }
        }

        #endregion

        #region APIs
        public void AddTimer(Timer timer, string timerName)
        {
            this._timers.Add(timerName, timer);
        }

        public Timer AddTimer(float time, string timerName, Timer.TimeType timeType = Timer.TimeType.Scaled)
        {
            var timer = new Timer(time, timeType);
            AddTimer(timer, timerName);
            return timer;
        }
        
        public Timer AddTimer(string timerName, Timer.TimeType timeType = Timer.TimeType.Scaled)
        {
            var timer = new Timer(timeType);
            AddTimer(timer, timerName);
            return timer;
        }

        public bool GetTimer(string timerName, out Timer timer)
        {
            return _timers.TryGetValue(name, out timer);
        }
        #endregion
    }
}
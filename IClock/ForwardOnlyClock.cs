using System;

namespace IClock
{
    public class ForwardOnlyClock : IClock
    {
        private IClock _internalclock;
        private DateTimeOffset _lasttime;
        private object _lock = new object();

        public ForwardOnlyClock(IClock clock)
        {
            _internalclock = clock ?? throw new ArgumentNullException(nameof(clock));
            _lasttime = clock.GetTime();
        }

        public DateTimeOffset GetTime()
        {
            var current = _internalclock.GetTime();
            lock (_lock)
            {
                if (current.CompareTo(_lasttime) > 0)
                    _lasttime = current;
                return _lasttime;
            }
        }
    }
}
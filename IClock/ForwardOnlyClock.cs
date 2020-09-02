using System;

namespace IClock
{
    public class ForwardOnlyClock : ITimeProvider
    {
        private readonly ITimeProvider _internalclock;
        private DateTimeOffset _lasttime;
        private readonly object _lock = new object();

        public ForwardOnlyClock(ITimeProvider clock)
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
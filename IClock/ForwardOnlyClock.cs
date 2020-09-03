using System;

namespace IClock
{
    /// <summary>
    /// Represents a clock that only moves incremental time and never goes back (for example during DST changes).  
    /// </summary>
    /// <remarks>
    /// Note that when the given internal clock goes back in time, for example during DST changes, time will apear
    /// to stand still during that period until time "catches up" to the time where the clock was changed.
    /// </remarks>
    public class ForwardOnlyClock : ITimeProvider
    {
        private readonly ITimeProvider _timeprovider;
        private DateTimeOffset _lasttime;
        private readonly object _lock = new object();

        /// <summary>
        /// Initializes the <see cref="ForwardOnlyClock"/> with a given <see cref="ITimeProvider"/>
        /// </summary>
        /// <param name="timeProvider">The actual <see cref="ITimeProvider"/> to provide the actual time for this clock.</param>
        public ForwardOnlyClock(ITimeProvider timeProvider)
        {
            _timeprovider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
            _lasttime = timeProvider.GetTime();
        }

        /// <summary>
        /// Returns the (date)time from the <see cref="ITimeProvider"/> given in the constructor.
        /// </summary>
        /// <returns>Returns the (date)time from the given <see cref="ITimeProvider"/>.</returns>
        /// <remarks>
        /// Note that when the clock goes back, for example during DST changes, time will apear to stand still during
        /// that period until time "catches up" to the time where the clock was changed.
        /// </remarks>
        /// <threadsafety static="true" instance="true"/>
        public DateTimeOffset GetTime()
        {
            var current = _timeprovider.GetTime();
            lock (_lock)
            {
                if (current.CompareTo(_lasttime) > 0)
                    _lasttime = current;
                return _lasttime;
            }
        }
    }
}
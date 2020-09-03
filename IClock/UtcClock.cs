using System;

namespace IClock
{
    /// <summary>
    /// Represents a clock that provides UTC (date)time.
    /// </summary>
    public class UTCClock : ITimeProvider
    {
        /// <summary>
        /// Returns the current UTC (date)time.
        /// </summary>
        /// <returns>Returns the current UTC (date)time.</returns>
        public DateTimeOffset GetTime() => DateTimeOffset.UtcNow;
    }
}
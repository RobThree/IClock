using System;

namespace IClock
{
    /// <summary>
    /// Represents a clock that provides local (date)time.
    /// </summary>
    public class LocalClock : ITimeProvider
    {
        /// <summary>
        /// Returns the current local (date)time.
        /// </summary>
        /// <returns>Returns the current local (date)time.</returns>
        public DateTimeOffset GetTime() => DateTimeOffset.Now;
    }
}
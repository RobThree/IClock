using System;

namespace IClock
{
    /// <summary>
    /// Defines a method to get (date)time.
    /// </summary>
    public interface ITimeProvider
    {
        /// <summary>
        /// Returns the providers' (date)time.
        /// </summary>
        /// <returns>The providers' (date)time.</returns>
        DateTimeOffset GetTime();
    }
}
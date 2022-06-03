using System;

namespace IClock
{
    /// <summary>
    /// Provides a simple implementation to wrap any (date)time function into an <see cref="ITimeProvider"/>.
    /// </summary>
    public class CustomClock : ITimeProvider
    {
        private readonly Func<DateTimeOffset> _gettimefunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomClock"/> class with a specific time function.
        /// </summary>
        /// <param name="getTimeFunction">The function to get the (date)time.</param>
        public CustomClock(Func<DateTimeOffset> getTimeFunction)
            => _gettimefunc = getTimeFunction ?? throw new ArgumentNullException(nameof(getTimeFunction));

        /// <summary>
        /// Returns the (date)time from the given function.
        /// </summary>
        /// <returns>Returns the (date)time from the given function.</returns>
        public DateTimeOffset GetTime()
            => _gettimefunc();
    }
}
using System;

namespace IClock
{
    /// <summary>
    /// Represents a clock that keeps the same time during it's lifetime as if time stands still.
    /// </summary>
    /// <remarks>
    /// This <see cref="ITimeProvider"/> can be used as a clock that doesn't change time during it's lifetime. This
    /// can be handy in situations where you need a single point in time during the handling of a request for example.
    /// </remarks>
    public class ScopedClock : IScopedTimeProvider, IDisposable
    {
        private readonly DateTimeOffset _time;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of a <see cref="ScopedClock"/> that 'freezes time'.
        /// </summary>
        /// <param name="timeProvider">The <see cref="ITimeProvider"/> the get the (date)time from.</param>
        /// <remarks>
        /// The (date)time this provider will return will be a 'snapshot' of the (date)time at the time this
        /// constructor is called.
        /// </remarks>
        public ScopedClock(ITimeProvider timeProvider)
            : this((timeProvider ?? throw new ArgumentNullException(nameof(timeProvider))).GetTime()) { }


        /// <summary>
        /// Initializes a new instance of a <see cref="ScopedClock"/> that 'freezes time'.
        /// </summary>
        /// <param name="getTimeFunction">The function to get the (date)time.</param>
        /// <remarks>
        /// The (date)time this provider will return will be a 'snapshot' of the (date)time at the time this
        /// constructor is called.
        /// </remarks>
        public ScopedClock(Func<DateTimeOffset> getTimeFunction)
            : this((getTimeFunction ?? throw new ArgumentNullException(nameof(getTimeFunction))).Invoke()) { }

        /// <summary>
        /// Initializes a new instance of a <see cref="ScopedClock"/> that 'freezes time'.
        /// </summary>
        /// <param name="time">The (date)time this <see cref="ITimeProvider"/> will return.</param>
        /// <remarks>
        /// The (date)time this provider will return will be a 'snapshot' of the (date)time at the time this
        /// constructor is called.
        /// </remarks>
        public ScopedClock(DateTimeOffset time)
        {
            _time = time;
        }

        /// <summary>
        /// Returns the 'frozen' (date)time.
        /// </summary>
        /// <returns>Returns the 'frozen' (date)time.</returns>
        public DateTimeOffset GetTime() => _time;

        #region IDisposable
        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="ScopedClock"/> object, and optionally disposes of the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to releases only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                _disposed = true;
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="ScopedClock"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
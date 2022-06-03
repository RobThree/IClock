using System;
using System.Runtime.CompilerServices;

namespace IClock
{
    /// <summary>
    /// An <see cref="ITimeProvider"/> to be used in unittests. This clock can be set to any (date)time at any
    /// given moment for testing purposes. It also provides a means to advance time (in both directions) using
    /// the <see cref="Tick"/> method for convenience.
    /// </summary>
    /// <threadsafety static="true" instance="true"/>
    public class TestClock : ITimeProvider
    {
        private DateTimeOffset _datetime;
        private TimeSpan _increment;
        private readonly object _lock = new();

        /// <summary>
        /// The default offset for the <see cref="DefaultTime"/>.
        /// </summary>
        public static TimeSpan DefaultOffset { get; } = TimeSpan.FromHours(6);

        /// <summary>
        /// The default (date)time at which the <see cref="TestClock"/> is initialized unless specified otherwise.
        /// </summary>
        public static DateTimeOffset DefaultTime { get; } = new DateTimeOffset(2013, 12, 11, 10, 9, 8, 7, DefaultOffset);

        /// <summary>
        /// The default increment which the <see cref="TestClock"/> uses when the <see cref="Tick"/> method is called..
        /// </summary>
        public static TimeSpan DefaultIncrement { get; } = TimeSpan.FromSeconds(1);

        /// <summary>
        /// Initializes a new instance of the <see cref="TestClock"/> class with the <see cref="DefaultTime"/>
        /// and <see cref="DefaultIncrement"/>.
        /// </summary>
        public TestClock()
            : this(DefaultTime, DefaultIncrement) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestClock"/> class with the given (date)time
        /// and <see cref="DefaultIncrement"/>.
        /// </summary>
        /// <param name="dateTime">The (date)time for the clock.</param>
        public TestClock(DateTimeOffset dateTime)
            : this(dateTime, DefaultIncrement) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestClock"/> class with the <see cref="DefaultTime"/>
        /// and given increment.
        /// </summary>
        /// <param name="increment">The increment to be used when <see cref="Tick"/> is called.</param>
        public TestClock(TimeSpan increment)
            : this(DefaultTime, increment) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestClock"/> class with the given (date)time
        /// and given increment.
        /// </summary>
        /// <param name="dateTime">The (date)time for the clock.</param>
        /// <param name="increment">The increment to be used when <see cref="Tick"/> is called.</param>
        public TestClock(DateTimeOffset dateTime, TimeSpan increment)
        {
            _datetime = dateTime;
            _increment = increment;
        }

        /// <summary>
        /// Returns the (date)time.
        /// </summary>
        /// <returns>Returns the (date)time.</returns>
        public DateTimeOffset GetTime() => _datetime;

        /// <summary>
        /// Sets the (date)time.
        /// </summary>
        /// <param name="dateTime">The (date)time to set the clock to.</param>
        public void Set(DateTimeOffset dateTime) => _datetime = dateTime;

        /// <summary>
        /// Adds a given time to the clock's current (date)time.
        /// </summary>
        /// <param name="timeSpan">The time to add.</param>
        /// <returns>Returns the new (date)time.</returns>
        public DateTimeOffset Adjust(TimeSpan timeSpan)
        {
            lock (_lock)
            {
                return _datetime = _datetime.Add(timeSpan);
            }
        }

        /// <summary>
        /// Adds the clock's increment to the clock's current time.
        /// </summary>
        public void Tick() => Adjust(_increment);

        /// <summary>
        /// Returns a deterministic (pseudo)random (date)time based on the CallerMemberName with a zero offset (UTC).
        /// </summary>
        /// <param name="name">The name of the calling method or any string for a repeatable result.</param>
        /// <returns>A deterministic (pseudo)random (date)time based on the CallerMemberName.</returns>
        public static DateTimeOffset GetDeterministicRandomTime([CallerMemberName] string? name = null)
            => GetDeterministicRandomTime(TimeSpan.Zero, name);

        /// <summary>
        /// Returns a deterministic (pseudo)random (date)time based on the CallerMemberName with the given offset.
        /// </summary>
        /// <param name="offset">The offset to be returned for the resulting (date)time.</param>
        /// <param name="name">The name of the calling method or any string for a repeatable result.</param>
        /// <returns>A deterministic (pseudo)random (date)time based on the CallerMemberName with the given offset.</returns>
        public static DateTimeOffset GetDeterministicRandomTime(TimeSpan offset, [CallerMemberName] string? name = null)
        {
            name ??= string.Empty;
            return new DateTimeOffset(DefaultTime.AddSeconds(GetHashCode(name)).AddTicks(GetHashCode(name)).DateTime, offset);
        }

        private static int GetHashCode(string value)
        {
            var hashCode = 411826868;
            unchecked
            {
                for (var i = 0; i < value.Length; i++)
                {
                    hashCode *= -1521134295 + value[i];
                }
            }
            return hashCode;
        }
    }
}
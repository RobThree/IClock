using System;
using System.Runtime.CompilerServices;

namespace IClock
{
    public class TestClock : ITimeProvider
    {
        private DateTimeOffset _time;
        private TimeSpan _defaultincrement;
        private readonly object _lock = new object();

        public static DateTimeOffset DefaultTime { get; } = new DateTimeOffset(2013, 12, 11, 10, 9, 8, 7, TimeSpan.FromHours(6));

        public TestClock(TimeSpan? defaultIncrement = null)
            : this(DefaultTime, defaultIncrement) { }

        public TestClock(DateTimeOffset current, TimeSpan? defaultIncrement = null)
        {
            _time = current;
            _defaultincrement = defaultIncrement ?? TimeSpan.FromSeconds(1);
        }

        public DateTimeOffset GetTime() => _time;

        public void Set(DateTimeOffset dateTimeOffset) => _time = dateTimeOffset;

        public void Add(TimeSpan timeSpan)
        {
            lock (_lock)
                _time = _time.Add(timeSpan);
        }

        public void Tick() => Add(_defaultincrement);


        private static int GetHashCode(string value)
        {
            var hashCode = 411826868;
            unchecked
            {
                for (var i = 0; i < value.Length; i++)
                    hashCode *= -1521134295 + value[i];
            }
            return hashCode;
        }


        public static DateTimeOffset GetDeterministicRandomTime(TimeSpan? offset = null, [CallerMemberName] string name = null)
        {
            name = name ?? string.Empty;
            return new DateTimeOffset(DefaultTime.AddSeconds(GetHashCode(name)).AddTicks(GetHashCode(name)).DateTime, offset ?? TimeSpan.Zero);
        }
    }
}
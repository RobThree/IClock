using System;

namespace IClock
{
    public class CustomClock : IClock
    {
        private readonly Func<DateTimeOffset> _gettimefunc;
        public CustomClock(Func<DateTimeOffset> getTimeFunction)
        {
            _gettimefunc = getTimeFunction ?? throw new ArgumentNullException(nameof(getTimeFunction));
        }

        public DateTimeOffset GetTime() => _gettimefunc();
    }
}
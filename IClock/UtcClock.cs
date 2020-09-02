using System;

namespace IClock
{
    public class UTCClock : ITimeProvider
    {
        public DateTimeOffset GetTime() => DateTimeOffset.UtcNow;
    }
}
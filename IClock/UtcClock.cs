using System;

namespace IClock
{
    public class UTCClock : IClock
    {
        public DateTimeOffset GetTime() => DateTimeOffset.UtcNow;
    }
}
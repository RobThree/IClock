using System;

namespace IClock
{
    public class LocalClock : ITimeProvider
    {
        public DateTimeOffset GetTime() => DateTimeOffset.Now;
    }
}
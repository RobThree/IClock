using System;

namespace IClock
{
    public class LocalClock : IClock
    {
        public DateTimeOffset GetTime() => DateTimeOffset.Now;
    }
}
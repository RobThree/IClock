using System;

namespace IClock
{
    public interface ITimeProvider
    {
        DateTimeOffset GetTime();
    }
}
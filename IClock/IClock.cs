using System;

namespace IClock
{
    public interface IClock
    {
        DateTimeOffset GetTime();
    }
}
using System;

namespace IClock;

/// <summary>
/// Defines a method to get (date)time.
/// </summary>
[Obsolete("Use System.TimeProvider instead (https://learn.microsoft.com/en-us/dotnet/api/system.timeprovider)")]
public interface ITimeProvider
{
    /// <summary>
    /// Returns the providers' (date)time.
    /// </summary>
    /// <returns>The providers' (date)time.</returns>
    DateTimeOffset GetTime();
}
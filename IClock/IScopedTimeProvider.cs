namespace IClock
{
    /// <summary>
    /// Interface to specifically resolve scoped clocks using dependency injection frameworks.
    /// </summary>
    public interface IScopedTimeProvider : ITimeProvider { }
}
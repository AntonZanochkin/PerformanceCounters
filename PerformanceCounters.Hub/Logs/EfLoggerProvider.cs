namespace PerformanceCounters.Hub.Logs
{
  public class EfLoggerProvider : ILoggerProvider
  {
    public ILogger CreateLogger(string categoryName)
    {
      return new EfLogger();
    }

    public void Dispose() { }
  }
}

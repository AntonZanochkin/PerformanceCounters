namespace PerformanceCounters.Hub.Logs
{
  public class EfLogger : ILogger
  {
    public IDisposable BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
      if (logLevel == LogLevel.Information)
      {
        // Log the response from the database
        Console.WriteLine($"EF Query Response: {formatter(state, exception)}");

        if (state is IEnumerable<KeyValuePair<string, object>> logData)
        {
          // Extract the result from the log data
          var result = logData.FirstOrDefault(kv => kv.Key == "Result");
          if (result.Value != null)
          {
            Console.WriteLine($"EF Query Result: {result.Value}");
          }
        }
      }
    }
  }
}

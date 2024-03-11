using System.Collections.Concurrent;

namespace PerformanceCounters.Hub.Services.Cache
{
  public class ConnectionSubscribe
  {
    public int DeviceId;
    public int ProcessId;
    public CounterType CounterType;
    public ConcurrentDictionary<string, int> CounterRevisionByName = new();
  }
}

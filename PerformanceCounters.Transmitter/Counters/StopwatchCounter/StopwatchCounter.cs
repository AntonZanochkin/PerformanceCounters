using System;
using System.Diagnostics;

namespace PerformanceCounters.Transmitter.Counters.StopwatchCounter
{
  public class StopwatchCounter : IDisposable
  {
    private readonly string _name;
    private readonly Stopwatch _stopwatch;

    public StopwatchCounter(string name)
    {
      _name = name;
      _stopwatch = Stopwatch.StartNew();
    }

    public void Dispose()
    {
      _stopwatch.Stop();
      var namedStorage = StorageService.StopwatchStorage.GetOrCreateNamedStorage(_name);
      namedStorage.AddTicks(_stopwatch.Elapsed.Ticks);
    }
  }
}

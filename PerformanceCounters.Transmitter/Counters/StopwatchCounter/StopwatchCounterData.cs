using Newtonsoft.Json;
using System.Threading;

namespace PerformanceCounters.Transmitter.Counters.StopwatchCounter
{
  public class StopwatchCounterData : ICounterData
  {
    private long _ticks;
    public string JsonValue => JsonConvert.SerializeObject(new { value = GetSeconds()});
    public long AddTicks(long ticks)
    {
      return Interlocked.Add(ref _ticks, ticks);
    }
    public double GetSeconds()
    {
      var ticks = Interlocked.Read(ref _ticks);
      return ticks / 10000000d;
    }
  }
}
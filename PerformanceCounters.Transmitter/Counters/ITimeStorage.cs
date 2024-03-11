using System;

namespace PerformanceCounters.Transmitter.Counters
{
  public interface ITimeStorage
  {
    CounterType Type { get; }
    void DeleteCountersUpToTime(DateTime endTime);
  }
}

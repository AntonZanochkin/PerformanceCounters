using System.Threading;

namespace PerformanceCounters.Transmitter.Counters.CpuTimeCounter
{
  public class CounterMetric
  {
    public long KernelTicks;
    public long UserTicks;
    public long DurationTicks;

    public void UpdateMetric(CpuUsage cpuUsage, long durationTicks)
    {
      KernelTicks = Interlocked.Add(ref KernelTicks, cpuUsage.KernelUsage);
      UserTicks = Interlocked.Add(ref UserTicks, cpuUsage.UserUsage);
      DurationTicks = Interlocked.Add(ref DurationTicks, durationTicks);
    }
  }
}

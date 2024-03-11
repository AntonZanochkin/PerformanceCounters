using System.Diagnostics;
using System.Threading;

namespace PerformanceCounters.Transmitter.Counters.CpuTimeCounter
{
  public class ThreadMetric
  {
    public long OwnerObjectId;
    public int ManagedThreadId;
    public CpuUsage CpuUsage;
    public Stopwatch StartAt;

    public ThreadMetric(long ownerObjectId)
    {
      ManagedThreadId = Thread.CurrentThread.ManagedThreadId;
      CpuUsage = CpuUsage.GetByThread();
      StartAt = Stopwatch.StartNew();
      OwnerObjectId = ownerObjectId;
    }
  }
}

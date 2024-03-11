using System;
using System.Diagnostics;

namespace PerformanceCounters.Transmitter.Counters.CpuTimeCounter
{
  internal class AsyncLocalValueChangedObj
  {
    public IntPtr HThread;
    public AsyncLocalState State;
    public CpuUsage? CpuUsage;
    public Stopwatch StartAt;
    public int ManagedThreadId;

    public AsyncLocalValueChangedObj Clone()
    {
      return new AsyncLocalValueChangedObj
      {
        HThread = HThread,
        State = State,
        CpuUsage = CpuUsage,
        StartAt = StartAt,
        ManagedThreadId = ManagedThreadId
      };
    }
  }
}
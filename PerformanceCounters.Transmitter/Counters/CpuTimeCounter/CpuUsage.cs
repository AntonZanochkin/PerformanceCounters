using System;
using System.Runtime.InteropServices;

namespace PerformanceCounters.Transmitter.Counters.CpuTimeCounter
{
  public struct CpuUsage
  {
    public long KernelUsage;
    public long UserUsage;

    public CpuUsage(long kernelUsage, long userUsage)
    {
      KernelUsage = kernelUsage;
      UserUsage = userUsage;
    }
    
    public static CpuUsage GetByThread()
    {
      var threadHandle = WindowsCpuUsageInterop.GetCurrentThread();
      if (!WindowsCpuUsageInterop.GetThreadTimes(threadHandle, out var creationTime, out var exitTime, out var kernelStartTime, out var userStartTime))
      {
        throw new InvalidOperationException($"GetThreadTimes failed with error code {Marshal.GetLastWin32Error()}");
      }

      return new CpuUsage(kernelStartTime, userStartTime);
    }

    public static CpuUsage Subtract(CpuUsage onEnd, CpuUsage onStart)
    {
      var kernelUsage = onEnd.KernelUsage - onStart.KernelUsage;
      var userUsage = onEnd.UserUsage - onStart.UserUsage;
      return new CpuUsage(kernelUsage, userUsage);
    }
  }
}

using System.Threading;
using Newtonsoft.Json;

namespace PerformanceCounters.Transmitter.Counters.CpuTimeCounter
{
  public class CpuTimeCounterData : ICounterData
  {
    private static readonly long Frequency;

    private long _kernelTicks;
    private long _userTicks;
    private long _durationTicks;
    private long _sleepTicks;

    public double KernelTimeSeconds() => Interlocked.Read(ref _kernelTicks) / (double)Frequency;
    public double UserTimeSeconds() => Interlocked.Read(ref _userTicks) / (double)Frequency;
    public double DurationTimeSeconds() => Interlocked.Read(ref _durationTicks) / (double)Frequency;
    public double SleepTimeSeconds() => Interlocked.Read(ref _sleepTicks) / (double)Frequency;

    public string JsonValue => JsonConvert.SerializeObject(new
    {
      kernelTime = KernelTimeSeconds(), 
      userTime = UserTimeSeconds(),
      durationTime = DurationTimeSeconds(),
      sleepTime = SleepTimeSeconds(),
    });

    static CpuTimeCounterData()
    {
      WindowsCpuUsageInterop.QueryPerformanceFrequency(out Frequency);
    }

    public void AddCounterMetric(CounterMetric metric, long durationTicks)
    {
      Interlocked.Add(ref _kernelTicks, metric.KernelTicks);
      Interlocked.Add(ref _userTicks, metric.UserTicks);
      Interlocked.Add(ref _durationTicks, metric.DurationTicks);
      Interlocked.Add(ref _sleepTicks, durationTicks - metric.KernelTicks - metric.UserTicks);
    }
  }
}
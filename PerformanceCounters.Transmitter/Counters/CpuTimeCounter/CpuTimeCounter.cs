using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace PerformanceCounters.Transmitter.Counters.CpuTimeCounter
{
  public class CpuTimeCounter : IDisposable
  {
    private static readonly ThreadLocal<List<ThreadMetric>> ThreadMetrics = new ThreadLocal<List<ThreadMetric>>();
    private static long _lastCounterObjectId;
    private readonly AsyncLocal<AsyncLocalValueChangedObj> _contextSwitchListener;
    private readonly string _name;
    public CounterMetric Metric { get; } = new CounterMetric();
    public Stopwatch Duration { get; }
    private volatile bool _isDisposed;
    private readonly long _objectId = Interlocked.Increment(ref _lastCounterObjectId);

    public CpuTimeCounter(string name)
    {
      _name = name;
     
      Duration = Stopwatch.StartNew();
      _contextSwitchListener = new AsyncLocal<AsyncLocalValueChangedObj>(ContextChangedHandler)
      {
        Value = new AsyncLocalValueChangedObj
        {
          State = AsyncLocalState.Constructor
        }
      };

      if (ThreadMetrics.Value == null)
        ThreadMetrics.Value = new List<ThreadMetric>();

      var threadMetric = new ThreadMetric(_objectId);
      ThreadMetrics.Value.Add(threadMetric);
    }

    private void ContextChangedHandler(AsyncLocalValueChangedArgs<AsyncLocalValueChangedObj> args)
    {
      if (_isDisposed || !args.ThreadContextChanged)
        return;

      ProcessThreadMetric();
    }

    private void ProcessThreadMetric()
    {
      if (ThreadMetrics.Value == null)
        ThreadMetrics.Value = new List<ThreadMetric>();

      var threadMetric = ThreadMetrics.Value.FirstOrDefault(x => x.OwnerObjectId == _objectId);
      if (threadMetric == null)//Continuation
      {
        threadMetric = new ThreadMetric(_objectId);
        ThreadMetrics.Value.Add(threadMetric);
      }
      else
      {
        RecordThreadMetric(threadMetric);
        ThreadMetrics.Value.Remove(threadMetric);
      }
    }

    private void RecordThreadMetric(ThreadMetric threadMetric)
    {
      var currentCpuUsage = CpuUsage.GetByThread();
      var cpuUsage = CpuUsage.Subtract(currentCpuUsage, threadMetric.CpuUsage);
      Metric.UpdateMetric(cpuUsage, threadMetric.StartAt.ElapsedTicks);
    }

    public void Dispose()
    {
      _isDisposed = true;

      var threadMetric = ThreadMetrics.Value.FirstOrDefault(x => x?.OwnerObjectId == _objectId);
      if (threadMetric == null)
        throw new InvalidOperationException("Thread metric not found during disposal.");

      Duration.Stop();

      RecordThreadMetric(threadMetric);
      ThreadMetrics.Value.Remove(threadMetric);

      var namedStorage = StorageService.CpuTimeStorage.GetOrCreateNamedStorage(_name);
      namedStorage.AddCounterMetric(Metric, Duration.ElapsedTicks);
    }
  }
}
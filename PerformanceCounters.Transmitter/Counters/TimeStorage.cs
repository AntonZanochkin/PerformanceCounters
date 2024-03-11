using PerformanceCounters.Transmitter.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace PerformanceCounters.Transmitter.Counters
{
    public class TimeStorage<T> : ITimeStorage where T: ICounterData, new()
  {
    public CounterType Type { get; }

    private ConcurrentDictionary<DateTime, NamedStorage<T>> Storage { get; } = new ConcurrentDictionary<DateTime, NamedStorage<T>>();
    public TimeStorage(CounterType type)
    {
      Type = type;
    }
    private NamedStorage<T> GetOrCreateTimeStorage()
    {
      var roundedTime = DateTime.UtcNow.RoundToMinute();
      return Storage.GetOrAdd(roundedTime, time => new NamedStorage<T>());
    }
    public T GetOrCreateNamedStorage(string name)
    {
      var timeStorage = GetOrCreateTimeStorage();
      var namedStorage = timeStorage.GetOrCreateNamedStorage(name);
      return namedStorage;
    }

    private IEnumerable<KeyValuePair<DateTime, NamedStorage<T>>> SelectNamedStorageUpToTime(DateTime endTime)
    {
      return Storage.Where(s => s.Key <= endTime);
    }

    public void EnumerateCounterDataUpToTime(DateTime endTime, Action<DateTime, string, ICounterData> counterDataCallBack)
    {
      var finalized = SelectNamedStorageUpToTime(endTime);

      foreach (var kVp in finalized)
      {
        var time = kVp.Key;
        var timeStorage = kVp.Value;

        foreach (var namedKvP in timeStorage.GetCounterData())
        {
          var name = namedKvP.Key;
          var iCounterData = namedKvP.Value;
          counterDataCallBack(time, name, iCounterData);
        }
      }
    }

    public void DeleteCountersUpToTime(DateTime endTime)
    {
      Storage
        .Where(kVp => kVp.Key <= endTime)
        .Select(kVp => kVp.Key)
        .ToList()
        .ForEach(time=> Storage.TryRemove(time, out _));
    }
  }
}

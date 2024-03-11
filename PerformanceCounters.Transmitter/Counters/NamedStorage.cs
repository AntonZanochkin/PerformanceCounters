using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace PerformanceCounters.Transmitter.Counters
{
  public class NamedStorage<T> where T : ICounterData, new()
  {
    private ConcurrentDictionary<string, T> Storage { get; } = new ConcurrentDictionary<string, T>();

    public T GetOrCreateNamedStorage(string name)
    {
      return Storage.GetOrAdd(name, _ => new T());
    }

    public Dictionary<string, ICounterData> GetCounterData()
    {
      return Storage.ToDictionary(kVp => kVp.Key, kVp => (ICounterData)kVp.Value);
    }
  }
}

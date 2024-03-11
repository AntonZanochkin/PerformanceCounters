using System.Collections.Concurrent;
using PerformanceCounters.Hub.Dto.Process;
using PerformanceCounters.Hub.Utilities;

namespace PerformanceCounters.Hub.Services.Cache
{
  public class ProcessCacheItem
  {
    public int Id;
    public string Name;
    public ConcurrentDictionary<CounterType, ConcurrentHashSet<string>> CounterNamesByType = new();

    public ProcessDto BuildDto()
    {
      return new ProcessDto
      {
        Id = Id,
        Name = Name,
        CounterNamesByType = new Dictionary<string, List<string>>(CounterNamesByType.ToDictionary(kVp => kVp.Key.ToString(), kVp => kVp.Value.ToList()))
      };
    }
  }
}

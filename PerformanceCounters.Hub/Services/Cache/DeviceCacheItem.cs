using System.Collections.Concurrent;

namespace PerformanceCounters.Hub.Services.Cache
{
  public class DeviceCacheItem
  {
    public int Id;
    public string Name;
    public ConcurrentDictionary<int, ProcessCacheItem> Processes = new();
  }
}

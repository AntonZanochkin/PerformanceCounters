using System.Collections.Concurrent;
using PerformanceCounters.Hub.EF;
using PerformanceCounters.Hub.Services.Cache;
using PerformanceCounters.Hub.Utilities;

namespace PerformanceCounters.Hub.Services
{
  public class DbCacheService
  {
    private static readonly ConcurrentDictionary<int, DeviceCacheItem> DeviceCacheItems = new();
    private static bool _isInit;

    public DbCacheService(CountersDbContext dbContext)
    {
      if (!_isInit)
      {
        lock (DeviceCacheItems)
        {
          if (!_isInit)
            Init(dbContext);

          _isInit = true;
        }
      }
    }

    private void Init(CountersDbContext dbContext)
    {
      var result = dbContext.Device
        .Select(m => new
        {
          Id = m.Id,
          Name = m.Name,
          Processes = m.Processes.Select(p => new
          {
            Id = p.Id,
            Name = p.Name,
            CounterNamesByType = p.Counters.GroupBy(x => x.Type)
              .Select(g => new { type = g.Key, names = g.Select(x => x.Name).Distinct() }),
          }).ToList(),
        }).ToList();

      result.ForEach(d =>
      {
        var processes = new ConcurrentDictionary<int, ProcessCacheItem>();
        d.Processes.ForEach(p =>
        {
          var counterNamesByType = new ConcurrentDictionary<CounterType, ConcurrentHashSet<string>>();
          foreach (var x1 in p.CounterNamesByType)
            counterNamesByType.TryAdd(x1.type, new ConcurrentHashSet<string>(x1.names));

          processes.TryAdd(p.Id, new ProcessCacheItem
          {
            Id = p.Id,
            Name = p.Name,
            CounterNamesByType = counterNamesByType,
          });
        });

        DeviceCacheItems.TryAdd(d.Id, new DeviceCacheItem
        {
          Id = d.Id,
          Name = d.Name,
          Processes = processes,
        });
      });
    }

    public bool TryAddDevice(int deviceId, string deviceName)
    {
      return DeviceCacheItems.TryAdd(deviceId, new DeviceCacheItem
      {
        Id = deviceId,
        Name = deviceName
      });
    }

    public bool TryAddProcess(int deviceId, int processId, string processName)
    {
      if (DeviceCacheItems.TryGetValue(deviceId, out var deviceCacheItem))
      {
        return deviceCacheItem.Processes.TryAdd(processId, new ProcessCacheItem
        {
          Id = processId,
          Name = processName,
        });
      }

      return false;
    }

    public ConcurrentDictionary<int, DeviceCacheItem> GetDevices()
    {
      return DeviceCacheItems;
    }

    public List<string>? TryAddCounterNames(int deviceId, int processId, CounterType type, string[] counterNames)
    {
      if (!DeviceCacheItems.TryGetValue(deviceId, out var deviceCacheItem))
        return null;

      if (!deviceCacheItem.Processes.TryGetValue(processId, out var processCacheItem))
        return null;

      var newNames = new ConcurrentHashSet<string>();

      processCacheItem.CounterNamesByType.AddOrUpdate(type, counterType =>
      {
        newNames.AddRange(counterNames);
        return new ConcurrentHashSet<string>(counterNames);
      }, (counterType, hashSet) =>
      {
        foreach (var counterName in counterNames)
        {
          if (hashSet.Add(counterName))
            newNames.Add(counterName);
        }

        return hashSet;
      });

      return newNames.ToList();
    }
  }
}
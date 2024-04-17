using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using PerformanceCounters.Hub.Dto.Counter;
using PerformanceCounters.Hub.EF.Entity;
using PerformanceCounters.Hub.Services.Cache;
using PerformanceCounters.Hub.SignalR.Hubs;

namespace PerformanceCounters.Hub.Services.SignalR
{
  public class CounterSignalService
  {
    private readonly IHubContext<ClientHub> _hubContext;
    private static readonly ConcurrentDictionary<string, ConnectionSubscribe> SubscribesByConnection = new();

    public CounterSignalService(IHubContext<ClientHub> hubContext)
    {
      _hubContext = hubContext;
    }

    public ConnectionSubscribe SubscribeConnection(string connectionId, int deviceId, int processId, CounterType counterType,
      string counterName, int revision)
    {
      return SubscribesByConnection.AddOrUpdate(connectionId,
        _ => new ConnectionSubscribe
        {
          DeviceId = deviceId,
          ProcessId = processId,
          CounterType = counterType,
          CounterRevisionByName = new ConcurrentDictionary<string, int>(new List<KeyValuePair<string, int>> { new(counterName, revision) })
        }, (_, subscribe) =>
        {
          subscribe.DeviceId = deviceId;
          subscribe.ProcessId = processId;
          subscribe.CounterType = counterType;
          subscribe.CounterRevisionByName.TryAdd(counterName, revision);
          return subscribe;
        }
      );
    }

    public void UnsubscribeConnection(string connectionId, int deviceId, int processId, CounterType counterType,
      string counterName)
    {
      if(!SubscribesByConnection.TryGetValue(connectionId, out var connectionSubscribe))
        return;

      if(connectionSubscribe.DeviceId != deviceId || connectionSubscribe.ProcessId != processId)
        return;

      connectionSubscribe.CounterRevisionByName.TryRemove(counterName, out var revision);
    }

    public void UpdateSubscribeRevision(ConnectionSubscribe connectionSubscribe, List<UpdateCounterDto> updateDtoList)
    {
      foreach (var groupByName in updateDtoList.GroupBy(x => x.Name))
      {
        var counterName = groupByName.Key;
        var lastRevision = groupByName.Max(x => x.Id);
        connectionSubscribe.CounterRevisionByName[counterName] = lastRevision;
      }
    }

    public async Task SendCountersToSubscribers(int deviceId, int processId, List<IGrouping<CounterType, CounterEntity>> groupEntityListByCounterType)
    {
      foreach (var kVp in SubscribesByConnection)
      {
        var connectionId = kVp.Key;
        var subscribe = kVp.Value;
        
        if (subscribe.DeviceId != deviceId || subscribe.ProcessId != processId) continue;

        foreach (var entityGroup in groupEntityListByCounterType)
        {
          var entityCounterType = entityGroup.Key;
          if(entityCounterType != subscribe.CounterType) continue;

          foreach (var counterEntity in entityGroup)
          {
            if(!subscribe.CounterRevisionByName.TryGetValue(counterEntity.Name, out var revision))
              continue;

            var addCounterDto = UpdateCounterDto.Create(counterEntity);

            await _hubContext.Clients.Client(connectionId).SendAsync("updateCounters", new UpdateCountersDto()
            {
              DeviceId = deviceId,
              ProcessId = processId,
              Counters = new List<UpdateCounterDto> { addCounterDto }
            });
            
            subscribe.CounterRevisionByName.AddOrUpdate(counterEntity.Name, 
              _ => addCounterDto.Id,
              (_, _) => addCounterDto.Id);
          }
        }
      }
    }
  }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PerformanceCounters.Hub.Services;
using PerformanceCounters.Hub.Services.SignalR;

namespace PerformanceCounters.Hub.SignalR.Hubs
{
  public class ClientHub : Microsoft.AspNetCore.SignalR.Hub
  {
    public async Task GetDevices([FromServices] DeviceService deviceService)
    {
      var dto = deviceService.BuildSetDevicesDto();
      await Clients.Caller.SendAsync("SetDevices", dto);
    }

    public async Task SubscribeCounter([FromServices] CounterService counterService, [FromServices] CounterSignalService counterSignalService, int deviceId, int processId, string counterTypeString, string counterName, int revision)
    {
      if (!Enum.TryParse<CounterType>(counterTypeString, out var counterType))
        throw new ArgumentException(counterTypeString);
      
      var userCounterSubscribe = counterSignalService.SubscribeConnection(Context.ConnectionId, deviceId, processId, counterType, counterName, revision);
      var updateDtoList = await counterService.LoadUpdateDtoAsync(userCounterSubscribe);
      counterSignalService.UpdateSubscribeRevision(userCounterSubscribe, updateDtoList);

      if (updateDtoList.Count > 0)
        await Clients.Client(Context.ConnectionId).SendAsync("updateCounters", deviceId, processId, updateDtoList);
    }

    public void UnsubscribeCounter([FromServices] CounterSignalService counterSignalService, int deviceId, int processId, string counterTypeString, string counterName)
    {
      if (!Enum.TryParse<CounterType>(counterTypeString, out var counterType))
        throw new ArgumentException(counterTypeString);

      counterSignalService.UnsubscribeConnection(Context.ConnectionId, deviceId, processId, counterType, counterName);
    }
  }
}
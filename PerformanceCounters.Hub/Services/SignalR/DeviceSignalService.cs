using Microsoft.AspNetCore.SignalR;
using PerformanceCounters.Hub.Dto.Device;
using PerformanceCounters.Hub.EF.Entity;
using PerformanceCounters.Hub.SignalR.Hubs;

namespace PerformanceCounters.Hub.Services.SignalR
{
  public class DeviceSignalService
  {
    private readonly IHubContext<ClientHub> _hubContext;

    public DeviceSignalService(IHubContext<ClientHub> hubContext)
    {
      _hubContext = hubContext;
    }

    public async Task AddDeviceAsync(DeviceEntity deviceEntity)
    {
      var dto = new SetDeviceDto
      {
        Id = deviceEntity.Id,
        Name = deviceEntity.Name,
      };

      await _hubContext.Clients.All.SendAsync("addDevice", dto);
    }

    public async Task DeleteDeviceAsync(int deviceId)
    {
      await _hubContext.Clients.All.SendAsync("deleteDevice", deviceId);
    }
  }
}
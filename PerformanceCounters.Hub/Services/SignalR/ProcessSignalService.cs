using Microsoft.AspNetCore.SignalR;
using PerformanceCounters.Hub.Dto.Process;
using PerformanceCounters.Hub.SignalR.Hubs;

namespace PerformanceCounters.Hub.Services.SignalR
{
  public class ProcessSignalService
  {
    private readonly IHubContext<ClientHub> _hubContext;
    public ProcessSignalService(IHubContext<ClientHub> hubContext)
    {
      _hubContext = hubContext;
    }
    public async Task AddProcessAsync(int deviceId, int processId, string processName)
    {
      var dto = new ProcessDto
      {
        Id = processId,
        Name = processName,
      };

      await _hubContext.Clients.All.SendAsync("addProcess", deviceId, dto);
    }

    public async Task DeleteProcessAsync(int processId)
    {
      await _hubContext.Clients.All.SendAsync("deleteProcess", processId);
    }

    public async Task AddCounterNames(int deviceId, int processId, CounterType counterType, List<string> newCounterNames)
    {
      await _hubContext.Clients.All.SendAsync("addCounterNames", deviceId, processId, counterType, newCounterNames);
    }
  }
}

using Microsoft.EntityFrameworkCore;
using PerformanceCounters.Hub.Dto.Process;
using PerformanceCounters.Hub.EF;
using PerformanceCounters.Hub.EF.Entity;
using PerformanceCounters.Hub.Services.SignalR;

namespace PerformanceCounters.Hub.Services
{
  public class ProcessService
  {
    private readonly CountersDbContext _context;
    private readonly DeviceService _deviceService;
    private readonly ProcessSignalService _processSignalService;
    private readonly DbCacheService _dbCacheService;

    public ProcessService(CountersDbContext context, DeviceService deviceService, ProcessSignalService processSignalService, DbCacheService dbCacheService)
    {
      _context = context;
      _deviceService = deviceService;
      _processSignalService = processSignalService;
      _dbCacheService = dbCacheService;
    }

    public async Task<GetProcessTransferInfoDto> GetOrCreateDeviceTransferInfo(string deviceName, string processName)
    {
      var deviceEntity = await _deviceService.GetOrCreateDeviceEntityAsync(deviceName);
      var processEntity = await GetOrCreateProcessEntityAsync(deviceEntity.Id, processName);

      return new GetProcessTransferInfoDto
      {
        DeviceId = deviceEntity.Id,
        ProcessId = processEntity.Id
      };
    }

    private async Task<ProcessEntity> GetOrCreateProcessEntityAsync(int deviceId, string processName)
    {
      var processEntity = await _context.Process.FirstOrDefaultAsync(d=>d.DeviceEntityId == deviceId && d.Name == processName);
      if (processEntity != null) return processEntity;

      processEntity = ProcessEntity.Create(deviceId, processName);
      _context.Process.Add(processEntity);
      await _context.SaveChangesAsync();
      _dbCacheService.TryAddProcess(deviceId, processEntity.Id, processEntity.Name);
      await _processSignalService.AddProcessAsync(deviceId, processEntity.Id, processEntity.Name);
      return processEntity;
    }
  }
}

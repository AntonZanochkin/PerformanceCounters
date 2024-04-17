using Microsoft.EntityFrameworkCore;
using PerformanceCounters.Hub.Dto.Device;
using PerformanceCounters.Hub.EF;
using PerformanceCounters.Hub.EF.Entity;
using PerformanceCounters.Hub.Services.SignalR;

namespace PerformanceCounters.Hub.Services
{
  public class DeviceService
  {
    private readonly CountersDbContext _context;
    private readonly DeviceSignalService _deviceSignalService;
    private readonly DbCacheService _dbCacheService;

    public DeviceService(CountersDbContext context, DeviceSignalService deviceSignalService, DbCacheService dbCacheService)
    {
      _context = context;
      _deviceSignalService = deviceSignalService;
      _dbCacheService = dbCacheService;
    }

    public async Task<DeviceEntity> GetOrCreateDeviceEntityAsync(string deviceName)
    {
      var deviceEntity = await _context.Device
        .FirstOrDefaultAsync(m => m.Name == deviceName);

      if (deviceEntity != null) return deviceEntity;

      deviceEntity = DeviceEntity.Create(deviceName);
      _context.Device.Add(deviceEntity);
      await _context.SaveChangesAsync();
      _dbCacheService.TryAddDevice(deviceEntity.Id, deviceEntity.Name);
      await _deviceSignalService.AddDeviceAsync(deviceEntity.Id, deviceEntity.Name);
      return deviceEntity;
    }

    public SetDevicesDto BuildSetDevicesDto()
    {
      var dto = new SetDevicesDto();
      var cache = _dbCacheService.GetDevices();

      var response = cache
        .Select(dKvP => new SetDeviceDto
        {
          Id = dKvP.Key,
          Name = dKvP.Value.Name,
          Processes = dKvP.Value.Processes.Select(pKvp => pKvp.Value.BuildDto()).ToList()
        }).ToList();

      dto.Devices = response;
      return dto;
    }
  }
}

using Microsoft.EntityFrameworkCore;
using PerformanceCounters.Hub.Dto.Counter;
using PerformanceCounters.Hub.EF;
using PerformanceCounters.Hub.EF.Entity;
using PerformanceCounters.Hub.Services.Cache;
using PerformanceCounters.Hub.Services.SignalR;

namespace PerformanceCounters.Hub.Services
{
  public class CounterService
  {
    private readonly CountersDbContext _dbContext;
    private readonly DbCacheService _dbCacheService;
    private readonly ProcessSignalService _processSignalService;
    private readonly CounterSignalService _counterSignalService;

    public CounterService(CountersDbContext dbContext, ProcessSignalService processSignalService, DbCacheService dbCacheService, CounterSignalService counterSignalService)
    {
      _dbContext = dbContext;
      _dbCacheService = dbCacheService;
      _processSignalService = processSignalService;
      _counterSignalService = counterSignalService;
    }

    public async Task AddCountersAsync(int deviceId, int processId, List<AddCounterDto> addCountersDtoList)
    {
      var counterEntityList = addCountersDtoList.Select(dto => CounterEntity.Create(processId, dto)).ToList();
      await _dbContext.Counter.AddRangeAsync(counterEntityList);
      await _dbContext.SaveChangesAsync();

      var groupEntityListByCounterType = counterEntityList.GroupBy(x => x.Type).ToList();

      foreach (var groupEntityByType in groupEntityListByCounterType)
      {
        var counterType = groupEntityByType.Key;

        var newCounterNames = _dbCacheService
          .TryAddCounterNames(deviceId, processId, counterType, groupEntityByType.Select(x=>x.Name)
            .Distinct()
            .ToArray());

        if (newCounterNames?.Count > 0)
          await _processSignalService.AddCounterNames(deviceId, processId, groupEntityByType.Key, newCounterNames);
      }

      await _counterSignalService.SendCountersToSubscribers(deviceId, processId, groupEntityListByCounterType);
    }

    public async Task<List<AddCounterDto>> SelectCountersDtoAsync(int processId, CounterType counterType, string counterName, int revision)
    {
      var addCounterDtoList = await _dbContext.Counter.Where(n => n.ProcessEntityId == processId && n.Type == counterType && n.Name == counterName && n.Id > revision)
        .Select(n => AddCounterDto.Create(n)).ToListAsync();

      return addCounterDtoList;
    }

    public async Task<List<AddCounterDto>> LoadUpdateDtoAsync(ConnectionSubscribe subscribe)
    {
      var dtoList = new List<AddCounterDto>();
      foreach (var kVp in subscribe.CounterRevisionByName)
      {
        var counterName = kVp.Key;
        var revision = kVp.Value;

        var dto = await SelectCountersDtoAsync(subscribe.ProcessId, subscribe.CounterType, counterName, revision);
        dtoList.AddRange(dto);
      }

      return dtoList;
    }
  }
}

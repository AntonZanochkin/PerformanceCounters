using Microsoft.AspNetCore.Mvc;
using PerformanceCounters.Hub.Dto.Counter;
using PerformanceCounters.Hub.Services;

namespace PerformanceCounters.Hub.Controllers
{
  [Route("api/v1/[controller]/[action]")]
  [ApiController]
  public class CounterController : ControllerBase
  {
    private readonly CounterService _counterService;
    public CounterController(CounterService counterService)
    {
      _counterService = counterService;
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromQuery] int deviceId, [FromQuery] int processId, [FromBody] List<AddCounterDto> dto)
    {
      await _counterService.AddCountersAsync(deviceId, processId, dto);
      return Ok("Success");
    }
  }
}

using Microsoft.AspNetCore.Mvc;
using PerformanceCounters.Hub.Services;

namespace PerformanceCounters.Hub.Controllers
{
  [Route("api/v1/[controller]/[action]")]
  [ApiController]
  public class ProcessController : ControllerBase
  {
    private readonly ProcessService _processService;
    public ProcessController(ProcessService processService)
    {
      _processService = processService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTransferInfo([FromQuery] string deviceName, [FromQuery] string processName)
    {
      var response = await _processService.GetOrCreateDeviceTransferInfo(deviceName, processName);
      return Ok(response);
    }
  }
}

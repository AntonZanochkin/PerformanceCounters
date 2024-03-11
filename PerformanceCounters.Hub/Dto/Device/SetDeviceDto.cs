using PerformanceCounters.Hub.Dto.Process;

namespace PerformanceCounters.Hub.Dto.Device
{
  public class SetDeviceDto
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public IList<ProcessDto> Processes { get; set; } = new List<ProcessDto>();
  }
}
namespace PerformanceCounters.Hub.Dto.Process
{
  public class AddProcessDto
  {
    public int DeviceId { get; set; }
    public int ProcessId { get; set; }
    public required string ProcessName { get; set; }
  }
}

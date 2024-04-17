namespace PerformanceCounters.Hub.Dto.Counter
{
  public class UpdateCountersDto
  {
    public int DeviceId { get; set; }
    public int ProcessId { get; set; }
    public List<UpdateCounterDto> Counters { get; set; }
  }
}

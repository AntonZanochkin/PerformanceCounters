namespace PerformanceCounters.Hub.Dto.Counter
{
  public class AddCounterNamesDto
  {
    public int DeviceId { get; set; }
    public int ProcessId { get; set; }

    public CounterType CounterType { get; set; }
    public required List<string> NewCounterNames { get; set; }
  }
}

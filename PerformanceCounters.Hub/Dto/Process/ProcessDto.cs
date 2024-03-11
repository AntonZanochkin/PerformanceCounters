namespace PerformanceCounters.Hub.Dto.Process
{
  public class ProcessDto
  {
    public int Id { get; set; }
    public string Name { get; set; }

    public Dictionary<string, List<string>> CounterNamesByType { get; set; } = new ();

    public static ProcessDto Create(int id, string name, Dictionary<string, List<string>> counterNamesByType)
    {
      return new ProcessDto
      {
        Id = id,
        Name = name,
        CounterNamesByType = counterNamesByType,
      };
    }
  }
}
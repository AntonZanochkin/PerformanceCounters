using PerformanceCounters.Hub.EF.Entity;

namespace PerformanceCounters.Hub.Dto.Counter
{
  public class AddCounterDto
  {
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public CounterType Type { get; set; }
    public string Name { get; set; }
    public string ValueJson { get; set; }

    public static AddCounterDto Create(CounterEntity counterEntity)
    {
      return new AddCounterDto
      {
        Id = counterEntity.Id,
        DateTime = counterEntity.DateTime,
        Type = counterEntity.Type,
        Name = counterEntity.Name,
        ValueJson = counterEntity.ValueJson,
      };
    }
  }
}

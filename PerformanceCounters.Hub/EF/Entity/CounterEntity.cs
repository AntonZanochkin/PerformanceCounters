using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PerformanceCounters.Hub.Dto.Counter;

namespace PerformanceCounters.Hub.EF.Entity
{
  [Table("Counter")]
  public class CounterEntity
  {
    [Key]
    public int Id { get; set; }
    public int ProcessEntityId { get; set; }
    public DateTime DateTime { get; set; }
    public CounterType Type { get; set; }
    public string Name { get; set; }
    public string ValueJson { get; set; }

    public static CounterEntity Create(int processId, AddCounterDto addCounterDto)
    {
      return new CounterEntity
      {
        ProcessEntityId = processId,
        DateTime = addCounterDto.DateTime,
        Type = addCounterDto.Type,
        Name = addCounterDto.Name,
        ValueJson = addCounterDto.ValueJson,
      };
    }
  }
}

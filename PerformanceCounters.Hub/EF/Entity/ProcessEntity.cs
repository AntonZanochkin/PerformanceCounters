using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PerformanceCounters.Hub.EF.Entity
{
  [Table("Process")]
  public class ProcessEntity
  {
    [Key]
    public int Id { get; set; }
    public int DeviceEntityId { get; set; }
    public string Name { get; set; }
    public List<CounterEntity> Counters { get; set; } = new();

    public static ProcessEntity Create(int deviceId, string name)
    {
      return new ProcessEntity
      {
        DeviceEntityId = deviceId,
        Name = name
      };
    }
  }
}

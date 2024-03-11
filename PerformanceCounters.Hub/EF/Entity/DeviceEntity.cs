using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PerformanceCounters.Hub.EF.Entity
{
  [Table("Device")]
  public class DeviceEntity
  {
    [Key] public int Id { get; set; }
    public string Name { get; set; }
    public List<ProcessEntity> Processes { get; set; } = new();

    public static DeviceEntity Create(string name)
    {
      return new DeviceEntity
      {
        Name = name,
      };
    }
  }
}
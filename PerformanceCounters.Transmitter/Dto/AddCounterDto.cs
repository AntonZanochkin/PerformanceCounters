using System;
using PerformanceCounters.Transmitter.Counters;

namespace PerformanceCounters.Transmitter.Dto
{
    public class AddCounterDto
  {
    public DateTime DateTime { get; set; }
    public CounterType Type { get; set; }
    public string Name { get; set; }
    public string ValueJson { get; set; }

    public static AddCounterDto Create(DateTime time, CounterType type, string name, ICounterData counterData)
    {
      return new AddCounterDto { DateTime = time, Type = type, Name = name, ValueJson = counterData.JsonValue };
    }
  }
}

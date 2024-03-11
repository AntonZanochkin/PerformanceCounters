using System.Threading;
using Newtonsoft.Json;

namespace PerformanceCounters.Transmitter.Counters.IntegerCounter
{
  public class IntegerCounterData : ICounterData
  {
    private int _value;
    public string JsonValue => JsonConvert.SerializeObject(new {value = _value});

    public int Increment()
    {
      return Interlocked.Increment(ref _value);
    }

    public int IncrementBy(int value)
    {
      return Interlocked.Add(ref _value, value);
    }

    public int Decrement()
    {
      return Interlocked.Decrement(ref _value);
    }

    public int DecrementBy(int value)
    {
      return Interlocked.Add(ref _value, -value);
    }

    public int SetValue(int value)
    {
      return Interlocked.Exchange(ref _value, value);
    }
  }
}
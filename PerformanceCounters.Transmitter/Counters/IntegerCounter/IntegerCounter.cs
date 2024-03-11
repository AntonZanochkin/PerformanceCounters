using System;

namespace PerformanceCounters.Transmitter.Counters.IntegerCounter
{
  public class IntegerCounter : IDisposable
  {
    private readonly string _name;

    public IntegerCounter(string name)
    {
      _name = name;
    }

    public IntegerCounter Increment()
    {
      var namedStorage = StorageService.IntegerStorage.GetOrCreateNamedStorage(_name);
      namedStorage.Increment();
      return this;
    }

    public IntegerCounter IncrementBy(int value)
    {
      var namedStorage = StorageService.IntegerStorage.GetOrCreateNamedStorage(_name);
      namedStorage.IncrementBy(value);
      return this;
    }

    public IntegerCounter Decrement(int value)
    {
      var namedStorage = StorageService.IntegerStorage.GetOrCreateNamedStorage(_name);
      namedStorage.Decrement();
      return this;
    }

    public IntegerCounter DecrementBy(int value)
    {
      var namedStorage = StorageService.IntegerStorage.GetOrCreateNamedStorage(_name);
      namedStorage.DecrementBy(value);
      return this;
    }

    public void Dispose()
    {
    }
  }
}
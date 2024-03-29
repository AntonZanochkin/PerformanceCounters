﻿using PerformanceCounters.Transmitter.Counters.CpuTimeCounter;
using PerformanceCounters.Transmitter.Counters.IntegerCounter;
using PerformanceCounters.Transmitter.Counters.StopwatchCounter;
using PerformanceCounters.Transmitter.Helpers;
using PerformanceCounters.Transmitter.Services;

var transferService = new TransferService("http://localhost:5068/api/v1", "Server 1", "Test application 1");
transferService.Run();

var timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

Console.WriteLine("Timer started. Press any key to exit.");
Console.ReadKey();
timer.Dispose();

static async void TimerCallback(object? state)
{
  var rnd = new Random();

  using (new CpuTimeCounter("LoadCpuSync 4 sec, sleep 6 sec"))
  {
    CpuLoader.LoadCpuSync(1000);
    Thread.Sleep(1000);
    await Task.Delay(5000);
    await CpuLoader.LoadCpuAsync(3000);
  }

  using (new CpuTimeCounter("LoadCpuSync (parent + child) 4 sec, sleep 2 sec"))
  {
    CpuLoader.LoadCpuSync(1000);
    Thread.Sleep(500);
    await CpuLoader.LoadCpuAsync(1000);
    await Task.Delay(500);

    using (new CpuTimeCounter("LoadCpuSync 2 sec (child), sleep 1 sec"))
    {
      CpuLoader.LoadCpuSync(1000);
      Thread.Sleep(500);
      CpuLoader.LoadCpuSync(1000);
      await Task.Delay(500);
    }
  }

  using (new CpuTimeCounter("Random LoadCpuSync (parent + child) from 2 to 10 sec"))
  {
    await CpuLoader.LoadCpuAsync(rnd.Next(1 * 1000, 5 * 1000));
    await Task.Delay(rnd.Next(1 * 1000, 5 * 1000));

    using (new CpuTimeCounter("Random LoadCpuSync (child)  from 1 to 5 sec"))
    {
      await CpuLoader.LoadCpuAsync(rnd.Next(1 * 1000, 5 * 1000));
      await Task.Delay(rnd.Next(1 * 1000, 5 * 1000));
    }
  }

  for (var t = 0; t < 10; t++)
    using (new IntegerCounter("IncrementBy 3 x 10").IncrementBy(3)) { }

  for (var t = 0; t < 5; t++)
    using (new IntegerCounter("Increment 1 x 5").Increment()) { }

  await Task.Delay(30000);

  using (new IntegerCounter("IntegerCounter from 1000 to 5000").IncrementBy(rnd.Next(1000, 5000)))
  {
  }

  using (new IntegerCounter("IntegerCounter from 5000 to 30000").IncrementBy(rnd.Next(1000, 30000)))
  {
  }

  using (new StopwatchCounter("StopwatchCounter from 5 to 10 sec"))
  {
    await Task.Delay(rnd.Next(5 * 1000, 10 * 1000));
  }
}
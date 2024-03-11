using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using System.Threading;
using PerformanceCounters.Transmitter.Counters.CpuTimeCounter;
using PerformanceCounters.Transmitter.Helpers;

namespace PerformanceCounters.Transmitter.Tests
{
  [TestClass]
  public class CpuTimeCounterTests
  {
    private long _frequency;
    
    [TestInitialize]
    public void ClassInitialize()
    {
      WindowsCpuUsageInterop.QueryPerformanceFrequency(out var frequency);
      _frequency = frequency;
    }

    [TestMethod]
    public void CpuTimeCounter_LoadCpu_2()
    {
      // Arrange
      var testName = "CpuTimeCounter_LoadCpu_2";
      var loadCpuSec = 2;
      var sleepCpuSec = 0;
      var cpuTimeCounter = new CpuTimeCounter(testName);

      // Act
      CpuLoader.LoadCpu(loadCpuSec * 1000);
      cpuTimeCounter.Dispose();

      // Assert
      var actualTotalTicks = cpuTimeCounter.Metric.UserTicks + cpuTimeCounter.Metric.KernelTicks;
      var actualLoadCpuSec = actualTotalTicks / (double)_frequency;
      var actualDurationSec = cpuTimeCounter.Duration.ElapsedTicks / (double)_frequency;
      var actualSleepCpuSec = actualDurationSec - actualLoadCpuSec;

      Assert.AreEqual(loadCpuSec, actualLoadCpuSec, 0.1);
      Assert.AreEqual(sleepCpuSec, actualSleepCpuSec, 0.1);
    }

    [TestMethod]
    public async Task CpuTimeCounter_LoadCpu_2_Async()
    {
      // Arrange
      var testName = "CpuTimeCounter_LoadCpu_2_Async";
      var loadCpuSec = 2;
      var sleepCpuSec = 0;
      var cpuTimeCounter = new CpuTimeCounter(testName);

      // Act
      await CpuLoader.LoadCpuAsync(loadCpuSec * 1000);
      cpuTimeCounter.Dispose();

      // Assert
      var actualTotalTicks = cpuTimeCounter.Metric.UserTicks + cpuTimeCounter.Metric.KernelTicks;
      var actualLoadCpuSec = actualTotalTicks / (double)_frequency;
      var actualDurationSec = cpuTimeCounter.Duration.ElapsedTicks / (double)_frequency;
      var actualSleepCpuSec = actualDurationSec - actualLoadCpuSec;

      Assert.AreEqual(loadCpuSec, actualLoadCpuSec, 0.1);
      Assert.AreEqual(sleepCpuSec, actualSleepCpuSec, 0.1);
    }

    [TestMethod]
    public void CpuTimeCounter_ThreadSleep_2()
    {
      // Arrange
      var testName = "CpuTimeCounter_ThreadSleep_2";
      var loadCpuSec = 0;
      var sleepCpuSec = 2;
      var cpuTimeCounter = new CpuTimeCounter(testName);

      // Act
      Thread.Sleep(sleepCpuSec * 1000);
      cpuTimeCounter.Dispose();

      // Assert
      var actualTotalTicks = cpuTimeCounter.Metric.UserTicks + cpuTimeCounter.Metric.KernelTicks;
      var actualLoadCpuSec = actualTotalTicks / (double)_frequency;
      var actualDurationSec = cpuTimeCounter.Duration.ElapsedTicks / (double)_frequency;
      var actualSleepCpuSec = actualDurationSec - actualLoadCpuSec;

      Assert.AreEqual(loadCpuSec, actualLoadCpuSec, 0.1);
      Assert.AreEqual(sleepCpuSec, actualSleepCpuSec, 0.1);
    }

    [TestMethod]
    public void CpuTimeCounter_TaskDelay_2_NoWait()
    {
      // Arrange
      var testName = "CpuTimeCounter_TaskDelay_2_NoWait";
      var loadCpuSec = 0;
      var sleepCpuSec = 2;
      var cpuTimeCounter = new CpuTimeCounter(testName);

      // Act
      Task.Delay(sleepCpuSec * 1000);
      cpuTimeCounter.Dispose();

      // Assert
      var actualTotalTicks = cpuTimeCounter.Metric.UserTicks + cpuTimeCounter.Metric.KernelTicks;
      var actualLoadCpuSec = actualTotalTicks / (double)_frequency;
      var actualDurationSec = cpuTimeCounter.Duration.ElapsedTicks / (double)_frequency;
      var actualSleepCpuSec = actualDurationSec - actualLoadCpuSec;

      Assert.AreEqual(loadCpuSec, actualLoadCpuSec, 0.1);
      Assert.AreEqual(0, actualSleepCpuSec, 0.1);//NoWait
    }

    [TestMethod]
    public async Task CpuTimeCounter_LoadCpu_2_LoadCpu_2_Async_ThreadSleep_1_TaskDelay_1()
    {
      // Arrange
      var testName = "CpuTimeCounter_ThreadSleep_2";
      var loadCpuSec = 4d;
      var sleepCpuSec = 2d;
      var cpuTimeCounter = new CpuTimeCounter(testName);

      // Act
      CpuLoader.LoadCpu((int)(loadCpuSec / 2 * 1000));
      await Task.Delay((int)(sleepCpuSec / 2 * 1000));
      Thread.Sleep((int)(sleepCpuSec / 2 * 1000));
      await CpuLoader.LoadCpuAsync((int)(loadCpuSec / 2 * 1000));
      cpuTimeCounter.Dispose();

      // Assert
      var actualTotalTicks = cpuTimeCounter.Metric.UserTicks + cpuTimeCounter.Metric.KernelTicks;
      var actualLoadCpuSec = actualTotalTicks / (double)_frequency;
      var actualDurationSec = cpuTimeCounter.Duration.ElapsedTicks / (double)_frequency;
      var actualSleepCpuSec = actualDurationSec - actualLoadCpuSec;

      Assert.AreEqual(loadCpuSec, actualLoadCpuSec, 0.1);
      Assert.AreEqual(sleepCpuSec, actualSleepCpuSec, 0.1);
    }

    [TestMethod]
    public async Task CpuTimeCounter_LoadCpuParentAndChild_4_2_LoadCpuChild_2_1()
    {
      // Arrange
      var parentTestName = "LoadCpu (parent + child) 4 sec, sleep 2 sec";
      var childTestName = "LoadCpu 2 sec (child), sleep 1 sec";
      var parentLoadCpuSec = 4d;
      var parentSleepCpuSec = 2d;
      var childLoadCpuSec = 2d;
      var childSleepCpuSec = 1d;

      // Act
      var parentCpuTimeCounter = new CpuTimeCounter(parentTestName);
      
      CpuLoader.LoadCpu(1000);
      Thread.Sleep(500);
      await CpuLoader.LoadCpuAsync(1000);
      await Task.Delay(500);

      var childCpuTimeCounter = new CpuTimeCounter(childTestName);
        
      CpuLoader.LoadCpu(1000);
      Thread.Sleep(500);
      await CpuLoader.LoadCpuAsync(1000);
      await Task.Delay(500);

      childCpuTimeCounter.Dispose();
      parentCpuTimeCounter.Dispose();

      // Assert
      var parentActualTotalTicks = parentCpuTimeCounter.Metric.UserTicks + parentCpuTimeCounter.Metric.KernelTicks;
      var parentActualLoadCpuSec = parentActualTotalTicks / (double)_frequency;
      var parentActualDurationSec = parentCpuTimeCounter.Duration.ElapsedTicks / (double)_frequency;
      var parentActualSleepCpuSec = parentActualDurationSec - parentActualLoadCpuSec;

      Assert.AreEqual(parentLoadCpuSec, parentActualLoadCpuSec, 0.1);
      Assert.AreEqual(parentSleepCpuSec, parentActualSleepCpuSec, 0.1);

      var childActualTotalTicks = childCpuTimeCounter.Metric.UserTicks + childCpuTimeCounter.Metric.KernelTicks;
      var childActualLoadCpuSec = childActualTotalTicks / (double)_frequency;
      var childActualDurationSec = childCpuTimeCounter.Duration.ElapsedTicks / (double)_frequency;
      var childActualSleepCpuSec = childActualDurationSec - childActualLoadCpuSec;

      Assert.AreEqual(childLoadCpuSec, childActualLoadCpuSec, 0.1);
      Assert.AreEqual(childSleepCpuSec, childActualSleepCpuSec, 0.1);
    }
  }
}

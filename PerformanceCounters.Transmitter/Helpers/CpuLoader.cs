using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace PerformanceCounters.Transmitter.Helpers
{
  public static class CpuLoader
  {
    public static void LoadCpu(int milliseconds)
    {
      var sw = Stopwatch.StartNew();
      while (sw.ElapsedMilliseconds <= milliseconds)
      {
        for (int i = 0; i < 50; i++);
      }
      sw.Stop();
    }

    public static async Task LoadCpuAsync(int milliseconds)
    {
      await Task.Run(() => LoadCpu(milliseconds));
    }

    public static void LoadCpuKernel(int milliseconds)
    {
      var sw = Stopwatch.StartNew();
      while (sw.ElapsedMilliseconds <= milliseconds)
      {
        var ptr = Marshal.AllocHGlobal(4 * 1024);
        Marshal.FreeHGlobal(ptr);
      }
      sw.Stop();
    }
  }
}

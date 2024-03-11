using System;
using System.Runtime.InteropServices;
using System.Security;

namespace PerformanceCounters.Transmitter.Counters.CpuTimeCounter
{
  public class WindowsCpuUsageInterop
  {
    [DllImport("kernel32.dll")]
    internal static extern IntPtr GetCurrentThread();

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool GetThreadTimes(IntPtr hThread, out long lpCreationTime,
      out long lpExitTime, out long lpKernelTime, out long lpUserTime);

    [DllImport("kernel32.dll")]
    public static extern IntPtr GetCurrentProcess();

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool GetProcessTimes(IntPtr hThread, out long lpCreationTime,
      out long lpExitTime, out long lpKernelTime, out long lpUserTime);

    [SuppressUnmanagedCodeSecurity]
    [DllImport("Kernel32.dll")]
    public static extern bool QueryPerformanceFrequency(out long lpFrequency);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DuplicateHandle(IntPtr hSourceProcessHandle,
      IntPtr hSourceHandle, IntPtr hTargetProcessHandle, out IntPtr lpTargetHandle,
      uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwOptions);

    [Flags]
    public enum DuplicateOptions : uint
    {
      DUPLICATE_CLOSE_SOURCE = (0x00000001), // Closes the source handle. This occurs regardless of any error status returned.
      DUPLICATE_SAME_ACCESS = (0x00000002),  // Ignores the dwDesiredAccess parameter. The duplicate handle has the same access as the source handle.
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern uint Sleep(uint milliseconds);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr CreateFile(
      string lpFileName,
      uint dwDesiredAccess,
      uint dwShareMode,
      IntPtr lpSecurityAttributes,
      uint dwCreationDisposition,
      uint dwFlagsAndAttributes,
      IntPtr hTemplateFile
    );

    public static bool GetThreadTimes(out long kernelMicroseconds, out long userMicroseconds)
    {
      if (GetThreadTimes(GetCurrentThread(), out _, out _, out var kernel, out var user))
      {
        kernelMicroseconds = kernel / 10L;
        userMicroseconds = user / 10L;
        return true;
      }
      else
      {
        kernelMicroseconds = -1;
        userMicroseconds = -1;
        return false;
      }

    }
  }
}

using System;
using System.Linq;

namespace PerformanceCounters.Transmitter.Helpers
{
  internal class LocalDeviceInfo
  {
    private string _deviceName;
    private string _deviceAddress;
    private string _combinedDeviceName;
    private string _processName;
    private int _processId;
    public string DeviceName => _deviceName ?? (_deviceName = Environment.MachineName);

    public string DeviceAddress
    {
      get
      {
        if (_deviceAddress != null) return _deviceAddress;

        if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
          return _deviceAddress ?? (_deviceAddress = "<no network>");
        
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        var ip = host.AddressList.FirstOrDefault(i =>
          i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

        if (ip != null)
          _deviceAddress = ip.ToString();

        return _deviceAddress ?? (_deviceAddress = "<no network>");
      }
    }

    public string CombinedDeviceName =>
      _combinedDeviceName ?? (_combinedDeviceName = DeviceName + "(" + DeviceAddress + ")");

    public string ProcessName
    {
      get
      {
        if (_processName != null) return _processName;

        var process = System.Diagnostics.Process.GetCurrentProcess();
        _processName = process.ProcessName;

        return _processName;
      }
    }

    public int ProcessId
    {
      get
      {
        if (_processId != 0) return _processId;

        var process = System.Diagnostics.Process.GetCurrentProcess();
        _processId = process.Id;

        return _processId;
      }
    }
  }
}
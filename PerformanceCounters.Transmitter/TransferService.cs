using System;
using System.Collections.Generic;
using PerformanceCounters.Transmitter.Extensions;
using System.Threading;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using PerformanceCounters.Transmitter.Dto;

namespace PerformanceCounters.Transmitter
{
  public class TransferService
  {
    private Timer _pushTimer;
    private bool _inProgress;
    private readonly string _baseUrl;

    private readonly string _deviceName;
    private readonly string _processName;
    private int? _transferDeviceId;
    private int? _transferProcessId;
    public TransferService(string url, string deviceName, string processName)
    {
      _baseUrl = url;
      _deviceName = deviceName;
      _processName = processName;
    }

    public void Run()
    {
      _pushTimer = new Timer(CollectCompletedStorage, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
    }

    private async void CollectCompletedStorage(object _)
    {
      try
      {
        if (_inProgress) return;

        _inProgress = true;
        await CollectCompletedStorageImpl();
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
      }
      finally
      {
        _inProgress = false;
      }
    }

    private async Task CollectCompletedStorageImpl()
    {
      var finalizedTime = DateTime.UtcNow.RoundToMinute().AddMinutes(-1);

      if (!_transferDeviceId.HasValue || !_transferProcessId.HasValue)
      {
        var transferInfo = await GetProcessTransferInfo(_deviceName, _processName);
        _transferDeviceId = transferInfo.DeviceId;
        _transferProcessId = transferInfo.ProcessId;
      }

      var addCounterDtoList = StorageService.BuildAddCounterDtoUpToTime(finalizedTime);
      if (addCounterDtoList.Count == 0)
        return;
      
      if (await PostCounters(_transferDeviceId.Value, _transferProcessId.Value, addCounterDtoList))
        StorageService.DeleteCountersUpToTime(finalizedTime);
    }

    private async Task<GetProcessTransferInfoDto> GetProcessTransferInfo(string deviceName, string processName)
    {
      var uriBuilder = new UriBuilder($"{_baseUrl}/Process/GetTransferInfo");
      var query = HttpUtility.ParseQueryString(uriBuilder.Query);
      query["deviceName"] = deviceName;
      query["processName"] = processName;
      uriBuilder.Query = query.ToString();

      try
      {
        using (var client = new HttpClient())
        {
          var response = await client.GetAsync(uriBuilder.Uri);

          if (response.IsSuccessStatusCode)
          {
            var result = await response.Content.ReadAsStringAsync();
            var localDeviceInfo = JsonConvert.DeserializeObject<GetProcessTransferInfoDto>(result);
            return localDeviceInfo;
          }
          Console.WriteLine($"Request failed with status code: {response.StatusCode}");
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      throw new InvalidOperationException("Failed to get local device info.");
    }
    private async Task<bool> PostCounters(int deviceId, int processId, List<AddCounterDto> addCounterDtoList)
    {
      var uriBuilder = new UriBuilder($"{_baseUrl}/Counter/Add");
      var query = HttpUtility.ParseQueryString(uriBuilder.Query);
      query["deviceId"] = deviceId.ToString();
      query["processId"] = processId.ToString();
      uriBuilder.Query = query.ToString();

      var jsonPayload = JsonConvert.SerializeObject(addCounterDtoList);
      try
      {
        using (var client = new HttpClient())
        {
          var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

          var response = await client.PostAsync(uriBuilder.Uri, content);
          if (response.IsSuccessStatusCode)
          {
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"The counters were successfully synchronized");
            return true;
          }
          else
          {
            Console.WriteLine($"Request failed with status code: {response.StatusCode}");
            return false;
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return false;
      }
    }
  }
}
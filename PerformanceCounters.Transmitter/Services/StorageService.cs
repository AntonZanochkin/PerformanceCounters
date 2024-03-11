using System.Collections.Generic;
using System;
using PerformanceCounters.Transmitter.Counters;
using PerformanceCounters.Transmitter.Counters.CpuTimeCounter;
using PerformanceCounters.Transmitter.Counters.IntegerCounter;
using PerformanceCounters.Transmitter.Counters.StopwatchCounter;
using PerformanceCounters.Transmitter.Dto;

namespace PerformanceCounters.Transmitter.Services
{
    public static class StorageService
    {
        public static TimeStorage<IntegerCounterData> IntegerStorage = new TimeStorage<IntegerCounterData>(CounterType.Integer);
        public static TimeStorage<StopwatchCounterData> StopwatchStorage = new TimeStorage<StopwatchCounterData>(CounterType.Stopwatch);
        public static TimeStorage<CpuTimeCounterData> CpuTimeStorage = new TimeStorage<CpuTimeCounterData>(CounterType.CpuTime);

        public static List<AddCounterDto> BuildAddCounterDtoUpToTime(DateTime endTime)
        {
            var addCounterDtoList = new List<AddCounterDto>();

            IntegerStorage.EnumerateCounterDataUpToTime(endTime, (time, name, data) => addCounterDtoList.Add(AddCounterDto.Create(time, CounterType.Integer, name, data)));
            StopwatchStorage.EnumerateCounterDataUpToTime(endTime, (time, name, data) => addCounterDtoList.Add(AddCounterDto.Create(time, CounterType.Stopwatch, name, data)));
            CpuTimeStorage.EnumerateCounterDataUpToTime(endTime, (time, name, data) => addCounterDtoList.Add(AddCounterDto.Create(time, CounterType.CpuTime, name, data)));

            return addCounterDtoList;
        }

        public static void DeleteCountersUpToTime(DateTime endTime)
        {
            IntegerStorage.DeleteCountersUpToTime(endTime);
            StopwatchStorage.DeleteCountersUpToTime(endTime);
            CpuTimeStorage.DeleteCountersUpToTime(endTime);
        }
    }
}

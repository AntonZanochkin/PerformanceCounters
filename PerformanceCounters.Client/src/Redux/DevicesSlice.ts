import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { StoreType } from '../types/StoreType.ts';
import { PayloadType } from '../types/PayloadType.ts';

export type SliceState = {
  devices: Array<StoreType.Device>;
};

export const devicesSlice = createSlice({
  name: "devices",
  initialState: [],
  reducers: {
    addDevice: (devices: Array<StoreType.Device>, action: PayloadAction<PayloadType.AddDevice>) => {
      devices.push({id: action.payload.id,  name: action.payload.name, processes:[]});
    },

    setDevices: (state: Array<StoreType.Device>, action: PayloadAction<PayloadType.SetDevices>) => {
      const devicesPayload: Array<StoreType.Device> = action.payload.devices.map(d => ({
        id: d.id,
        name: d.name,
        processes: d.processes.map(p => ({
          id: p.id,
          name: p.name,
          counters: [],
          counterNamesByType: p.counterNamesByType
        }))
      }));

      state.splice(0, state.length, ...devicesPayload);
    },

    addProcess: (devices: Array<StoreType.Device>, action: PayloadAction<PayloadType.AddProcess>) => {
      const { deviceId, processId, processName } = action.payload;
      const { device } = getDeviceAndProcessOrThrow(devices, deviceId, processId);

      const counterNamesByType: EnumDictionary<CounterType, string[]> = {
        [CounterType.Integer]: [],
        [CounterType.Stopwatch]: [],
        [CounterType.CpuTime]: [],
      };

      device.processes.push({id: processId, name: processName,  counters: [], counterNamesByType: counterNamesByType});
    },

    addCounterNames: (devices: Array<StoreType.Device>, action: PayloadAction<PayloadType.AddCounterNames>) => {
      const { deviceId, processId, counterType, newCounterNames } = action.payload;
      const { process } = getDeviceAndProcessOrThrow(devices, deviceId, processId);

      if (!process.counterNamesByType[counterType]) process.counterNamesByType[counterType] = [];
      process.counterNamesByType[counterType].push(...newCounterNames);
    },

    updateCounters: (devices: Array<StoreType.Device>, action:PayloadAction<PayloadType.UpdateCounters>) => {
      const { deviceId, processId, counters } = action.payload;
      const { process } = getDeviceAndProcessOrThrow(devices, deviceId, processId);

      if (!process.counters) process.counters = [];

      counters.map((updateDto) => {
        let parsedValue = JSON.parse(updateDto.valueJson);
        Object.assign(updateDto, parsedValue);
      });

      process.counters.push(...counters);
    },
  },

  extraReducers(builder) {},
});

function getDeviceAndProcessOrThrow(devices: StoreType.Device[], deviceId: number, processId: number) {
  
  const device = devices.find((x) => x.id === deviceId);
  if (!device) {
    throw new Error(`Device with ID ${deviceId} not found`);
  }

  const process = device.processes.find((x) => x.id === processId);
  if (!process) {
    throw new Error(`Process with ID ${processId} not found`);
  }

  return { device, process };
}

export const selectCounters = (deviceId: number, processId: number, counterType: string, counterName:string, revision:number) => (state : SliceState) => {
  
  const device = state.devices.find((x) => x.id === deviceId);
  if (device === undefined) 
    throw new Error(`Device with ID ${deviceId} not found`);

  const process = device.processes.find((x) => x.id === processId);
  if (process === undefined) 
    throw new Error(`Process with ID ${processId} not found`);

  return process.counters?.filter((x) => x.type === counterType && x.name === counterName && x.id > revision);
};

export const selectDevices = () => (state: SliceState) => {
  return state.devices;
};

export const selectDeviceAndProcess = (deviceId: number, processId: number) => (state: SliceState) => {
  
  const device = state.devices.find((x) => x.id === deviceId);
  if (device === undefined) 
    throw new Error(`Device with ID ${deviceId} not found`);

  const process = device.processes.find((x) => x.id === processId);
  if (process === undefined) 
    throw new Error(`Process with ID ${processId} not found`);

  return {device, process};
};

export const selectDeviceAndProcessOrDefault = (deviceId: number, processId: number) => (state: SliceState) => {
  
  const device = state.devices.find((x) => x.id === deviceId);
  if (device === undefined) 
    return undefined;

  const process = device.processes.find((x) => x.id === processId);
  if (process === undefined) 
    return undefined;

  return {device, process};
};

export const { setDevices, addDevice, addProcess, addCounterNames, updateCounters } = devicesSlice.actions;
export default devicesSlice.reducer;

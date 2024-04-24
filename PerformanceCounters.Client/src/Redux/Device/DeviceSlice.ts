import { createSelector, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { StoreType } from '../../types/StoreType.ts';
import { DevicePayloadType } from './ServerDataPayloadType.ts';
import { CounterType, EnumDictionary } from '../../types/CounterType.ts';
import { RootState } from "../Store.ts";

export interface DeviceState {
  devices: Array<StoreType.Device>;
};

const initialState: DeviceState = {
  devices: [],
}

export const DeviceSlice = createSlice({
  name: "serverData",
  initialState: initialState,
  reducers: {
    addDevice: (state, action: PayloadAction<DevicePayloadType.AddDevice>) => {
      state.devices.push({id: action.payload.id,  name: action.payload.name, processes:[]});
    },

    setDevices: (state, action: PayloadAction<DevicePayloadType.SetDevices>) => {
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

      state.devices.splice(0, state.devices.length, ...devicesPayload);
    },

    addProcess: (state, action: PayloadAction<DevicePayloadType.AddProcess>) => {
      const { deviceId, processId, processName } = action.payload;
      const { device } = findDeviceAndProcess(state.devices, deviceId, processId);

      const counterNamesByType: EnumDictionary<CounterType, string[]> = {
        [CounterType.Integer]: [],
        [CounterType.Stopwatch]: [],
        [CounterType.CpuTime]: [],
      };

      device.processes.push({id: processId, name: processName,  counters: [], counterNamesByType: counterNamesByType});
    },

    addCounterNames: (state, action: PayloadAction<DevicePayloadType.AddCounterNames>) => {
      const { deviceId, processId, counterType, newCounterNames } = action.payload;
      const { process } = findDeviceAndProcess(state.devices, deviceId, processId);

      if (!process.counterNamesByType[counterType]) process.counterNamesByType[counterType] = [];
      process.counterNamesByType[counterType].push(...newCounterNames);
    },

    updateCounters: (state, action:PayloadAction<DevicePayloadType.UpdateCounters>) => {
      const { deviceId, processId, counters } = action.payload;
      const { process } = findDeviceAndProcess(state.devices, deviceId, processId);

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

export const { setDevices, addDevice, addProcess, addCounterNames, updateCounters } = DeviceSlice.actions;
export default DeviceSlice.reducer;

//Selectors

type Selector<S> = (state: RootState) => S;

export const selectDevices = 
createSelector(
  [(state: RootState) => state.deviceState.devices], 
  (devices) => devices
);

export const selectCounters = (deviceId: number, processId: number, counterType: string, counterName:string, revision:number): Selector<StoreType.Counter[]> =>
  createSelector(
    [(state: RootState) => state.deviceState.devices],
    (devices) => {
      const { process } = findDeviceAndProcess(devices, deviceId, processId);
      return process.counters?.filter((x) => x.type === counterType && x.name === counterName && x.id > revision);
    }
  );

export const selectDeviceAndProcess = (deviceId: number, processId: number): Selector<{device:StoreType.Device, process:StoreType.Process}> =>
  createSelector(
    [(state: RootState) => state.deviceState.devices],
    (devices) => {
      const { device, process } = findDeviceAndProcess(devices, deviceId, processId);
      return {device, process};
    }
  );

export const selectDeviceAndProcessOrDefault = (deviceId: number, processId: number) : Selector<{device:StoreType.Device | undefined, process:StoreType.Process | undefined}> =>
  createSelector(
    [(state: RootState) => state.deviceState.devices],
    (devices) => {
      const device = devices.find((x) => x.id === deviceId);
      if (device === undefined) 
        return { device: undefined, process: undefined};
    
      const process = device.processes.find((x) => x.id === processId);
      if (process === undefined) 
        return { device: undefined, process: undefined};

      return {device, process};
    }
  );

function findDeviceAndProcess(devices: StoreType.Device[], deviceId: number, processId: number) {
  
  const device = devices.find((x) => x.id === deviceId);
  if (!device) {
    throw new Error(`Device with ID ${deviceId} not found`);
  }

  const process = device?.processes.find((x) => x.id === processId);
  if (!process) {
    throw new Error(`Process with ID ${processId} not found`);
  }

  return { device, process };
}
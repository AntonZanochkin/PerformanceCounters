import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";

export const devicesSlice = createSlice({
  name: "devices",
  initialState: [],
  reducers: {
    setDevices: (state, action) => {
      return action.payload;
    },
    addDevice: (state, action) => {
      state.push(action.payload);
    },
    addProcess: (state, action) => {
      const { deviceId, addProcessDto } = action.payload;
      const device = state.find((x) => x.id === deviceId);
      device.processes.push(addProcessDto);
    },
    addCounterNames: (state, action) => {
      const { deviceId, processId, counterType, newCounterNames } = action.payload;
      const device = state.find((x) => x.id === deviceId);
      const process = device.processes.find((x) => x.id === processId);

      if (!process.counterNamesByType[counterType]) process.counterNamesByType[counterType] = [];
      process.counterNamesByType[counterType].push(...newCounterNames);
    },
    updateCounters: (state, action) => {
      const { deviceId, processId, updateDtoList } = action.payload;
      const device = state.find((x) => x.id === deviceId);
      const process = device.processes.find((x) => x.id === processId);
      if (!process.counters) process.counters = [];

      updateDtoList.map((updateDto) => {
        let parsedValue = JSON.parse(updateDto.valueJson);
        Object.assign(updateDto, parsedValue);
      });

      process.counters.push(...updateDtoList);
    },
  },
  extraReducers(builder) {},
});

export const selectCounters = (deviceId, processId, counterType, counterName, revision) => (state) => {
  const device = state.devices.find((x) => x.id === deviceId);
  const process = device.processes.find((x) => x.id === processId);
  return process.counters?.filter((x) => x.type === counterType && x.name === counterName && x.id > revision);
};

export const selectDevices = () => (state) => {
  return state.devices;
};

export const selectProcess = (deviceId, processId) => (state) => {
  const device = state.devices?.find((p) => p.id === deviceId);
  const process = device?.processes.find((p) => p.id === processId);
  return process;
};

export const { setStatus, setDevices, addDevice, addProcess, addCounterNames, updateCounters } = devicesSlice.actions;
export default devicesSlice.reducer;

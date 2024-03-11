import { createSlice } from "@reduxjs/toolkit";

export const uiSlice = createSlice({
  name: "ui",
  initialState: {
    activeDeviceId: undefined,
    activeProcessId: undefined,
    activeConterType: undefined,
  },
  reducers: {
    setActiveDeviceId(state, action) {
      state.activeDeviceId = action.payload;
    },
    setActiveProcessId(state, action) {
      if (state.activeProcessId !== action.payload) state.activeConterType = undefined;

      state.activeProcessId = action.payload;
    },
    setActiveCounterType(state, action) {
      state.activeConterType = action.payload;
    },
  },
  extraReducers(builder) {},
});

export const selectActiveDeviceId = (state) => {
  return state.ui.activeDeviceId;
};

export const selectActiveProcessId = (state) => {
  return state.ui.activeProcessId;
};

export const selectActiveCounterType = (state) => {
  return state.ui.activeConterType;
};

export const { setActiveDeviceId, setActiveProcessId, setActiveCounterType } = uiSlice.actions;
export default uiSlice.reducer;

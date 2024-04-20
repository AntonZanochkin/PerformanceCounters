import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { CounterType, EnumDictionary } from '../../types/CounterType.ts';
import { UiPayloadType } from '../Ui/UiPayloadType.ts';

interface UiSliceState {
  activeDeviceId: number | undefined;
  activeProcessId: number | undefined;
  activeConterType: CounterType | undefined;
};

const initialState: UiSliceState = {
  activeDeviceId: undefined,
  activeProcessId: undefined,
  activeConterType: undefined,
};

export const uiSlice = createSlice({
  name: "ui",
  initialState:initialState,
  reducers: {
    setActiveDeviceId(state, action:PayloadAction<UiPayloadType.SetActiveDeviceId>) {
      state.activeDeviceId = action.payload.activeDeviceId;
    },
    setActiveProcessId(state, action:PayloadAction<UiPayloadType.SetActiveProcessId>) {
      if (state.activeProcessId !== action.payload.activeProcessId) state.activeConterType = undefined;

      state.activeProcessId = action.payload.activeProcessId;
    },
    setActiveCounterType(state, action:PayloadAction<UiPayloadType.SetActiveCounterType>) {
      state.activeConterType = action.payload.activeConterType;
    },
  },
  extraReducers(builder) {},
});

export const selectActiveDeviceId = (state:UiSliceState) => {
  return state.activeDeviceId;
};

export const selectActiveProcessId = (state:UiSliceState) => {
  return state.activeProcessId;
};

export const selectActiveCounterType = (state:UiSliceState) => {
  return state.activeConterType;
};

export const { setActiveDeviceId, setActiveProcessId, setActiveCounterType } = uiSlice.actions;
export default uiSlice.reducer;

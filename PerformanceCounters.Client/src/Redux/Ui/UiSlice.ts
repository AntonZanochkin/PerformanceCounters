import { createSelector, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { CounterType, EnumDictionary } from '../../types/CounterType.ts';
import { RootState } from "../Store.ts";

export interface UiState {
  activeDeviceId: number | undefined;
  activeProcessId: number | undefined;
  activeConterType: CounterType | undefined;
};

const initialState: UiState = {
  activeDeviceId: undefined,
  activeProcessId: undefined,
  activeConterType: undefined,
};

export const UiSlice = createSlice({
  name: "ui",
  initialState:initialState,
  reducers: {
    setActiveDeviceId(state, action:PayloadAction<number>) {
      state.activeDeviceId = action.payload;
    },
    setActiveProcessId(state, action:PayloadAction<number>) {
      if (state.activeProcessId !== action.payload) state.activeConterType = undefined;

      state.activeProcessId = action.payload;
    },
    setActiveCounterType(state, action:PayloadAction<CounterType>) {
      state.activeConterType = action.payload;
    },
  },
  extraReducers(builder) {},
});

export const { setActiveDeviceId, setActiveProcessId, setActiveCounterType } = UiSlice.actions;
export default UiSlice.reducer;

//Selectors
type Selector<S> = (state: RootState) => S;

export const selectActiveDeviceId = 
  createSelector(
    [(state: RootState) => state.uiState.activeDeviceId], 
    (activeDeviceId) => activeDeviceId
  );

export const selectActiveProcessId = 
  createSelector(
    [(state: RootState) => state.uiState.activeProcessId], 
    (activeProcessId) => activeProcessId
  );

export const selectActiveCounterType = 
  createSelector(
    [(state: RootState) => state.uiState.activeConterType], 
    (activeConterType) => activeConterType
  );
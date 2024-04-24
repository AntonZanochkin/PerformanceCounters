import { configureStore } from "@reduxjs/toolkit";
import DeviceSlice, {DeviceState }from "./Device/DeviceSlice.ts";
import UiSlice, {UiState} from "./Ui/UiSlice.ts";
import SignalRSlice,{SignalRState} from "./SignalR/SignalRSlice.ts";
import { TypedUseSelectorHook, useDispatch, useSelector } from "react-redux";

export interface RootState {
  deviceState: DeviceState;
  uiState:UiState,
  signalRState: SignalRState
};

export const store = configureStore({
  reducer: {
    deviceState: DeviceSlice,
    uiState: UiSlice,
    signalRState: SignalRSlice,
  },
});

export const useAppDispatch: () => typeof store.dispatch = useDispatch;
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;

import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { SignalRPayloadType } from "./SignalRPayloadType.ts";

export interface SignalRSliceState {
  isConnected: boolean;
};

const initialState: SignalRSliceState = {
  isConnected: false,
}

export const signalRSlice = createSlice({
  name: "signalR",
  initialState: initialState,
  reducers: {
    setIsConnected: (state, action:PayloadAction<SignalRPayloadType.SetIsConnected>) => {
      state.isConnected = action.payload.isConnected;
    },
  },
  extraReducers(builder) {},
});

export const selectIsConnected = () => (state:SignalRSliceState): boolean => {
  return state.isConnected;
};

export const { setIsConnected } = signalRSlice.actions;
export default signalRSlice.reducer;

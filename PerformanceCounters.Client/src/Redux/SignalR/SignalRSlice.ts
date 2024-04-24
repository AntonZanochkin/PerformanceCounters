import { createSlice, PayloadAction, createSelector } from "@reduxjs/toolkit";
import { RootState } from "../Store.ts";

export interface SignalRState {
  isConnected: boolean;
};

const initialState: SignalRState = {
  isConnected: false,
}

export const SignalRSlice = createSlice({
  name: "signalR",
  initialState: initialState,
  reducers: {
    setIsConnected: (state, action:PayloadAction<boolean>) => {
      state.isConnected = action.payload;
    },
  },
  extraReducers(builder) {},
});

export const { setIsConnected } = SignalRSlice.actions;
export default SignalRSlice.reducer;

//Selectors

export const selectIsConnected = 
createSelector(
  [(state: RootState) => state.signalRState], 
  (signalRState) => signalRState.isConnected
);
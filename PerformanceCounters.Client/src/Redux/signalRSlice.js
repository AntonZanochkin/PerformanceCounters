import { createSlice } from "@reduxjs/toolkit";

export const signalRSlice = createSlice({
  name: "signalR",
  initialState: {
    isConnected: false,
  },
  reducers: {
    setIsConnected: (state, action) => {
      state.isConnected = action.payload;
    },
  },
  extraReducers(builder) {},
});

export const selectIsConnected = () => (state) => {
  return state.signalR.isConnected;
};

export const { setIsConnected } = signalRSlice.actions;
export default signalRSlice.reducer;

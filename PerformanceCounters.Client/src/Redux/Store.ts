import { configureStore } from "@reduxjs/toolkit";
import devicesSlice from "./Device/DeviceSlice.ts";
import uiSlice from "./Ui/UiSlice.ts";
import signalRSlice from "./SignalR/SignalRSlice.ts";

export default configureStore({
  reducer: {
    devices: devicesSlice,
    ui: uiSlice,
    signalR: signalRSlice,
  },
});

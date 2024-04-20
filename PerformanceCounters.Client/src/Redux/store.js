import { configureStore } from "@reduxjs/toolkit";
import devicesSlice from "./Device/DeviceSlice.ts";
import deviceMiddleware from "./Device/deviceMiddleware.js";
import uiSlice from "./Ui/UiSlice.ts";
import signalRSlice from "./signalRSlice";

export default configureStore({
  reducer: {
    devices: devicesSlice,
    ui: uiSlice,
    signalR: signalRSlice,
  },
});

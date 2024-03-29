import { configureStore } from "@reduxjs/toolkit";
import devicesSlice from "./devicesSlice";
import deviceMiddleware from "./deviceMiddleware";
import uiSlice from "./uiSlice";
import signalRSlice from "./signalRSlice";

export default configureStore({
  reducer: {
    devices: devicesSlice,
    ui: uiSlice,
    signalR: signalRSlice,
  },
});

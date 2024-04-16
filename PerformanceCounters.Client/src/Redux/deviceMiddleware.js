import { Middleware } from "redux";
import { startConnecting } from "./DevicesSlice.ts";
import Connector from "../API/SignalrConnection.ts";

const signalR = require("@microsoft/signalr");
const deviceMiddleware = (store) => (next) => (action) => {
  return next(action);
};

export default deviceMiddleware;

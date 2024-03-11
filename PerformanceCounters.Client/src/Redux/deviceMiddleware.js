import { Middleware } from "redux";
import { startConnecting } from "./devicesSlice";
import Connector from "../API/signalr-connection.tsx";

const signalR = require("@microsoft/signalr");
const deviceMiddleware = (store) => (next) => (action) => {
  return next(action);
};

export default deviceMiddleware;

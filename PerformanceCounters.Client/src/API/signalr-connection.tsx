import * as signalR from "@microsoft/signalr";
import { delay } from "@reduxjs/toolkit/dist/utils";
const URL = process.env.REACT_APP_HUB_ADDRESS;
class Connector {
  private connection: signalR.HubConnection;
  public events: {
    // connect: (onConnect: () => void) => void;
    setDevices: (onReceived: (devices: any) => void) => void;
    addDevice: (onReceived: (device: any) => void) => void;
    addProcess: (onReceived: (deviceId: any, addProcessDto: any) => void) => void;
    addCounterNames: (onReceived: (deviceId: any, processId: any, counterType: any, newCounterNames: any) => void) => void;
    updateCounters: (onReceived: (deviceId: any, processId: any, updateDtoList: any) => void) => void;
  } = { setDevices: () => {}, addDevice: () => {}, addProcess: () => {}, addCounterNames: () => {}, updateCounters: () => {} };
  static instance: Connector;

  constructor() {
    this.connection = new signalR.HubConnectionBuilder().withUrl(URL).withAutomaticReconnect().build();

    this.events.addDevice = (onReceived) => {
      this.connection.on("addDevice", (device) => {
        console.log("addDeviceReceived", device);
        onReceived(device);
      });
    };

    this.events.setDevices = (onReceived) => {
      this.connection.on("setDevices", (devices) => {
        console.log("setDevicesReceived", devices);
        onReceived(devices);
      });
    };

    this.events.addProcess = (onReceived) => {
      this.connection.on("addProcess", (deviceId, addProcessDto) => {
        console.log("addProcessReceived", deviceId, addProcessDto);
        onReceived(deviceId, addProcessDto);
      });
    };

    this.events.addCounterNames = (onReceived) => {
      this.connection.on("addCounterNames", (deviceId, processId, counterType, newCounterNames) => {
        console.log("addCounterNamesReceived", deviceId, processId, counterType, newCounterNames);
        onReceived(deviceId, processId, counterType, newCounterNames);
      });
    };

    this.events.updateCounters = (onReceived) => {
      this.connection.on("updateCounters", (deviceId, processId, updateDtoList) => {
        console.log("updateCounters", deviceId, processId, updateDtoList);
        onReceived(deviceId, processId, updateDtoList);
      });
    };
  }

  public connectStart = (onConnect) => {
    this.connection
      .start()
      .then(() => {
        console.log("onConnect");
        onConnect();
      })
      .catch((err) => document.write(err));
  };

  public sendGetDevices = () => {
    this.connection.send("GetDevices");
  };

  public sendSubscribeCounter = (deviceId, processId, type, conterName, revision) => {
    this.connection.send("SubscribeCounter", deviceId, processId, type, conterName, revision);
  };

  public sendUnsubscribeConuter = (deviceId, processId, type, counterName) => {
    this.connection.send("UnsubscribeCounter", deviceId, processId, type, counterName);
  };

  public static getInstance(): Connector {
    if (!Connector.instance) Connector.instance = new Connector();
    return Connector.instance;
  }
}
export default Connector.getInstance;

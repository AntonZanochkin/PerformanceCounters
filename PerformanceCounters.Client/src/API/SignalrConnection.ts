import * as signalR from "@microsoft/signalr";
import { delay } from "@reduxjs/toolkit/dist/utils";
import { StoreType } from '../types/StoreType.ts';
import { PayloadType } from '../types/PayloadType.ts';
import { DtoType } from '../types/DtoType.ts';

if (process.env.REACT_APP_HUB_ADDRESS === undefined) {
  throw new Error('REACT_APP_HUB_ADDRESS environment variable is not defined');
}

const hubAddress:string = process.env.REACT_APP_HUB_ADDRESS;

class Connector {
  private connection: signalR.HubConnection;
  public events: {
    addDevice: (onReceived:(dto:DtoType.AddDevice) => void) => void; 
    setDevices: (onReceived: (dto: DtoType.SetDevices) => void) => void;
    addProcess: (onReceived: (dto: DtoType.AddProcess) => void) => void;
    addCounterNames: (onReceived: (dto:DtoType.AddCounterNames) => void) => void;
    updateCounters: (onReceived: (dto:DtoType.UpdateCounters) => void) => void;
  } = { addDevice: () => {}, setDevices: () => {}, addProcess: () => {}, addCounterNames: () => {}, updateCounters: () => {} };
  
  static instance: Connector;

  constructor() {
    this.connection = new signalR.HubConnectionBuilder().withUrl(hubAddress).withAutomaticReconnect().build();

    this.events.addDevice = (onReceived) => {
      this.connection.on("addDevice", dto => {
        console.log("addDeviceReceived", dto);
        onReceived(dto);
      });
    };

    this.events.setDevices = (onReceived) => {
      this.connection.on("setDevices", dto => {
        console.log("setDevicesReceived", dto);
        onReceived(dto);
      });
    };

    this.events.addProcess = (onReceived) => {
      this.connection.on("addProcess", dto => {
        console.log("addProcessReceived", dto);
        onReceived(dto);
      });
    };

    this.events.addCounterNames = (onReceived) => {
      this.connection.on("addCounterNames", dto => {
        console.log("addCounterNamesReceived", dto);
        onReceived(dto);
      });
    };

    this.events.updateCounters = (onReceived) => {
      this.connection.on("updateCounters", dto => {
        console.log("updateCounters", dto);
        onReceived(dto);
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

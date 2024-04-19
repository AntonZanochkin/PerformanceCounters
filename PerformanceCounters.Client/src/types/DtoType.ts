import { StoreType } from '../types/StoreType.ts';
import { CounterType, EnumDictionary } from "./CounterType.ts";

export namespace DtoType {
  
    export interface AddDevice {
      id: number;
      name: string;
    } 

    export interface SetDevices {
        devices: Array<SetDevice>;
      }

    export interface SetDevice {
        id: number;
        name: string;
        processes: Array<Process>;
      }

      export interface Process {
        id: number;
        name: string;
        counterNamesByType: EnumDictionary<CounterType, string[]>;
      }

      export interface AddProcess {
        deviceId: number;
        processId: number;
        processName: string;
      }
      
      export interface AddCounterNames {
        deviceId: number;
        processId: number;
        counterType: CounterType;
        newCounterNames: Array<string>;
      }
      
      export interface UpdateCounters {
        deviceId: number;
        processId: number;
        counters: Array<StoreType.Counter>
      }
  }
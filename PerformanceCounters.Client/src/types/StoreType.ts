import { CounterType, EnumDictionary } from "./CounterType.ts";

export namespace StoreType {
   
    export type Device = {
      id: number;
      name: string;
      processes: Process[];
    };
  
    export type Process = {
      id: number;
      name: string;
      counters: Counter[];
      counterNamesByType: EnumDictionary<CounterType, string[]>;
    };
  
    export type Counter = {
      id: number;
      type: CounterType;
      name: string;
      dateTime: string;
      valueJson: string;
      value?:number;
      userTime?:number;
      kernelTime?:number;
      sleepTime?:number;
    };
  }
  
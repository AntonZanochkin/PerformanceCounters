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
      counterNamesByType: CounterNamesByType;
    };
  
    export type Counter = {
      id: number;
      type: string;
      name: string;
      dateTime: string;
      valueJson: string;
    };
  
    export interface CounterNamesByType {
      [counterType: string]: string[];
    }
  }
  
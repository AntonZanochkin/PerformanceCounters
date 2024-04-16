import { StoreType } from '../types/StoreType.ts';
import { DtoType } from '../types/DtoType.ts';

export namespace PayloadType {
  export interface AddDevice extends DtoType.AddDevice {
  }
  
  export interface SetDevices extends DtoType.SetDevices {
  }

  export interface AddProcess extends DtoType.AddProcess {
  }
  
  export interface AddCounterNames extends DtoType.AddProcess {
  }
  
  export interface UpdateCounters extends DtoType. AddProcess{
  }
}
import { DtoType } from '../../types/DtoType.ts';

export namespace DevicePayloadType {
  export interface AddDevice extends DtoType.AddDevice {
  }
  
  export interface SetDevices extends DtoType.SetDevices {
  }

  export interface AddProcess extends DtoType.AddProcess {
  }
  
  export interface AddCounterNames extends DtoType.AddCounterNames {
  }
  
  export interface UpdateCounters extends DtoType.UpdateCounters{
  }
}
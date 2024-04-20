import { CounterType } from '../../types/CounterType.ts';

export namespace UiPayloadType {

  export interface SetActiveDeviceId {
    activeDeviceId: number;
  } 

  export interface SetActiveProcessId {
    activeProcessId: number;
  } 

  export interface SetActiveCounterType {
    activeConterType: CounterType;
  } 
}
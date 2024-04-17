type EnumDictionary<KeyType extends string | symbol | number, Value> = { 
    [Key in KeyType]: Value
  }

export enum CounterType {
    Integer = "Integer",
    Stopwatch = "Stopwatch",
    CpuTime = "CpuTime",
  }
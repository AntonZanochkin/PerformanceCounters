import React, { useState, useEffect, useRef, useMemo } from "react";
import { VegaLite } from "react-vega";
import { useSelector, useDispatch } from "react-redux";
import { selectCounters } from "../../../Redux/Device/DeviceSlice.ts";
import { selectIsConnected } from "../../../Redux/signalRSlice.js";
import Connector from "../../../API/SignalrConnection.ts";
import { integerSpec, stopwatchSpec, cpuTimeSpec, vegaLiteSpec } from "./VegaLiteSpec.js";
import { CounterType } from "../../../types/CounterType.ts";

type Props = {
  deviceId: number;
  processId: number;
  type: CounterType;
  counterName: string;
};

export const Chart = ({ deviceId, processId, type, counterName }: Props) => {
  const connector = Connector();
  const [width, setWidth] = useState<number>(0);

  const vegaLiteSpecMemo = useMemo(() => {
    let spec;

    switch (type) {
      case CounterType.Integer: {
        spec = integerSpec();
        break;
      }
      case CounterType.Stopwatch: {
        spec = stopwatchSpec();
        break;
      }
      case CounterType.CpuTime: {
        spec = cpuTimeSpec();
        break;
      }
    }

    return spec;
  }, [vegaLiteSpec]);

  const componentRef = useRef<HTMLDivElement>(null);

  const dispatch = useDispatch();

  const isConnected = useSelector(selectIsConnected());
  
  const countersStateRef = useRef<{ counters: ViewCounter[], revision: number }>({ counters: [], revision: -1 });
  const countersStore = useSelector(selectCounters(deviceId, processId, type, counterName, countersStateRef.current.revision));
  const revision = countersStore && countersStore.length > 0 ? Math.max(...countersStore.map((c) => c.id)) : -1;

  type ViewCounter = {
    id: number;
    type: CounterType;
    name: string;
    dateTime: string;
    value:number;
    timeType?:string;
  };

  //countersStore.filter
  useEffect(() => {
    if (revision === -1) return;

    const newCounters = countersStore.map((c) => ({ ...c }));

    console.log(type);
    switch (type) {
      case CounterType.Stopwatch:
      case CounterType.Integer: {
        newCounters.map((c) => {
          countersStateRef.current.counters.push({ id:c.id, type:c.type, name:c.name, dateTime:c.dateTime, value:c.value ?? 0});
        });
        break;
      }
      case CounterType.CpuTime: {
        newCounters.map((c) => {
          countersStateRef.current.counters.push({ id:c.id, type:c.type, name:c.name, dateTime:c.dateTime, value:(c.userTime ?? 0) + (c.kernelTime ?? 0), timeType: "Cpu"});
          countersStateRef.current.counters.push({ id:c.id, type:c.type, name:c.name, dateTime:c.dateTime, value:c.sleepTime ?? 0, timeType: "Sleep"});
        });
        break;
      }
    }

    countersStateRef.current.revision = revision;
    console.log(countersStateRef.current.counters);
  }, [revision]);

  //Subscribe
  useEffect(() => {
    if (isConnected) connector.sendSubscribeCounter(deviceId, processId, type, counterName, revision);

    return () => {
      if (isConnected) connector.sendUnsubscribeConuter(deviceId, processId, type, counterName);
    };
  }, [isConnected]);

  //Width
  useEffect(() => {
    const getWidth = ():number => {
      if (componentRef.current) {
        const width = componentRef.current.clientWidth;
        return width;
      }

      return 0;
    };

    const width = getWidth();
    setWidth(width);

    const handleResize = () => {
      const width = getWidth();
      setWidth(width);
    };

    window.addEventListener("resize", handleResize);

    return () => {
      window.removeEventListener("resize", handleResize);
    };
  }, []);

  vegaLiteSpecMemo.vconcat[0].width = width - 220;
  vegaLiteSpecMemo.vconcat[1].width = width - 220;

  //selectedState
  const selectedState = useRef({ selectedValue: undefined });

  const handleSelection = (name, value) => {
    if (name !== "brush") return;
    selectedState.current.selectedValue = value;
  };

  if (selectedState.current.selectedValue) {
    vegaLiteSpecMemo.vconcat[1].params[0].value = selectedState.current.selectedValue;
  }
  useEffect(() => {}, []);

  const actions = {
    export: true,
    source: false,
    compiled: false,
    editor: false,
  };

  return (
    <div ref={componentRef}>
      <VegaLite
        spec={vegaLiteSpecMemo}
        data={{ counterData: countersStateRef.current.counters }}
        actions={actions}
        signalListeners={{
          brush: (name, value) => handleSelection(name, value),
        }}
      ></VegaLite>
    </div>
  );
};

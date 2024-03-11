import React, { useState, useEffect, useRef, useMemo } from "react";
import { VegaLite } from "react-vega";
import { useSelector, useDispatch } from "react-redux";
// import { GetIntegerCountersByName } from "../../../API/apiManager";
import { selectCounters as selectCounters } from "../../../Redux/devicesSlice.js";
import { selectIsConnected } from "../../../Redux/signalRSlice.js";
import Connector from "../../../API/signalr-connection.tsx";
import { integerSpec, stopwatchSpec, cpuTimeSpec, vegaLiteSpec } from "./VegaLiteSpec.js";

export const Chart = ({ deviceId, processId, type, counterName }) => {
  const connector = Connector();
  const [width, setWidth] = useState([]);

  const vegaLiteSpecMemo = useMemo(() => {
    let spec;

    switch (type) {
      case "Integer": {
        spec = integerSpec();
        break;
      }
      case "Stopwatch": {
        spec = stopwatchSpec();
        break;
      }
      case "CpuTime": {
        spec = cpuTimeSpec();
        break;
      }
    }

    console.log("spec", spec);
    return spec;
  }, [vegaLiteSpec]);

  const componentRef = useRef(null);

  const dispatch = useDispatch();

  const isConnected = useSelector(selectIsConnected());

  const countersStateRef = useRef({ counters: [], revision: -1 });
  const countersStore = useSelector(selectCounters(deviceId, processId, type, counterName, countersStateRef.current.revision));
  const revision = countersStore && countersStore.length > 0 ? Math.max(...countersStore.map((c) => c.id)) : -1;

  //countersStore.filter
  useEffect(() => {
    if (revision === -1) return;

    const newCounters = countersStore.map((c) => ({ ...c }));

    switch (type) {
      case "Stopwatch":
      case "Integer": {
        newCounters.map((c) => {
          countersStateRef.current.counters.push({ ...c });
        });
        break;
      }
      case "CpuTime": {
        newCounters.map((c) => {
          countersStateRef.current.counters.push({ ...c, value: c.userTime + c.kernelTime, timeType: "Cpu" });
          countersStateRef.current.counters.push({ ...c, value: c.sleepTime, timeType: "Sleep" });
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
    const getWidth = () => {
      if (componentRef.current) {
        const width = componentRef.current.clientWidth;
        return width;
      }
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

import React, { useCallback } from "react";
import "./Statistics.css";
import { useLocation } from "react-router-dom";
import { selectActiveDeviceId, setActiveDeviceId, selectActiveProcessId, setActiveProcessId, selectActiveCounterType, setActiveCounterType } from "../../Redux/Ui/UiSlice.ts";
import { useDispatch } from "react-redux";
import { selectDeviceAndProcessOrDefault } from "../../Redux/Device/DeviceSlice.ts"
import { Counter } from "./Counter/Counter.tsx";
import { useAppSelector } from "../../Redux/Store.ts";

export const Statistics = () => {
  const location = useLocation();
  const searchParams = new URLSearchParams(location.search);

  const deviceIdString = searchParams.get("deviceId");
  const processIdString = searchParams.get("processId");

  if(deviceIdString === null || processIdString === null)
    throw new Error("Missing deviceId or processId");
  
  const deviceId = parseInt(deviceIdString, 10);
  const processId = parseInt(processIdString, 10);

  const dispatch = useDispatch();
  if (useAppSelector(selectActiveDeviceId) === undefined) dispatch(setActiveDeviceId(deviceId));
  if (useAppSelector(selectActiveProcessId) === undefined) dispatch(setActiveProcessId(processId));

  const activeCounterType = useAppSelector(selectActiveCounterType);
  const {device, process} = useAppSelector(selectDeviceAndProcessOrDefault(deviceId, processId));

  const handleItemClick = useCallback(
    (type) => {
      dispatch(setActiveCounterType(type));
    },
    [dispatch]
  );

  return (
    <div className="statistics-div">
      <div className="navigation-panel">
        <ul className="ul">
          {process &&
            Object.entries(process.counterNamesByType).map(([type, names]) => (
              <li key={type} className={activeCounterType === type ? "li selected" : "li"} onClick={() => handleItemClick(type)}>
                {type}
                <span className={"badge-counter "}>{names.length}</span>
              </li>
            ))}
        </ul>
      </div>
      <div>{process && activeCounterType && <Counter deviceId={deviceId} processId={processId} type={activeCounterType} counterNames={process.counterNamesByType[activeCounterType]} />}</div>
    </div>
  );
};

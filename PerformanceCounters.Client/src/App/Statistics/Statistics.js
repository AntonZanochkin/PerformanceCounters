import React, { useState, useEffect, useCallback } from "react";
import "./Statistics.css";
import { useLocation } from "react-router-dom";
import { selectActiveDeviceId, setActiveDeviceId, selectActiveProcessId, setActiveProcessId, selectActiveCounterType, setActiveCounterType } from "../../Redux/uiSlice";
import { useSelector, useDispatch } from "react-redux";
import { selectProcess } from "../../Redux/DevicesSlice.ts";
import { Counter } from "./Counter/Counter.js";

export const Statistics = () => {
  const location = useLocation();
  const searchParams = new URLSearchParams(location.search);
  const deviceIdString = searchParams.get("deviceId");
  const processIdString = searchParams.get("processId");

  const deviceId = parseInt(deviceIdString, 10);
  const processId = parseInt(processIdString, 10);

  const dispatch = useDispatch();
  const activeCounterType = useSelector(selectActiveCounterType);

  if (useSelector(selectActiveDeviceId) === undefined) dispatch(setActiveDeviceId(deviceId));
  if (useSelector(selectActiveProcessId) === undefined) dispatch(setActiveProcessId(processId));

  const processStore = useSelector(selectProcess(deviceId, processId));

  const handleItemClick = useCallback(
    (type) => {
      dispatch(setActiveCounterType(type));
    },
    [dispatch]
  );

  return (
    <div className="statisticsDiv">
      <div className="navigation-panel">
        <ul className="ul">
          {processStore &&
            Object.entries(processStore.counterNamesByType).map(([type, names]) => (
              <li key={type} className={activeCounterType === type ? "li selected" : "li"} onClick={() => handleItemClick(type)}>
                {type}
                <span className={"badge-counter "}>{names.length}</span>
              </li>
            ))}
        </ul>
      </div>
      <div>{activeCounterType && <Counter deviceId={deviceId} processId={processId} type={activeCounterType} counterNames={processStore.counterNamesByType[activeCounterType]} />}</div>
    </div>
  );
};

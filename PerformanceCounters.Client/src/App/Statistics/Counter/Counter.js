import React, { useState, useEffect } from "react";
import Collapsible from "../../Collapsible/Collapsible.js";
import "./Counter.css";
import { Chart } from "../Chart/Chart.js";

export const Counter = ({ deviceId, processId, type, counterNames }) => {
  return (
    <div className="counterDiv">
      {counterNames.map((name) => {
        return (
          <Collapsible key={deviceId + processId + name} label={name}>
            <Chart deviceId={deviceId} processId={processId} type={type} counterName={name} />
          </Collapsible>
        );
      })}
    </div>
  );
};

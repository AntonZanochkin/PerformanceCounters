import React, { useState, useEffect } from "react";
import Collapsible from "../../Collapsible/Collapsible.tsx";
import "./Counter.css";
import { Chart } from "../Chart/Chart.tsx";
import { CounterType } from "../../../types/CounterType.ts";

type Props = {
  deviceId: number;
  processId: number;
  type: CounterType;
  counterNames: string[];
};

export const Counter = ({ deviceId, processId, type, counterNames }: Props) => {
  return (
    <div className="counter-div">
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

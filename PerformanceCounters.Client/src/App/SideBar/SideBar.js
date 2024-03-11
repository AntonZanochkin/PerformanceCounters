import React, { useState, useEffect } from "react";
import "./SideBar.css";
import { useSelector, useDispatch } from "react-redux";
import { selectDevices, setDevices, addDevice, addProcess, addCounterNames, updateCounters } from "../../Redux/devicesSlice";
import { selectIsConnected, setIsConnected } from "../../Redux/signalRSlice.js";
import DropdownMenu from "./DropdownMenu";
import Connector from "../../API/signalr-connection.tsx";

export const SideBar = () => {
  const connector = Connector();

  const dispatch = useDispatch();
  const devicesStore = useSelector(selectDevices());
  const isConnected = useSelector(selectIsConnected());

  useEffect(() => {
    connector.events.setDevices((devices) => dispatch(setDevices(devices)));
    connector.events.addDevice((device) => dispatch(addDevice(device)));
    connector.events.addProcess((deviceId, addProcessDto) => dispatch(addProcess({ deviceId, addProcessDto })));
    connector.events.addCounterNames((deviceId, processId, counterType, newCounterNames) => dispatch(addCounterNames({ deviceId, processId, counterType, newCounterNames })));
    connector.events.updateCounters((deviceId, processId, updateDtoList) => dispatch(updateCounters({ deviceId, processId, updateDtoList })));

    connector.connectStart(() => {
      dispatch(setIsConnected(true));
    });
  }, [connector]);

  useEffect(() => {
    if (isConnected) connector.sendGetDevices();
  }, [isConnected]);

  const renderHeaderLi = () => (
    <li className="header-menu">
      <span>Devices</span>
    </li>
  );

  const renderDevices = () => {
    return devicesStore.map((device) => <DropdownMenu device={device} key={device.id} />);
  };

  return (
    <nav id="sidebar" className="sidebar-nav default-theme bg1 sidebar-bg">
      <div className="sidebar-content">
        <div className=" sidebar-item sidebar-menu">
          <ul>
            {renderHeaderLi()}
            {renderDevices()}
          </ul>
        </div>
      </div>
    </nav>
  );
};

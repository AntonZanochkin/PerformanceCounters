import React, { useEffect } from "react";
import "./SideBar.css";
import { useDispatch } from "react-redux";
import { selectDevices, setDevices, addDevice, addProcess, addCounterNames, updateCounters } from "../../Redux/Device/DeviceSlice.ts";
import { selectIsConnected, setIsConnected } from "../../Redux/SignalR/SignalRSlice.ts";
import DropdownMenu from "./DropdownMenu.tsx";
import Connector from "../../API/SignalrConnection.ts";
import { StoreType } from '../../types/StoreType.ts';
import { useAppSelector } from '../../Redux/Store.ts';

export const SideBar = () => {
  const connector = Connector();

  const dispatch = useDispatch();
  const devices = useAppSelector(selectDevices);
  const isConnected = useAppSelector(selectIsConnected);

  useEffect(() => {
    connector.events.addDevice(dto => dispatch(addDevice(dto)));
    connector.events.setDevices(dto => dispatch(setDevices(dto)));
    connector.events.addProcess(dto => dispatch(addProcess(dto)));
    connector.events.addCounterNames(dto => dispatch(addCounterNames(dto)));
    connector.events.updateCounters(dto => dispatch(updateCounters(dto)));

    connector.startConnect(() => {
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

  const renderDevices = (devices : StoreType.Device[] ) => {
    return devices.map((device) => <DropdownMenu device={device} key={device.id} />);
  };

  return (
    <nav id="sidebar" className="sidebar-nav default-theme bg1 sidebar-bg">
      <div className="sidebar-content">
        <div className=" sidebar-item sidebar-menu">
          <ul>
            {renderHeaderLi()}
            {renderDevices(devices)}
          </ul>
        </div>
      </div>
    </nav>
  );
};

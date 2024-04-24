import React from "react";
import { animated, useSpring } from "react-spring";
import { Spring } from "react-spring/renderprops";
import { Link } from "react-router-dom";
import { useDispatch } from "react-redux";
import { selectActiveDeviceId, setActiveDeviceId, selectActiveProcessId, setActiveProcessId } from "../../Redux/Ui/UiSlice.ts";
import { StoreType } from '../../types/StoreType.ts';
import { useAppSelector } from "../../Redux/Store.ts";

type Props = {
  device:StoreType.Device
};

function DropdownMenu(props:Props) {
  const device = props.device;

  const closedStyle = {
    height: 0,
  };

  const openStyle = {
    height: "auto",
  };

  const activeDeviceId = useAppSelector(selectActiveDeviceId);
  const ativeProcessId = useAppSelector(selectActiveProcessId);
  const IsActiveDropDownMenu = activeDeviceId === device.id;

  const dispatch = useDispatch();
  const handleMenuDropDownClick = (e) => {
    dispatch(setActiveDeviceId(activeDeviceId == device.id ? -1 : device.id));
  };

  const handleLinkClick = (processId:number) => {
    dispatch(setActiveProcessId(processId));
  };

  const menuDropDownContent = (
    <a
      // href="#s"
      onClick={(e:React.MouseEvent<HTMLAnchorElement, MouseEvent>) => {
        handleMenuDropDownClick(e);
      }}
    >
      <i className="fa fa-tachometer-alt"></i>
      <span className="menu-text">{device.name}</span>
      {<span className={"badge-device"}> {device.processes ? device.processes.length : 0} </span>}
    </a>
  );

  const renderProcessesItems = () => {
    if (!device.processes || !device.processes.length) return;

    return device.processes.map((process, index) => {
      var typesCount = process.counterNamesByType ? Object.entries(process.counterNamesByType).length : 0;

      return (
        <li key={process.id}>
          <Link
            className={ativeProcessId === process.id ? "selected" : ""}
            to={`/Statistics?deviceId=${encodeURIComponent(device.id)}&processId=${encodeURIComponent(process.id)}`}
            onClick={() => handleLinkClick(process.id)}
          >
            {" "}
            {process.name}
            {<span className={"badge-process"}> {typesCount} </span>}
          </Link>
        </li>
      );
    });
  };

  const subMenuContent = (
    <Spring from={openStyle} to={IsActiveDropDownMenu ? openStyle : closedStyle}>
      {(props) => (
        <animated.div className="sidebar-submenu" style={props}>
          <ul> {renderProcessesItems()} </ul>
        </animated.div>
      )}
    </Spring>
  );

  return (
    <li className={IsActiveDropDownMenu ? "sidebar-dropdown active" : "sidebar-dropdown"}>
      {" "}
      {menuDropDownContent}
      {subMenuContent}{" "}
    </li>
  );
}

export default DropdownMenu;

import React, { useState, useRef } from "react";
import "./Collapsible.css";

type Props = {
  label: string;
  children:React.ReactNode
};

const Collapsible = (props:Props) => {
  const [open, setOpen] = useState<boolean>(false);

  const toggle = ():void => {
    setOpen(!open);
  };

  return (
    <div>
      <button className="toggle" onClick={toggle}>
        {props.label}
      </button>
      <div className={open ? "content-show" : "content-parent"}>
        <div className="content"> {open && props.children} </div>
      </div>
    </div>
  );
};

export default Collapsible;

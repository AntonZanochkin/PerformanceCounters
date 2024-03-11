import React, { useState, useRef } from "react";
import "./Collapsible.css";

const Collapsible = (props) => {
  const [open, setOpen] = useState(false);
  const contentRef = useRef();

  const toggle = () => {
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

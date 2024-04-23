import React from "react";
import "./App.css";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { Statistics } from "./Statistics/Statistics.js";
import { SideBar } from "./SideBar/SideBar.tsx";
import { Provider } from "react-redux";
import  Store from "../Redux/Store.ts";

export const App = () => {
  return (
    <div className="app">
      <Provider store={Store}>
        <Router>
          <SideBar />
          <Routes>
            <Route path="/Statistics" element={<Statistics />} />
          </Routes>
        </Router>
      </Provider>
    </div>
  );
};

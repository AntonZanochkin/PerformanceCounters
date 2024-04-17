import React from "react";
import "./App.css";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { Statistics } from "./Statistics/Statistics";
import { SideBar } from "./SideBar/SideBar.tsx";
import { Provider } from "react-redux";
import store from "../Redux/store";

export const App = () => {
  return (
    <div className="app">
      <Provider store={store}>
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

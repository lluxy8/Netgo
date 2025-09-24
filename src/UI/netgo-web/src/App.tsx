"use client";

import { Outlet } from "react-router-dom";
import Navbar from "./components/Navbar";
import Layout from "./components/Layout";

function App() {
  
  return (
    <>
      <Navbar /> 
      <Layout>      
        <Outlet />
      </Layout>
    </>
  )
}

export default App

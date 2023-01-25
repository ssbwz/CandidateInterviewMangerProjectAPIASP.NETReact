
// import './App.css';
import "./styles/style.css";
import React, { useState } from "react";
import { AuthContext } from "./auth/Auth";
import 'bootstrap/dist/css/bootstrap.min.css';
import { Route, Routes, BrowserRouter as Router } from "react-router-dom";
import Navbar from "./components/navbar/Navbar";
import CreateAppointment from "./pages/CreateAppointment";
import VacancyOverviewPage from './pages/VacancyOverviewPage';
import CandidateOverviewPage from './pages/CandidateOverviewPage.';
import AppointmentPage from './pages/AppointmentPage';
import Appointment from "./appointment/OverviewAppointmentPage"
import Login from './pages/LoginPage';
import PrivateRoute from "./PrivateRoute";
<link href="//db.onlinewebfonts.com/c/dff56d27e2d0710a764d0f2d4fe2bc03?family=Roihu" rel="stylesheet" type="text/css" />;
<link href="//db.onlinewebfonts.com/c/840e46f8bc6d4e0072efe9a68fe8ad3c?family=Ratio+ Thin" rel="stylesheet" type="text/css" />;


function App() {

  const [authTokens, setAuthTokens] = useState(
    localStorage.getItem("tokens") || ""
  );

  const setTokens = (data) => {
    localStorage.setItem("tokens", JSON.stringify(data));
    setAuthTokens(data);
  };
  console.log("authTokens", authTokens);

  return (
    <div className="dialog-off-canvas-main-canvas" data-off-canvas-main-canvas>
      <div className="layout">
        <AuthContext.Provider value={{ authTokens, setAuthTokens: setTokens }}>

          <Router>
            <Navbar />

            <Routes>
              <Route element={<PrivateRoute allowedRoles={['Admin', 'Recruiter']} />}>
                <Route path="/Candidates/:vacancyId" element={<CandidateOverviewPage />} />
                <Route path="/singleappointment/:appointmentId" element={<AppointmentPage />} />
                <Route path="/appointments" element={<Appointment />} />
                <Route path="/" element={<VacancyOverviewPage />} />

              </Route>


              <Route path="/login" element={<Login />} />
              <Route path="appointment">
                <Route path="create/:usernameHashed/:identifierHashed" element={<CreateAppointment />}></Route>
              </Route>
            </Routes>
          </Router>
        </AuthContext.Provider>

      </div>
    </div>
  );
}

export default App;
// File: src/App.jsx
// Description: The final root component with all public and protected routes.

import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';

// Import Components
import Navbar from './components/Navbar.jsx';
import Footer from './components/Footer.jsx';
import ProtectedRoute from './components/ProtectedRoute.jsx';

// Import Pages
import HomePage from './pages/HomePage.jsx';
import LoginPage from './pages/LoginPage.jsx';
import RegisterDonorPage from './pages/RegisterDonorPage.jsx';
import RegisterHospitalPage from './pages/RegisterHospitalPage.jsx';
import AdminDashboard from './pages/AdminDashboard.jsx';
import DonorDashboard from './pages/DonorDashboard.jsx';
import HospitalDashboard from './pages/HospitalDashboard.jsx';
import AboutUsPage from './pages/AboutUsPage.jsx';
import ContactUsPage from './pages/ContactUsPage.jsx'; // Import the new page

function App() {
  return (
    <Router>
      <ToastContainer 
        position="top-right"
        autoClose={5000}
        hideProgressBar={false}
        newestOnTop={false}
        closeOnClick
        rtl={false}
        pauseOnFocusLoss
        draggable
        pauseOnHover
        theme="colored"
      />
      
      <div className="d-flex flex-column min-vh-100">
        <Navbar />
        
        <main className="flex-grow-1">
          <Routes>
            {/* --- Public Routes --- */}
            <Route path="/" element={<HomePage />} />
            <Route path="/login" element={<LoginPage />} />
            <Route path="/register-donor" element={<RegisterDonorPage />} />
            <Route path="/register-hospital" element={<RegisterHospitalPage />} />
            <Route path="/about" element={<AboutUsPage />} />
            <Route path="/contact" element={<ContactUsPage />} /> {/* Add the new route */}

            {/* --- Protected Routes --- */}
            <Route element={<ProtectedRoute requiredRole="ROLE_ADMIN" />}>
              <Route path="/admin-dashboard" element={<AdminDashboard />} />
            </Route>
            <Route element={<ProtectedRoute requiredRole="ROLE_DONOR" />}>
              <Route path="/donor-dashboard" element={<DonorDashboard />} />
            </Route>
            <Route element={<ProtectedRoute requiredRole="ROLE_HOSPITAL" />}>
              <Route path="/hospital-dashboard" element={<HospitalDashboard />} />
            </Route>
          </Routes>
        </main>
        
        <Footer />
      </div>
    </Router>
  );
}

export default App;
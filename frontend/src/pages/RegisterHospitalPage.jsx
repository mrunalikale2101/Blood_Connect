// File: src/pages/RegisterHospitalPage.jsx
// Description: The registration page for new hospitals, now with state management and API connection.

import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import authService from '../services/authService'; // Import our auth service
import '../assets/styles/RegisterPage.css'; 

const RegisterHospitalPage = () => {
  const [formData, setFormData] = useState({
    hospitalName: '',
    email: '',
    password: '',
    address: '',
    licenseNumber: '',
    contactPerson: ''
  });
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();

  // Handle input changes and update the state
  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.id]: e.target.value
    });
  };

  // Handle form submission
  const handleRegister = async (e) => {
    e.preventDefault();
    setIsLoading(true);

    // --- Regex Validation ---
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(formData.email)) {
      toast.error("Please enter a valid email address.");
      setIsLoading(false);
      return;
    }

    if (formData.password.length < 6) {
      toast.error("Password must be at least 6 characters long.");
      setIsLoading(false);
      return;
    }

    try {
      // Call the registerHospital function from our authService
      const response = await authService.registerHospital(formData);
      
      toast.success(response.data); // The backend sends a success message
      
      // Redirect to the login page after a short delay
      setTimeout(() => {
        navigate('/login');
      }, 2000);

    } catch (error) {
      const errorMessage = error.response?.data?.message || "Registration failed. Please try again.";
      toast.error(errorMessage);
      setIsLoading(false);
    }
  };

  return (
    <div className="register-container">
      <div className="container">
        <div className="row justify-content-center">
          <div className="col-md-8 col-lg-7">
            <div className="card shadow-lg border-0">
              <div className="card-body p-5">
                <h2 className="card-title text-center mb-4">Create a Hospital Account</h2>
                <form onSubmit={handleRegister}>
                  <div className="mb-3">
                    <label htmlFor="hospitalName" className="form-label">Hospital Name</label>
                    <input type="text" className="form-control" id="hospitalName" value={formData.hospitalName} onChange={handleChange} required />
                  </div>
                  <div className="mb-3">
                    <label htmlFor="email" className="form-label">Email Address</label>
                    <input type="email" className="form-control" id="email" value={formData.email} onChange={handleChange} required />
                  </div>
                   <div className="mb-3">
                    <label htmlFor="password" className="form-label">Password</label>
                    <input type="password" className="form-control" id="password" value={formData.password} onChange={handleChange} required />
                  </div>
                  <div className="mb-3">
                    <label htmlFor="address" className="form-label">Address</label>
                    <textarea className="form-control" id="address" rows="3" value={formData.address} onChange={handleChange} required></textarea>
                  </div>
                  <div className="row">
                    <div className="col-md-6 mb-3">
                      <label htmlFor="licenseNumber" className="form-label">License Number</label>
                      <input type="text" className="form-control" id="licenseNumber" value={formData.licenseNumber} onChange={handleChange} required />
                    </div>
                    <div className="col-md-6 mb-3">
                      <label htmlFor="contactPerson" className="form-label">Contact Person</label>
                      <input type="text" className="form-control" id="contactPerson" value={formData.contactPerson} onChange={handleChange} required />
                    </div>
                  </div>
                  <div className="d-grid mt-3">
                    <button type="submit" className="btn btn-danger btn-lg" disabled={isLoading}>
                      {isLoading ? 'Registering...' : 'Register'}
                    </button>
                  </div>
                </form>
                <p className="text-center mt-3">
                  Already have an account? <Link to="/login">Login Here</Link>
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default RegisterHospitalPage;
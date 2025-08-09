// File: src/pages/RegisterDonorPage.jsx
// Description: The registration page for new donors, now with state management and API connection.

import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import authService from '../services/authService'; // Import our auth service
import '../assets/styles/RegisterPage.css'; 

const RegisterDonorPage = () => {
  const [formData, setFormData] = useState({
    fullName: '',
    email: '',
    password: '',
    bloodGroup: '',
    contactNumber: ''
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

    // Password must be at least 6 characters
    if (formData.password.length < 6) {
      toast.error("Password must be at least 6 characters long.");
      setIsLoading(false);
      return;
    }

    try {
      // Call the registerDonor function from our authService
      const response = await authService.registerDonor(formData);
      
      // Show success message
      toast.success(response.data); // The backend sends a success message
      
      // Redirect to the login page after a short delay
      setTimeout(() => {
        navigate('/login');
      }, 2000);

    } catch (error) {
      // Show error message from the backend
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
                <h2 className="card-title text-center mb-4">Create a Donor Account</h2>
                <form onSubmit={handleRegister}>
                  <div className="mb-3">
                    <label htmlFor="fullName" className="form-label">Full Name</label>
                    <input type="text" className="form-control" id="fullName" value={formData.fullName} onChange={handleChange} required />
                  </div>
                  <div className="mb-3">
                    <label htmlFor="email" className="form-label">Email Address</label>
                    <input type="email" className="form-control" id="email" value={formData.email} onChange={handleChange} required />
                  </div>
                  <div className="mb-3">
                    <label htmlFor="password" className="form-label">Password</label>
                    <input type="password" className="form-control" id="password" value={formData.password} onChange={handleChange} required />
                  </div>
                  <div className="row">
                    <div className="col-md-6 mb-3">
                      <label htmlFor="bloodGroup" className="form-label">Blood Group</label>
                      <select id="bloodGroup" className="form-select" value={formData.bloodGroup} onChange={handleChange} required>
                        <option value="">Select...</option>
                        <option value="A+">A+</option>
                        <option value="A-">A-</option>
                        <option value="B+">B+</option>
                        <option value="B-">B-</option>
                        <option value="AB+">AB+</option>
                        <option value="AB-">AB-</option>
                        <option value="O+">O+</option>
                        <option value="O-">O-</option>
                      </select>
                    </div>
                    <div className="col-md-6 mb-3">
                      <label htmlFor="contactNumber" className="form-label">Contact Number</label>
                      <input type="text" className="form-control" id="contactNumber" value={formData.contactNumber} onChange={handleChange} required />
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

export default RegisterDonorPage;
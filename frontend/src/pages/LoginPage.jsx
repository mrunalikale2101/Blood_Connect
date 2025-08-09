// File: src/pages/LoginPage.jsx
// Description: The common login page, now with smarter role detection.

import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import authService from '../services/authService';
import '../assets/styles/LoginPage.css';

const LoginPage = () => {
  const [formData, setFormData] = useState({
    email: '',
    password: ''
  });
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.id]: e.target.value });
  };

  const handleLogin = async (e) => {
    e.preventDefault();
    setIsLoading(true);

    try {
      const userData = await authService.login(formData);
      
      toast.success('Login Successful! Redirecting...');

      // --- THIS IS THE CORRECTED LOGIC ---
      // It checks for the role in both possible locations
      const userRole = userData.userDetails?.user?.role?.roleName || userData.userDetails?.role?.roleName;

      setTimeout(() => {
        if (userRole === 'ROLE_ADMIN') {
          navigate('/admin-dashboard');
        } else if (userRole === 'ROLE_DONOR') {
          navigate('/donor-dashboard');
        } else if (userRole === 'ROLE_HOSPITAL') {
          navigate('/hospital-dashboard');
        } else {
          toast.error("Could not determine user role. Logging out.");
          authService.logout();
          navigate('/');
        }
      }, 1500);

    } catch (error) {
      const errorMessage = error.response?.data?.message || "Login failed. Please check your credentials.";
      toast.error(errorMessage);
      setIsLoading(false);
    }
  };

  return (
    <div className="login-container">
      <div className="container">
        <div className="row justify-content-center">
          <div className="col-md-6 col-lg-5">
            <div className="card shadow-lg border-0">
              <div className="card-body p-5">
                <h2 className="card-title text-center mb-4">Welcome Back</h2>
                <form onSubmit={handleLogin}>
                  <div className="mb-3">
                    <label htmlFor="email" className="form-label">Email address</label>
                    <input
                      type="email"
                      className="form-control"
                      id="email"
                      placeholder="Enter your email"
                      value={formData.email}
                      onChange={handleChange}
                      required
                    />
                  </div>
                  <div className="mb-3">
                    <label htmlFor="password" className="form-label">Password</label>
                    <input
                      type="password"
                      className="form-control"
                      id="password"
                      placeholder="Enter your password"
                      value={formData.password}
                      onChange={handleChange}
                      required
                    />
                  </div>
                  <div className="d-grid mt-4">
                    <button type="submit" className="btn btn-danger btn-lg" disabled={isLoading}>
                      {isLoading ? 'Logging In...' : 'Login'}
                    </button>
                  </div>
                </form>
                <p className="text-center mt-3">
                  Don't have an account? <Link to="/register-donor">Register Here</Link>
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default LoginPage;
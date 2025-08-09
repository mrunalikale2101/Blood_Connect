// File: src/components/Navbar.jsx
// Description: A responsive navbar that now shows Logout when a user is logged in.

import React from 'react';
import { Link, NavLink, useNavigate } from 'react-router-dom';
import authService from '../services/authService'; // Import our auth service

const Navbar = () => {
  const navigate = useNavigate();
  const currentUser = authService.getCurrentUser();

  const handleLogout = () => {
    authService.logout();
    navigate('/login'); // Redirect to login page after logout
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-danger shadow-sm">
      <div className="container">
        <Link className="navbar-brand" to="/">
          🩸BloodConnect
        </Link>
        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarNav"
          aria-controls="navbarNav"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="navbarNav">
          <ul className="navbar-nav ms-auto">
            <li className="nav-item">
              <NavLink className="nav-link" to="/">Home</NavLink>
            </li>
            
            {/* --- Conditional Rendering Logic --- */}
            {currentUser ? (
              // If user IS logged in, show their email and a Logout button
              <>
                <li className="nav-item">
                  <span className="nav-link">Welcome, {currentUser.userDetails?.user?.email || currentUser.userDetails?.email}</span>
                </li>
                <li className="nav-item">
                  <button className="btn btn-light ms-lg-2 mt-2 mt-lg-0" onClick={handleLogout}>
                    Logout
                  </button>
                </li>
              </>
            ) : (
              // If user is NOT logged in, show Register and Login
              <>
                <li className="nav-item">
                  <NavLink className="nav-link" to="/about">About Us</NavLink>
                </li>
                <li className="nav-item">
                  <NavLink className="nav-link" to="/contact">Contact Us</NavLink>
                </li>
                <li className="nav-item dropdown">
                  <a
                    className="nav-link dropdown-toggle"
                    href="#"
                    id="navbarDropdown"
                    role="button"
                    data-bs-toggle="dropdown"
                    aria-expanded="false"
                  >
                    Register
                  </a>
                  <ul className="dropdown-menu" aria-labelledby="navbarDropdown">
                    <li>
                      <Link className="dropdown-item" to="/register-donor">As a Donor</Link>
                    </li>
                    <li>
                      <Link className="dropdown-item" to="/register-hospital">As a Hospital</Link>
                    </li>
                  </ul>
                </li>
                <li className="nav-item">
                  <Link className="btn btn-light ms-lg-2 mt-2 mt-lg-0" to="/login">Login</Link>
                </li>
              </>
            )}
          </ul>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
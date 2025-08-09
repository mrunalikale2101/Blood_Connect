// File: src/components/ProtectedRoute.jsx
// Description: This component protects routes that require authentication and a specific role.

import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';
import authService from '../services/authService';

const ProtectedRoute = ({ requiredRole }) => {
  const currentUser = authService.getCurrentUser();

  // 1. Check if the user is logged in
  if (!currentUser) {
    return <Navigate to="/login" />;
  }

  // --- THIS IS THE CORRECTED LOGIC ---
  // It now checks for the role in both possible locations to correctly identify the Admin
  const userRole = currentUser.userDetails?.user?.role?.roleName || currentUser.userDetails?.role?.roleName;

  // 3. Check if the user's role matches the required role for this route
  if (requiredRole && userRole !== requiredRole) {
    // If roles don't match, redirect
    return <Navigate to="/login" />;
  }

  // 4. If all checks pass, render the dashboard page
  return <Outlet />;
};

export default ProtectedRoute;
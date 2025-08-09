// File: src/services/authService.js
// Description: Contains functions for all authentication-related API calls and session management.

import api from './api';

// --- API Call Functions ---

const registerDonor = (donorData) => {
  return api.post('/auth/register/donor', donorData);
};

const registerHospital = (hospitalData) => {
  return api.post('/auth/register/hospital', hospitalData);
};

const login = async (credentials) => {
  const response = await api.post('/auth/login', credentials);
  if (response.data.accessToken) {
    // If the login is successful and we get a token, save the user data
    localStorage.setItem('user', JSON.stringify(response.data));
  }
  return response.data;
};

// --- Session Management Functions ---

const logout = () => {
  localStorage.removeItem('user');
};

const getCurrentUser = () => {
  return JSON.parse(localStorage.getItem('user'));
};

export default {
  registerDonor,
  registerHospital,
  login,
  logout,
  getCurrentUser,
};
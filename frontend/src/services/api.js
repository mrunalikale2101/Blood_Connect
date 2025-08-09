// File: src/services/api.js
// Description: Centralized Axios instance with interceptors for requests and responses.

import axios from 'axios';
import authService from './authService';

// Create an instance of Axios with a base URL
const api = axios.create({
  baseURL: 'http://localhost:8080/api',
});

// --- Request Interceptor ---
// This runs before each request is sent.
api.interceptors.request.use(
  (config) => {
    const user = authService.getCurrentUser();
    if (user && user.accessToken) {
      config.headers['Authorization'] = `Bearer ${user.accessToken}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// --- NEW: Response Interceptor ---
// This runs after a response is received.
api.interceptors.response.use(
  (response) => {
    // If the response is successful, just return it
    return response;
  },
  (error) => {
    // Check if the error is a 401 Unauthorized error
    if (error.response && error.response.status === 401) {
      // Check if the user was previously logged in
      if (authService.getCurrentUser()) {
        console.log("Token expired or invalid. Logging out.");
        authService.logout();
        // Reload the page to reset the application state and redirect to login
        window.location.reload();
      }
    }
    // For all other errors, just pass them along
    return Promise.reject(error);
  }
);

export default api;
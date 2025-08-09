// File: src/services/hospitalService.js
// Description: Contains functions for all Hospital-related API calls.

import api from './api';

// Function to create a new blood request
// It takes an object like { bloodGroup: 'O-', units: 5, urgency: 'HIGH' }
const createBloodRequest = (requestData) => {
  return api.post('/hospital/request', requestData);
};

// Function to get the logged-in hospital's request history
const getMyBloodRequests = () => {
  return api.get('/hospital/requests');
};

export default {
  createBloodRequest,
  getMyBloodRequests,
};
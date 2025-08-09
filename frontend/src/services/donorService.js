// File: src/services/donorService.js
// Description: Contains functions for all Donor-related API calls.

import api from './api';

// --- Existing Functions ---
const getMyProfile = () => {
  return api.get('/donor/profile');
};

const bookAppointment = (appointmentData) => {
  return api.post('/donor/appointment', appointmentData);
};

const getMyDonationHistory = () => {
  return api.get('/donor/donations/history');
};

// --- NEW FUNCTIONS ---

// Function to get the logged-in donor's scheduled appointments
const getMyScheduledAppointments = () => {
  return api.get('/donor/appointments/scheduled');
};

// Function to cancel a specific appointment
const cancelAppointment = (appointmentId) => {
  return api.patch(`/donor/appointment/${appointmentId}/cancel`);
};


export default {
  getMyProfile,
  bookAppointment,
  getMyDonationHistory,
  getMyScheduledAppointments, // Export the new function
  cancelAppointment,          // Export the new function
};
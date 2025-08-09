// File: src/services/adminService.js
// Description: Contains functions for all Admin-related API calls.

import api from './api';

// --- Dashboard Stats ---
const getDashboardStats = () => {
  return api.get('/admin/dashboard/stats');
};

// --- Inventory Management ---
const getInventory = () => {
  return api.get('/admin/inventory');
};

const updateInventory = (inventoryData) => {
  return api.put('/admin/inventory', inventoryData);
};

// --- Request Management ---
const getPendingRequests = () => {
  return api.get('/admin/requests/pending');
};

const updateRequestStatus = (requestId, newStatus) => {
  return api.patch(`/admin/requests/${requestId}`, { newStatus });
};

// --- Appointment Management ---
const getScheduledAppointments = () => {
  return api.get('/admin/appointments/scheduled');
};

const completeAppointment = (appointmentId) => {
  return api.post(`/admin/appointments/${appointmentId}/complete`);
};

// --- NEW FUNCTIONS for Donor Management ---

// Function to get a list of all donors
const getAllDonors = () => {
  return api.get('/admin/donors');
};

// Function to update a donor's eligibility
const updateDonorEligibility = (userId, isEligible) => {
  return api.patch(`/admin/donors/${userId}/eligibility`, { eligible: isEligible });
};


export default {
  getDashboardStats,
  getInventory,
  updateInventory,
  getPendingRequests,
  updateRequestStatus,
  getScheduledAppointments,
  completeAppointment,
  getAllDonors,             // Export the new function
  updateDonorEligibility,   // Export the new function
};
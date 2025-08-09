// File: src/services/publicService.js
// Description: Contains functions for public-facing API calls, like the contact form.

import api from './api';

// Function to submit the contact form data
const submitContactForm = (formData) => {
  return api.post('/public/contact', formData);
};

export default {
  submitContactForm,
};
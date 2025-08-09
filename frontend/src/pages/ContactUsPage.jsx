// File: src/pages/ContactUsPage.jsx
// Description: The page displaying contact information and a fully functional contact form.

import React, { useState } from 'react';
import { toast } from 'react-toastify';
import publicService from '../services/publicService'; // Import our new service
import '../assets/styles/ContactUsPage.css';

const ContactUsPage = () => {
  // State to manage the form fields
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    message: ''
  });
  const [isLoading, setIsLoading] = useState(false);

  // Handler for input changes
  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.id]: e.target.value });
  };

  // Handler for form submission
  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);
    try {
      // Call the API function from our publicService
      const response = await publicService.submitContactForm(formData);
      
      // Show the success message from the backend
      toast.success(response.data);
      
      // Clear the form after successful submission
      setFormData({ name: '', email: '', message: '' });
    } catch (error) {
      const errorMessage = error.response?.data?.message || "Sorry, we couldn't send your message. Please try again later.";
      toast.error(errorMessage);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="contact-us-page">
      {/* Header Section */}
      <header className="contact-header text-white text-center d-flex align-items-center justify-content-center">
        <div className="container">
          <h1 className="display-4 fw-bold">Get In Touch</h1>
          <p className="lead">We'd love to hear from you. Here's how you can reach us.</p>
        </div>
      </header>

      {/* Contact Info and Form Section */}
      <section className="container my-5">
        <div className="row g-5">
          {/* Left Column: Contact Details */}
          <div className="col-lg-5">
            <h2 className="mb-4">Contact Information</h2>
            <div className="contact-info-item">
              <i className="bi bi-geo-alt-fill"></i>
              <div>
                <h5>Address</h5>
                <p>123 Blood Bank Lane, Navi Mumbai, Maharashtra, 400703, India</p>
              </div>
            </div>
            <div className="contact-info-item">
              <i className="bi bi-envelope-fill"></i>
              <div>
                <h5>Email Us</h5>
                <p>contact@bloodconnect.com</p>
              </div>
            </div>
            <div className="contact-info-item">
              <i className="bi bi-telephone-fill"></i>
              <div>
                <h5>Call Us</h5>
                <p>+91 98765 43210</p>
              </div>
            </div>
          </div>

          {/* Right Column: Contact Form */}
          <div className="col-lg-7">
            <div className="card shadow-sm border-0 p-4">
              <h2 className="mb-4">Send Us a Message</h2>
              <form onSubmit={handleSubmit}>
                <div className="mb-3">
                  <label htmlFor="name" className="form-label">Your Name</label>
                  <input type="text" className="form-control" id="name" value={formData.name} onChange={handleChange} required />
                </div>
                <div className="mb-3">
                  <label htmlFor="email" className="form-label">Your Email</label>
                  <input type="email" className="form-control" id="email" value={formData.email} onChange={handleChange} required />
                </div>
                <div className="mb-3">
                  <label htmlFor="message" className="form-label">Message</label>
                  <textarea className="form-control" id="message" rows="5" value={formData.message} onChange={handleChange} required></textarea>
                </div>
                <button type="submit" className="btn btn-danger btn-lg" disabled={isLoading}>
                  {isLoading ? 'Sending...' : 'Send Message'}
                </button>
              </form>
            </div>
          </div>
        </div>
      </section>
    </div>
  );
};

export default ContactUsPage;
// File: src/pages/HomePage.jsx
// Description: The main landing page of the application.

import React from 'react';
import { Link } from 'react-router-dom';
import '../assets/styles/HomePage.css'; // We will create this CSS file next

const HomePage = () => {
  return (
    <div className="home-page">
      {/* Hero Section */}
      <header className="hero-section text-white text-center d-flex align-items-center justify-content-center">
        <div className="container">
          <h1 className="display-3 fw-bold">Donate Blood, Save Lives</h1>
          <p className="lead my-3">Every drop counts. Join our community of heroes and make a difference today.</p>
          <div>
            <Link to="/register-donor" className="btn btn-light btn-lg me-2">Become a Donor</Link>
            <Link to="/login" className="btn btn-outline-light btn-lg">Request Blood</Link>
          </div>
        </div>
      </header>

      {/* Why Donate Section */}
      <section className="py-5">
        <div className="container">
          <h2 className="text-center mb-4">Why Your Donation Matters</h2>
          <div className="row text-center g-4">
            <div className="col-md-4">
              <div className="card h-100 shadow-sm border-0">
                <div className="card-body p-4">
                  <h3 className="card-title text-danger">Save Up to 3 Lives</h3>
                  <p className="card-text">A single blood donation can be separated into components, helping multiple patients in need.</p>
                </div>
              </div>
            </div>
            <div className="col-md-4">
              <div className="card h-100 shadow-sm border-0">
                <div className="card-body p-4">
                  <h3 className="card-title text-danger">Essential for Treatments</h3>
                  <p className="card-text">Blood is crucial for surgeries, cancer treatments, chronic illnesses, and traumatic injuries.</p>
                </div>
              </div>
            </div>
            <div className="col-md-4">
              <div className="card h-100 shadow-sm border-0">
                <div className="card-body p-4">
                  <h3 className="card-title text-danger">A Gift of Life</h3>
                  <p className="card-text">Blood cannot be manufactured. It must come from generous donors like you.</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>
    </div>
  );
};

export default HomePage;
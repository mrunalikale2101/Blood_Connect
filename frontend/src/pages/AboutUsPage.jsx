// File: src/pages/AboutUsPage.jsx
// Description: The page that describes the project and the team.

import React from 'react';
import '../assets/styles/AboutUsPage.css'; // We will create this CSS file next

const AboutUsPage = () => {
  // Placeholder data for team members
  const teamMembers = [
    { name: 'Developer One', role: 'Backend Lead', image: 'https://placehold.co/300x300/d9534f/white?text=Dev+1', linkedin: '#', instagram: '#' },
    { name: 'Developer Two', role: 'Frontend Lead', image: 'https://placehold.co/300x300/333333/white?text=Dev+2', linkedin: '#', instagram: '#' },
    { name: 'Developer Three', role: 'UI/UX Designer', image: 'https://placehold.co/300x300/d9534f/white?text=Dev+3', linkedin: '#', instagram: '#' },
    { name: 'Developer Four', role: 'Database Admin', image: 'https://placehold.co/300x300/333333/white?text=Dev+4', linkedin: '#', instagram: '#' },
  ];

  return (
    <div className="about-us-page">
      {/* Header Section */}
      <header className="about-header text-white text-center d-flex align-items-center justify-content-center">
        <div className="container">
          <h1 className="display-4 fw-bold">About BloodConnect</h1>
          <p className="lead">Connecting donors with those in need, one drop at a time.</p>
        </div>
      </header>

      {/* Our Mission Section */}
      <section className="container my-5">
        <div className="row">
          <div className="col-lg-8 mx-auto text-center">
            <h2 className="mb-4">Our Mission</h2>
            <p className="lead">
              Our mission is to create a seamless and efficient platform that connects blood donors, hospitals, and administrators. We aim to ensure that blood reaches those in need quickly and reliably, leveraging technology to bridge the gap and save lives.
            </p>
          </div>
        </div>
      </section>

      {/* Meet the Team Section */}
      <section className="bg-light py-5">
        <div className="container">
          <h2 className="text-center mb-5">Meet the Team</h2>
          <div className="row g-4">
            {teamMembers.map((member, index) => (
              <div className="col-lg-3 col-md-6" key={index}>
                <div className="card team-card h-100 text-center shadow-sm border-0">
                  <img src={member.image} className="card-img-top" alt={member.name} />
                  <div className="card-body">
                    <h5 className="card-title">{member.name}</h5>
                    <p className="card-text text-muted">{member.role}</p>
                    <div className="social-links">
                      <a href={member.linkedin} className="social-icon" target="_blank" rel="noopener noreferrer">
                        <i className="bi bi-linkedin"></i>
                      </a>
                      <a href={member.instagram} className="social-icon" target="_blank" rel="noopener noreferrer">
                        <i className="bi bi-instagram"></i>
                      </a>
                    </div>
                  </div>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>
    </div>
  );
};

export default AboutUsPage;
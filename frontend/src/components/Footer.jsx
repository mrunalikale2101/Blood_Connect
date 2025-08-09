// File: src/components/Footer.jsx
// Description: A simple footer for the application.

import React from 'react';

const Footer = () => {
  const currentYear = new Date().getFullYear();
  return (
    <footer className="bg-dark text-white text-center p-3 mt-auto">
      <div className="container">
        <p className="mb-0">&copy; {currentYear} BloodConnect. All Rights Reserved.</p>
      </div>
    </footer>
  );
};

export default Footer;
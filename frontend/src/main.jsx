// File: src/main.jsx
// Description: This is the main entry point of our application.
// We import Bootstrap and React Toastify CSS here to make them available globally.

import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App.jsx';

// Import Bootstrap CSS first to allow our custom CSS to override it later
import 'bootstrap/dist/css/bootstrap.min.css';
// Import React Toastify CSS for notifications
import 'react-toastify/dist/ReactToastify.css';

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);
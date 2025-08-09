// File: src/pages/DonorDashboard.jsx
// Description: The Donor Dashboard, now with Upcoming Appointments and a Cancel button.

import React, { useState, useEffect } from 'react';
import donorService from '../services/donorService';
import { toast } from 'react-toastify';
import '../assets/styles/Dashboard.css';

const DonorDashboard = () => {
  const [profile, setProfile] = useState(null);
  const [history, setHistory] = useState([]);
  const [scheduledAppointments, setScheduledAppointments] = useState([]); // New state for upcoming appointments
  const [isLoading, setIsLoading] = useState(true);
  const [appointmentDate, setAppointmentDate] = useState('');

  // Function to fetch all data for the dashboard
  const fetchData = async () => {
    setIsLoading(true);
    try {
      const [profileRes, historyRes, appointmentsRes] = await Promise.all([
        donorService.getMyProfile(),
        donorService.getMyDonationHistory(),
        donorService.getMyScheduledAppointments() // Fetch scheduled appointments
      ]);
      setProfile(profileRes.data);
      setHistory(historyRes.data);
      setScheduledAppointments(appointmentsRes.data);
    } catch (error) {
      toast.error("Could not fetch your data. Please try again later.");
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => { fetchData(); }, []);

  const handleBookAppointment = async (e) => {
    e.preventDefault();
    if (!appointmentDate) { toast.error("Please select a date."); return; }
    try {
      await donorService.bookAppointment({ appointmentDate });
      toast.success("Appointment booked successfully! A confirmation email has been sent.");
      setAppointmentDate('');
      fetchData(); // Refresh all data to show the new appointment in the list
    } catch (error) { toast.error(error.response?.data?.message || "Failed to book appointment."); }
  };

  // --- NEW: Handler for cancelling an appointment ---
  const handleCancelAppointment = async (appointmentId) => {
    // Ask for confirmation before cancelling
    if (window.confirm("Are you sure you want to cancel this appointment?")) {
      try {
        await donorService.cancelAppointment(appointmentId);
        toast.success("Appointment cancelled successfully. A confirmation email has been sent.");
        fetchData(); // Refresh all data to remove the appointment from the list
      } catch (error) {
        toast.error(error.response?.data?.message || "Failed to cancel appointment.");
      }
    }
  };

  if (isLoading) {
    return <div className="container mt-5"><p>Loading your dashboard...</p></div>;
  }

  return (
    <div className="dashboard-container">
      <div className="container">
        <h1 className="mb-4">Welcome, {profile?.fullName}!</h1>
        <div className="row g-4">
          <div className="col-lg-4">
            {/* My Profile Card */}
            <div className="card dashboard-card mb-4"><div className="card-header"><h5>My Profile</h5></div>
              <div className="card-body">
                <p><strong>Name:</strong> {profile?.fullName}</p>
                <p><strong>Email:</strong> {profile?.user.email}</p>
                <p><strong>Blood Group:</strong> {profile?.bloodGroup}</p>
                <p><strong>Contact:</strong> {profile?.contactNumber}</p>
                <p><strong>Eligible to Donate:</strong> {profile?.eligible ? 'Yes' : 'No'}</p>
                <p className="mb-0"><strong>Last Donation:</strong> {profile?.lastDonationDate ? new Date(profile.lastDonationDate).toLocaleDateString() : 'N/A'}</p>
              </div>
            </div>
            {/* Book Appointment Card */}
            <div className="card dashboard-card"><div className="card-header"><h5>Book an Appointment</h5></div>
              <div className="card-body">
                <form onSubmit={handleBookAppointment}>
                  <div className="mb-3"><label htmlFor="appointmentDate" className="form-label">Select a Date</label><input type="date" className="form-control" id="appointmentDate" value={appointmentDate} onChange={(e) => setAppointmentDate(e.target.value)} required /></div>
                  <div className="d-grid"><button type="submit" className="btn btn-danger">Book Now</button></div>
                </form>
              </div>
            </div>
          </div>
          <div className="col-lg-8">
            {/* --- NEW: Upcoming Appointments Card --- */}
            <div className="card dashboard-card mb-4">
              <div className="card-header"><h5>Upcoming Appointments</h5></div>
              <div className="card-body p-0">
                {scheduledAppointments.length > 0 ? (
                  <table className="table table-striped table-hover mb-0">
                    <thead className="table-dark"><tr><th>Appointment Date</th><th>Status</th><th>Actions</th></tr></thead>
                    <tbody>{scheduledAppointments.map((app) => (
                      <tr key={app.appointmentId}>
                        <td>{new Date(app.appointmentDate).toLocaleDateString()}</td>
                        <td><span className="badge bg-info">{app.status}</span></td>
                        <td><button className="btn btn-sm btn-warning" onClick={() => handleCancelAppointment(app.appointmentId)}>Cancel</button></td>
                      </tr>
                    ))}</tbody>
                  </table>
                ) : (<p className="text-center p-3 mb-0">You have no upcoming appointments.</p>)}
              </div>
            </div>
            
            {/* Donation History Card */}
            <div className="card dashboard-card"><div className="card-header"><h5>My Donation History</h5></div>
              <div className="card-body p-0">
                {history.length > 0 ? (
                  <table className="table table-striped table-hover mb-0">
                    <thead className="table-dark"><tr><th>Record ID</th><th>Donation Date</th><th>Units Donated</th></tr></thead>
                    <tbody>{history.map((record) => (<tr key={record.recordId}><td>{record.recordId}</td><td>{new Date(record.donationDate).toLocaleDateString()}</td><td>{record.unitsDonated}</td></tr>))}</tbody>
                  </table>
                ) : (<p className="text-center p-3 mb-0">You have no donation history yet.</p>)}
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default DonorDashboard;
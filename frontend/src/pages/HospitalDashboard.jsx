// File: src/pages/HospitalDashboard.jsx
// Description: The main dashboard for the Hospital, with the new custom theme.

import React, { useState, useEffect } from 'react';
import hospitalService from '../services/hospitalService';
import { toast } from 'react-toastify';
import '../assets/styles/Dashboard.css'; // Import the new dashboard styles

const HospitalDashboard = () => {
  const [requests, setRequests] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [formData, setFormData] = useState({ bloodGroup: '', units: '', urgency: 'MEDIUM' });

  const fetchRequestHistory = async () => {
    setIsLoading(true);
    try { const response = await hospitalService.getMyBloodRequests(); setRequests(response.data); } 
    catch (error) { toast.error("Could not fetch your request history."); } 
    finally { setIsLoading(false); }
  };

  useEffect(() => { fetchRequestHistory(); }, []);

  const handleChange = (e) => { setFormData({ ...formData, [e.target.id]: e.target.value }); };
  const handleCreateRequest = async (e) => {
    e.preventDefault();
    if (!formData.bloodGroup || !formData.units) { toast.error("Please fill out all fields."); return; }
    try {
      await hospitalService.createBloodRequest({ ...formData, units: parseInt(formData.units, 10) });
      toast.success("Blood request submitted successfully!");
      setFormData({ bloodGroup: '', units: '', urgency: 'MEDIUM' }); fetchRequestHistory(); 
    } catch (error) { toast.error(error.response?.data?.message || "Failed to submit request."); }
  };
  const getStatusBadge = (status) => {
    switch (status) { case 'APPROVED': return 'bg-success'; case 'REJECTED': return 'bg-danger'; case 'PENDING': return 'bg-warning text-dark'; default: return 'bg-secondary'; }
  };

  return (
    <div className="dashboard-container">
      <div className="container">
        <h1 className="mb-4">Hospital Dashboard</h1>
        <div className="row g-4">
          <div className="col-lg-4">
            <div className="card dashboard-card"><div className="card-header"><h5>Request Blood</h5></div>
              <div className="card-body">
                <form onSubmit={handleCreateRequest}>
                  <div className="mb-3"><label htmlFor="bloodGroup" className="form-label">Blood Group</label><select id="bloodGroup" className="form-select" value={formData.bloodGroup} onChange={handleChange} required><option value="">Select...</option><option value="A+">A+</option><option value="A-">A-</option><option value="B+">B+</option><option value="B-">B-</option><option value="AB+">AB+</option><option value="AB-">AB-</option><option value="O+">O+</option><option value="O-">O-</option></select></div>
                  <div className="mb-3"><label htmlFor="units" className="form-label">Units Required</label><input type="number" className="form-control" id="units" value={formData.units} onChange={handleChange} min="1" required /></div>
                  <div className="mb-3"><label htmlFor="urgency" className="form-label">Urgency</label><select id="urgency" className="form-select" value={formData.urgency} onChange={handleChange} required><option value="LOW">Low</option><option value="MEDIUM">Medium</option><option value="HIGH">High</option></select></div>
                  <div className="d-grid"><button type="submit" className="btn btn-danger">Submit Request</button></div>
                </form>
              </div>
            </div>
          </div>
          <div className="col-lg-8">
            <div className="card dashboard-card"><div className="card-header"><h5>My Request History</h5></div>
              <div className="card-body p-0">
                {isLoading ? (<p className="p-3">Loading history...</p>) : requests.length > 0 ? (
                  <table className="table table-striped table-hover mb-0">
                    <thead className="table-dark"><tr><th>ID</th><th>Blood Group</th><th>Units</th><th>Date</th><th>Status</th></tr></thead>
                    <tbody>{requests.map((req) => (<tr key={req.requestId}><td>{req.requestId}</td><td>{req.bloodGroup}</td><td>{req.unitsRequested}</td><td>{new Date(req.createdAt).toLocaleDateString()}</td><td><span className={`badge ${getStatusBadge(req.status)}`}>{req.status}</span></td></tr>))}</tbody>
                  </table>
                ) : (<p className="text-center p-3 mb-0">You have not made any requests yet.</p>)}
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default HospitalDashboard;
// File: src/pages/AdminDashboard.jsx
// Description: The complete dashboard for the Admin, with the new custom theme.

import React, { useState, useEffect } from 'react';
import adminService from '../services/adminService';
import { toast } from 'react-toastify';
import { Modal, Button, Form } from 'react-bootstrap';
import '../assets/styles/Dashboard.css'; // Import the new dashboard styles

const AdminDashboard = () => {
  const [stats, setStats] = useState(null);
  const [inventory, setInventory] = useState([]);
  const [pendingRequests, setPendingRequests] = useState([]);
  const [scheduledAppointments, setScheduledAppointments] = useState([]);
  const [donors, setDonors] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [currentItem, setCurrentItem] = useState(null);
  const [newUnits, setNewUnits] = useState("");

  const fetchData = async () => {
    setIsLoading(true);
    try {
      const [statsRes, inventoryRes, requestsRes, appointmentsRes, donorsRes] = await Promise.all([
        adminService.getDashboardStats(), adminService.getInventory(), adminService.getPendingRequests(),
        adminService.getScheduledAppointments(), adminService.getAllDonors()
      ]);
      setStats(statsRes.data); setInventory(inventoryRes.data); setPendingRequests(requestsRes.data);
      setScheduledAppointments(appointmentsRes.data); setDonors(donorsRes.data);
    } catch (error) { toast.error("Could not fetch all dashboard data."); } 
    finally { setIsLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  const handleShowModal = (item) => { setCurrentItem(item); setNewUnits(item.units); setShowModal(true); };
  const handleCloseModal = () => setShowModal(false);
  const handleUpdateInventory = async () => {
    if (!currentItem || newUnits === "" || isNaN(newUnits) || newUnits < 0) { toast.error("Please enter a valid number of units."); return; }
    try {
      await adminService.updateInventory({ bloodGroup: currentItem.bloodGroup, units: parseInt(newUnits, 10) });
      toast.success(`Inventory updated!`); handleCloseModal(); fetchData();
    } catch (error) { toast.error("Failed to update inventory."); }
  };
  const handleRequestUpdate = async (requestId, newStatus) => {
    try { await adminService.updateRequestStatus(requestId, newStatus); toast.success(`Request #${requestId} has been ${newStatus.toLowerCase()}.`); fetchData(); } 
    catch (error) { toast.error(error.response?.data?.message || `Failed to update request.`); }
  };
  const handleCompleteAppointment = async (appointmentId) => {
    try { await adminService.completeAppointment(appointmentId); toast.success(`Appointment #${appointmentId} complete.`); fetchData(); } 
    catch (error) { toast.error(error.response?.data?.message || `Failed to complete appointment.`); }
  };
  const handleEligibilityChange = async (donorId, currentEligibility) => {
    try { await adminService.updateDonorEligibility(donorId, !currentEligibility); toast.success(`Donor #${donorId} eligibility updated.`); fetchData(); } 
    catch (error) { toast.error(error.response?.data?.message || `Failed to update eligibility.`); }
  };

  if (isLoading) {
    return <div className="container mt-5 text-center"><h2>Loading Admin Dashboard...</h2></div>;
  }

  return (
    <div className="dashboard-container">
      <div className="container">
        <h1 className="mb-4">Admin Dashboard</h1>
        
        <div className="row g-4 mb-5">
          <div className="col-md-6 col-lg-3"><div className="stat-card"><div className="stat-card-icon"><i className="bi bi-people-fill"></i></div><div className="stat-card-title">Total Donors</div><div className="stat-card-value">{stats?.totalDonors}</div></div></div>
          <div className="col-md-6 col-lg-3"><div className="stat-card"><div className="stat-card-icon"><i className="bi bi-hospital-fill"></i></div><div className="stat-card-title">Total Hospitals</div><div className="stat-card-value">{stats?.totalHospitals}</div></div></div>
          <div className="col-md-6 col-lg-3"><div className="stat-card"><div className="stat-card-icon"><i className="bi bi-hourglass-split"></i></div><div className="stat-card-title">Pending Requests</div><div className="stat-card-value">{stats?.totalPendingRequests}</div></div></div>
          <div className="col-md-6 col-lg-3"><div className="stat-card"><div className="stat-card-icon"><i className="bi bi-droplet-half"></i></div><div className="stat-card-title">Total Blood Units</div><div className="stat-card-value">{stats?.totalBloodUnits}</div></div></div>
        </div>

        <div className="card dashboard-card"><div className="card-header"><h5>Blood Inventory Management</h5></div><div className="card-body p-0"><table className="table table-striped table-hover mb-0">
          <thead className="table-dark"><tr><th>Blood Group</th><th>Units Available</th><th>Last Updated</th><th>Actions</th></tr></thead>
          <tbody>{inventory.map((item) => (<tr key={item.inventoryId}><td>{item.bloodGroup}</td><td>{item.units}</td><td>{new Date(item.updatedAt).toLocaleString()}</td><td><button className="btn btn-sm btn-primary" onClick={() => handleShowModal(item)}>Update</button></td></tr>))}</tbody>
        </table></div></div>

        <div className="card dashboard-card"><div className="card-header"><h5>Pending Blood Requests</h5></div><div className="card-body p-0">
          {pendingRequests.length > 0 ? (<table className="table table-striped table-hover mb-0">
            <thead className="table-dark"><tr><th>ID</th><th>Hospital</th><th>Blood Group</th><th>Units</th><th>Urgency</th><th>Date</th><th>Actions</th></tr></thead>
            <tbody>{pendingRequests.map((req) => (<tr key={req.requestId}><td>{req.requestId}</td><td>{req.hospital.email}</td><td>{req.bloodGroup}</td><td>{req.unitsRequested}</td><td>{req.urgency}</td><td>{new Date(req.createdAt).toLocaleDateString()}</td><td><button className="btn btn-sm btn-success me-2" onClick={() => handleRequestUpdate(req.requestId, 'APPROVED')}>Approve</button><button className="btn btn-sm btn-danger" onClick={() => handleRequestUpdate(req.requestId, 'REJECTED')}>Reject</button></td></tr>))}</tbody>
          </table>) : (<p className="text-center p-3 mb-0">No pending blood requests at this time.</p>)}
        </div></div>

        <div className="card dashboard-card"><div className="card-header"><h5>Scheduled Donation Appointments</h5></div><div className="card-body p-0">
          {scheduledAppointments.length > 0 ? (<table className="table table-striped table-hover mb-0">
            <thead className="table-dark"><tr><th>ID</th><th>Donor Name</th><th>Email</th><th>Appointment Date</th><th>Actions</th></tr></thead>
            <tbody>{scheduledAppointments.map((app) => (<tr key={app.appointmentId}><td>{app.appointmentId}</td><td>{app.donor.fullName}</td><td>{app.donor.email}</td><td>{new Date(app.appointmentDate).toLocaleDateString()}</td><td><button className="btn btn-sm btn-info" onClick={() => handleCompleteAppointment(app.appointmentId)}>Mark as Complete</button></td></tr>))}</tbody>
          </table>) : (<p className="text-center p-3 mb-0">No scheduled appointments.</p>)}
        </div></div>
        
        <div className="card dashboard-card"><div className="card-header"><h5>Donor Management</h5></div><div className="card-body p-0">
          {donors.length > 0 ? (<table className="table table-striped table-hover mb-0">
            <thead className="table-dark"><tr><th>ID</th><th>Name</th><th>Email</th><th>Blood Group</th><th>Eligible</th><th>Actions</th></tr></thead>
            <tbody>{donors.map((donor) => (<tr key={donor.userId}>
              <td>{donor.userId}</td><td>{donor.fullName}</td><td>{donor.email}</td><td>{donor.bloodGroup}</td><td>{donor.eligible ? 'Yes' : 'No'}</td>
              <td><button className={`btn btn-sm ${donor.eligible ? 'btn-warning' : 'btn-success'}`} onClick={() => handleEligibilityChange(donor.userId, donor.eligible)}>{donor.eligible ? 'Mark Ineligible' : 'Mark Eligible'}</button></td>
            </tr>))}</tbody>
          </table>) : (<p className="text-center p-3 mb-0">No donors found.</p>)}
        </div></div>

        <Modal show={showModal} onHide={handleCloseModal} centered>
          <Modal.Header closeButton><Modal.Title>Update Inventory for {currentItem?.bloodGroup}</Modal.Title></Modal.Header>
          <Modal.Body><Form><Form.Group className="mb-3"><Form.Label>New Unit Count</Form.Label><Form.Control type="number" placeholder="Enter new total units" value={newUnits} onChange={(e) => setNewUnits(e.target.value)} autoFocus /></Form.Group></Form></Modal.Body>
          <Modal.Footer><Button variant="secondary" onClick={handleCloseModal}>Close</Button><Button variant="primary" onClick={handleUpdateInventory}>Save Changes</Button></Modal.Footer>
        </Modal>
      </div>
    </div>
  );
};

export default AdminDashboard;
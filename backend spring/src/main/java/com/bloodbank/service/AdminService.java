package com.bloodbank.service;

import com.bloodbank.dto.request.BloodInventoryUpdateRequest;
import com.bloodbank.dto.response.BloodRequestDto;
import com.bloodbank.dto.response.DashboardStatsDto;
import com.bloodbank.dto.response.DonationAppointmentDto;
import com.bloodbank.dto.response.DonationRecordDto;
import com.bloodbank.dto.response.DonorInfoResponseDto;
import com.bloodbank.entity.BloodInventory;
import com.bloodbank.entity.DonationAppointment;
import java.util.List;

public interface AdminService {
    // Inventory
    List<BloodInventory> getAllBloodInventory();
    BloodInventory updateBloodInventory(BloodInventoryUpdateRequest request);
    
    // Blood Requests
    List<BloodRequestDto> getAllPendingBloodRequests();
    BloodRequestDto updateBloodRequestStatus(Long requestId, String newStatus);
    
    // Donor Management
    List<DonorInfoResponseDto> getAllDonors();
    DonorInfoResponseDto updateDonorEligibility(Long donorUserId, boolean isEligible);
    
    // Appointment Management
    List<DonationAppointmentDto> getAllScheduledAppointments();
    DonationRecordDto completeDonationAppointment(Long appointmentId);
    
    // Dashboard
    DashboardStatsDto getDashboardStats();
}
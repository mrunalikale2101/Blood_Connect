package com.bloodbank.controller;

import com.bloodbank.dto.request.BloodInventoryUpdateRequest;
import com.bloodbank.dto.request.BloodRequestUpdateStatusRequest;
import com.bloodbank.dto.request.DonorEligibilityUpdateRequest;
import com.bloodbank.dto.response.BloodRequestDto;
import com.bloodbank.dto.response.DashboardStatsDto;
import com.bloodbank.dto.response.DonationAppointmentDto;
import com.bloodbank.dto.response.DonationRecordDto;
import com.bloodbank.dto.response.DonorInfoResponseDto;
import com.bloodbank.entity.BloodInventory;
import com.bloodbank.service.AdminService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/admin")
@PreAuthorize("hasRole('ADMIN')")
public class AdminController {

    @Autowired
    private AdminService adminService;

    // --- Inventory Endpoints ---
    @GetMapping("/inventory")
    public ResponseEntity<List<BloodInventory>> getAllInventory() {
        return ResponseEntity.ok(adminService.getAllBloodInventory());
    }

    @PutMapping("/inventory")
    public ResponseEntity<BloodInventory> updateInventory(@RequestBody BloodInventoryUpdateRequest request) {
        return ResponseEntity.ok(adminService.updateBloodInventory(request));
    }

    // --- Blood Request Endpoints ---
    @GetMapping("/requests/pending")
    public ResponseEntity<List<BloodRequestDto>> getPendingRequests() {
        return ResponseEntity.ok(adminService.getAllPendingBloodRequests());
    }

    @PatchMapping("/requests/{requestId}")
    public ResponseEntity<BloodRequestDto> updateRequestStatus(
            @PathVariable Long requestId,
            @RequestBody BloodRequestUpdateStatusRequest request) {
        BloodRequestDto updatedRequest = adminService.updateBloodRequestStatus(requestId, request.getNewStatus());
        return ResponseEntity.ok(updatedRequest);
    }

    // --- Donor Management Endpoints ---
    @GetMapping("/donors")
    public ResponseEntity<List<DonorInfoResponseDto>> getAllDonors() {
        return ResponseEntity.ok(adminService.getAllDonors());
    }

    @PatchMapping("/donors/{userId}/eligibility")
    public ResponseEntity<DonorInfoResponseDto> updateDonorEligibility(
            @PathVariable Long userId,
            @RequestBody DonorEligibilityUpdateRequest request) {
        DonorInfoResponseDto updatedDonor = adminService.updateDonorEligibility(userId, request.isEligible());
        return ResponseEntity.ok(updatedDonor);
    }

    // --- Appointment Management Endpoints ---
    @GetMapping("/appointments/scheduled")
    public ResponseEntity<List<DonationAppointmentDto>> getScheduledAppointments() {
        return ResponseEntity.ok(adminService.getAllScheduledAppointments());
    }

    @PostMapping("/appointments/{appointmentId}/complete")
    public ResponseEntity<DonationRecordDto> completeAppointment(@PathVariable Long appointmentId) {
        DonationRecordDto donationRecordDto = adminService.completeDonationAppointment(appointmentId);
        return new ResponseEntity<>(donationRecordDto, HttpStatus.CREATED);
    }
    
 // Add this new method to the AdminController class
    @GetMapping("/dashboard/stats")
    public ResponseEntity<DashboardStatsDto> getDashboardStats() {
        return ResponseEntity.ok(adminService.getDashboardStats());
    }
}
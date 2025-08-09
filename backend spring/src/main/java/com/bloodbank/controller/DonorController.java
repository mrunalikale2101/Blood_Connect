// File: src/main/java/com/bloodbank/controller/DonorController.java

package com.bloodbank.controller;

import com.bloodbank.dto.request.AppointmentCreateRequest;
import com.bloodbank.dto.response.DonationRecordDto;
import com.bloodbank.entity.DonationAppointment;
import com.bloodbank.entity.DonorProfile;
import com.bloodbank.service.DonorService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/donor")
@PreAuthorize("hasRole('DONOR')")
public class DonorController {

    @Autowired
    private DonorService donorService;

    // Existing methods
    @GetMapping("/profile")
    public ResponseEntity<DonorProfile> getMyProfile() {
        DonorProfile donorProfile = donorService.getMyProfile();
        return ResponseEntity.ok(donorProfile);
    }

    @PostMapping("/appointment")
    public ResponseEntity<DonationAppointment> bookAppointment(@RequestBody AppointmentCreateRequest request) {
        DonationAppointment newAppointment = donorService.bookAppointment(request);
        return new ResponseEntity<>(newAppointment, HttpStatus.CREATED);
    }

    @GetMapping("/donations/history")
    public ResponseEntity<List<DonationRecordDto>> getMyDonationHistory() {
        return ResponseEntity.ok(donorService.getMyDonationHistory());
    }

    // --- NEW ENDPOINTS ---

    @PatchMapping("/appointment/{appointmentId}/cancel")
    public ResponseEntity<DonationAppointment> cancelAppointment(@PathVariable Long appointmentId) {
        DonationAppointment cancelledAppointment = donorService.cancelAppointment(appointmentId);
        return ResponseEntity.ok(cancelledAppointment);
    }

    @GetMapping("/appointments/scheduled")
    public ResponseEntity<List<DonationAppointment>> getMyScheduledAppointments() {
        return ResponseEntity.ok(donorService.getMyScheduledAppointments());
    }
}
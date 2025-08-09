// File: src/main/java/com/bloodbank/service/DonorService.java

package com.bloodbank.service;

import com.bloodbank.dto.request.AppointmentCreateRequest;
import com.bloodbank.dto.response.DonationRecordDto;
import com.bloodbank.entity.DonationAppointment;
import com.bloodbank.entity.DonorProfile;

import java.util.List;

public interface DonorService {

    // Existing methods
    DonorProfile getMyProfile();
    List<DonationRecordDto> getMyDonationHistory();
    DonationAppointment bookAppointment(AppointmentCreateRequest request);

    // --- NEW METHODS ---
    
    // Method to cancel an appointment
    DonationAppointment cancelAppointment(Long appointmentId);
    
    // Method to get a list of upcoming (scheduled) appointments
    List<DonationAppointment> getMyScheduledAppointments();
}
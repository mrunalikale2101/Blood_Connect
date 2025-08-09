// File: src/main/java/com/bloodbank/service/impl/DonorServiceImpl.java

package com.bloodbank.service.impl;

import com.bloodbank.dto.request.AppointmentCreateRequest;
import com.bloodbank.dto.response.DonationRecordDto;
import com.bloodbank.dto.response.DonorSimpleDto;
import com.bloodbank.entity.DonationAppointment;
import com.bloodbank.entity.DonationRecord;
import com.bloodbank.entity.DonorProfile;
import com.bloodbank.entity.User;
import com.bloodbank.exception.BadRequestException;
import com.bloodbank.exception.ResourceNotFoundException;
import com.bloodbank.repository.DonationAppointmentRepository;
import com.bloodbank.repository.DonationRecordRepository;
import com.bloodbank.repository.DonorProfileRepository;
import com.bloodbank.repository.UserRepository;
import com.bloodbank.service.DonorService;
import com.bloodbank.service.mail.EmailService; // Import the EmailService
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.time.LocalDate;
import java.time.format.DateTimeFormatter;
import java.util.List;
import java.util.stream.Collectors;

@Service
public class DonorServiceImpl implements DonorService {

    @Autowired
    private DonorProfileRepository donorProfileRepository;
    @Autowired
    private UserRepository userRepository;
    @Autowired
    private DonationAppointmentRepository donationAppointmentRepository;
    @Autowired
    private DonationRecordRepository donationRecordRepository;
    @Autowired
    private EmailService emailService; // Inject the EmailService

    @Override
    public DonorProfile getMyProfile() {
        String currentUserEmail = SecurityContextHolder.getContext().getAuthentication().getName();
        User currentUser = userRepository.findByEmail(currentUserEmail)
                .orElseThrow(() -> new UsernameNotFoundException("User not found with email: " + currentUserEmail));
        return donorProfileRepository.findByUser(currentUser)
                .orElseThrow(() -> new ResourceNotFoundException("DonorProfile", "userId", currentUser.getUserId()));
    }

    @Override
    public List<DonationRecordDto> getMyDonationHistory() {
        String currentUserEmail = SecurityContextHolder.getContext().getAuthentication().getName();
        User currentUser = userRepository.findByEmail(currentUserEmail)
            .orElseThrow(() -> new UsernameNotFoundException("User not found: " + currentUserEmail));
        List<DonationRecord> records = donationRecordRepository.findByDonor_UserIdOrderByDonationDateDesc(currentUser.getUserId());
        DonorProfile donorProfile = donorProfileRepository.findByUser(currentUser)
            .orElseThrow(() -> new ResourceNotFoundException("DonorProfile", "user", currentUser.getUserId()));
        return records.stream()
            .map(record -> convertRecordToDto(record, donorProfile))
            .collect(Collectors.toList());
    }

    @Override
    public DonationAppointment bookAppointment(AppointmentCreateRequest request) {
        String currentUserEmail = SecurityContextHolder.getContext().getAuthentication().getName();
        User currentUser = userRepository.findByEmail(currentUserEmail)
                .orElseThrow(() -> new UsernameNotFoundException("User not found: " + currentUserEmail));

        if (request.getAppointmentDate().isBefore(LocalDate.now())) {
            throw new BadRequestException("Appointment date must be in the future.");
        }
        
        DonationAppointment newAppointment = new DonationAppointment();
        newAppointment.setDonor(currentUser);
        newAppointment.setAppointmentDate(request.getAppointmentDate());
        newAppointment.setStatus("SCHEDULED");
        DonationAppointment savedAppointment = donationAppointmentRepository.save(newAppointment);

        // --- Send Confirmation Email ---
        String subject = "Your Donation Appointment is Confirmed!";
        String formattedDate = savedAppointment.getAppointmentDate().format(DateTimeFormatter.ofPattern("MMMM dd, yyyy"));
        String body = "Dear Donor,\n\n" +
                      "Thank you for scheduling a blood donation appointment. Your appointment is confirmed for:\n\n" +
                      "Date: " + formattedDate + "\n\n" +
                      "We look forward to seeing you. Your contribution is vital.\n\n" +
                      "Best Regards,\nThe BloodConnect Team";
        emailService.sendSimpleMessage(currentUserEmail, subject, body);

        return savedAppointment;
    }

    // --- NEW IMPLEMENTATION ---
    @Override
    @Transactional
    public DonationAppointment cancelAppointment(Long appointmentId) {
        String currentUserEmail = SecurityContextHolder.getContext().getAuthentication().getName();
        User currentUser = userRepository.findByEmail(currentUserEmail)
                .orElseThrow(() -> new UsernameNotFoundException("User not found: " + currentUserEmail));

        DonationAppointment appointment = donationAppointmentRepository.findById(appointmentId)
                .orElseThrow(() -> new ResourceNotFoundException("DonationAppointment", "id", appointmentId));

        if (!appointment.getDonor().getUserId().equals(currentUser.getUserId())) {
            throw new BadRequestException("You are not authorized to cancel this appointment.");
        }
        if (!"SCHEDULED".equalsIgnoreCase(appointment.getStatus())) {
            throw new BadRequestException("This appointment cannot be cancelled.");
        }

        appointment.setStatus("CANCELLED");
        DonationAppointment cancelledAppointment = donationAppointmentRepository.save(appointment);

        // --- Send Cancellation Email ---
        String subject = "Your Donation Appointment has been Cancelled";
        String formattedDate = cancelledAppointment.getAppointmentDate().format(DateTimeFormatter.ofPattern("MMMM dd, yyyy"));
        String body = "Dear Donor,\n\n" +
                      "This email is to confirm that your blood donation appointment scheduled for " + formattedDate + " has been successfully cancelled.\n\n" +
                      "We hope to see you again soon.\n\n" +
                      "Best Regards,\nThe BloodConnect Team";
        emailService.sendSimpleMessage(currentUserEmail, subject, body);

        return cancelledAppointment;
    }

    // --- NEW IMPLEMENTATION ---
    @Override
    public List<DonationAppointment> getMyScheduledAppointments() {
        String currentUserEmail = SecurityContextHolder.getContext().getAuthentication().getName();
        User currentUser = userRepository.findByEmail(currentUserEmail)
            .orElseThrow(() -> new UsernameNotFoundException("User not found: " + currentUserEmail));
        
        return donationAppointmentRepository.findByDonor_UserIdAndStatus(currentUser.getUserId(), "SCHEDULED");
    }
    
    // --- Private helper method (remains the same) ---
    private DonationRecordDto convertRecordToDto(DonationRecord record, DonorProfile donorProfile) {
        DonorSimpleDto donorDto = new DonorSimpleDto();
        donorDto.setUserId(record.getDonor().getUserId());
        donorDto.setEmail(record.getDonor().getEmail());
        donorDto.setFullName(donorProfile.getFullName());

        DonationRecordDto dto = new DonationRecordDto();
        dto.setRecordId(record.getRecordId());
        dto.setDonor(donorDto);
        dto.setDonationDate(record.getDonationDate());
        dto.setUnitsDonated(record.getUnitsDonated());
        dto.setCreatedAt(record.getCreatedAt());

        return dto;
    }
}
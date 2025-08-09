package com.bloodbank.service.impl;

import com.bloodbank.dto.request.BloodInventoryUpdateRequest;
import com.bloodbank.dto.response.*;
import com.bloodbank.entity.*;
import com.bloodbank.exception.BadRequestException;
import com.bloodbank.exception.ResourceNotFoundException;
import com.bloodbank.repository.*;
import com.bloodbank.service.AdminService;
import com.bloodbank.service.mail.EmailService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.time.LocalDate;
import java.util.Date;
import java.util.List;
import java.util.stream.Collectors;

@Service
public class AdminServiceImpl implements AdminService {

    @Autowired
    private BloodInventoryRepository bloodInventoryRepository;
    @Autowired
    private BloodRequestRepository bloodRequestRepository;
    @Autowired
    private UserRepository userRepository;
    @Autowired
    private DonorProfileRepository donorProfileRepository;
    @Autowired
    private DonationAppointmentRepository donationAppointmentRepository;
    @Autowired
    private DonationRecordRepository donationRecordRepository;
    @Autowired
    private EmailService emailService;

    @Override
    public List<BloodInventory> getAllBloodInventory() {
        return bloodInventoryRepository.findAll();
    }

    @Override
    public BloodInventory updateBloodInventory(BloodInventoryUpdateRequest request) {
        BloodInventory inventory = bloodInventoryRepository.findByBloodGroup(request.getBloodGroup())
                .orElse(new BloodInventory(request.getBloodGroup()));
        inventory.setUnits(request.getUnits());
        return bloodInventoryRepository.save(inventory);
    }

    @Override
    public List<BloodRequestDto> getAllPendingBloodRequests() {
        List<BloodRequest> requests = bloodRequestRepository.findByStatus("PENDING");
        return requests.stream().map(this::convertRequestToDto).collect(Collectors.toList());
    }

    @Override
    @Transactional
    public BloodRequestDto updateBloodRequestStatus(Long requestId, String newStatus) {
        BloodRequest bloodRequest = bloodRequestRepository.findById(requestId)
                .orElseThrow(() -> new ResourceNotFoundException("BloodRequest", "id", requestId));

        if (!"PENDING".equalsIgnoreCase(bloodRequest.getStatus())) {
            throw new BadRequestException("Request is not in a PENDING state and cannot be updated.");
        }

        if ("APPROVED".equalsIgnoreCase(newStatus)) {
            BloodInventory inventory = bloodInventoryRepository.findByBloodGroup(bloodRequest.getBloodGroup())
                    .orElseThrow(() -> new ResourceNotFoundException("BloodInventory", "bloodGroup", bloodRequest.getBloodGroup()));
            int requestedUnits = bloodRequest.getUnitsRequested();
            if (inventory.getUnits() < requestedUnits) {
                throw new BadRequestException("Not enough units in inventory to approve this request.");
            }
            inventory.setUnits(inventory.getUnits() - requestedUnits);
            bloodInventoryRepository.save(inventory);
            bloodRequest.setFulfilled(true);
        }

        bloodRequest.setStatus(newStatus.toUpperCase());
        BloodRequest updatedRequest = bloodRequestRepository.save(bloodRequest);

        // --- Send Notification Email to Hospital ---
        String hospitalEmail = updatedRequest.getHospital().getEmail();
        String subject = "Update on your Blood Request #" + updatedRequest.getRequestId();
        String body = null;

        if ("APPROVED".equalsIgnoreCase(newStatus)) {
            body = "Dear Hospital,\n\n" +
                   "Great news! Your blood request for " + updatedRequest.getUnitsRequested() + " units of " +
                   updatedRequest.getBloodGroup() + " blood has been approved.\n\n" +
                   "Please contact us to arrange for collection.\n\n" +
                   "Best Regards,\nThe Blood Bank Team";
        } else if ("REJECTED".equalsIgnoreCase(newStatus)) {
            body = "Dear Hospital,\n\n" +
                   "We regret to inform you that your blood request #" + updatedRequest.getRequestId() +
                   " for " + updatedRequest.getUnitsRequested() + " units of " + updatedRequest.getBloodGroup() +
                   " has been rejected at this time, possibly due to stock limitations.\n\n" +
                   "Please feel free to contact us for more information.\n\n" +
                   "Best Regards,\nThe Blood Bank Team";
        }

        if (body != null) {
            emailService.sendSimpleMessage(hospitalEmail, subject, body);
        }
        // --- End of Notification Logic ---

        return convertRequestToDto(updatedRequest);
    }

    @Override
    public List<DonorInfoResponseDto> getAllDonors() {
        return donorProfileRepository.findAll().stream()
                .map(this::convertDonorProfileToDto)
                .collect(Collectors.toList());
    }

    @Override
    @Transactional
    public DonorInfoResponseDto updateDonorEligibility(Long donorUserId, boolean isEligible) {
        User user = userRepository.findById(donorUserId)
                .orElseThrow(() -> new ResourceNotFoundException("User", "id", donorUserId));
        if (!"ROLE_DONOR".equals(user.getRole().getRoleName())) {
            throw new BadRequestException("User with ID " + donorUserId + " is not a donor.");
        }
        DonorProfile donorProfile = donorProfileRepository.findByUser(user)
                .orElseThrow(() -> new ResourceNotFoundException("DonorProfile", "userId", donorUserId));
        donorProfile.setEligible(isEligible);
        DonorProfile updatedProfile = donorProfileRepository.save(donorProfile);
        return convertDonorProfileToDto(updatedProfile);
    }
    
    @Override
    public List<DonationAppointmentDto> getAllScheduledAppointments() {
        return donationAppointmentRepository.findAll()
                .stream()
                .filter(app -> "SCHEDULED".equalsIgnoreCase(app.getStatus()))
                .map(this::convertAppointmentToDto)
                .collect(Collectors.toList());
    }

    @Override
    @Transactional
    public DonationRecordDto completeDonationAppointment(Long appointmentId) {
        DonationAppointment appointment = donationAppointmentRepository.findById(appointmentId)
                .orElseThrow(() -> new ResourceNotFoundException("DonationAppointment", "id", appointmentId));
        if (!"SCHEDULED".equalsIgnoreCase(appointment.getStatus())) {
            throw new BadRequestException("Appointment is not in a SCHEDULED state.");
        }
        appointment.setStatus("COMPLETED");
        donationAppointmentRepository.save(appointment);
        User donor = appointment.getDonor();
        DonorProfile donorProfile = donorProfileRepository.findByUser(donor)
                .orElseThrow(() -> new ResourceNotFoundException("DonorProfile", "user", donor.getUserId()));
        //  donorProfile.setLastDonationDate(new Date());
       // TO THIS:
        donorProfile.setLastDonationDate(LocalDate.now());
        donorProfileRepository.save(donorProfile);
        DonationRecord record = new DonationRecord();
        record.setDonor(donor);
        record.setDonationDate(LocalDate.now());
        DonationRecord savedRecord = donationRecordRepository.save(record);
        BloodInventory inventory = bloodInventoryRepository.findByBloodGroup(donorProfile.getBloodGroup())
                .orElse(new BloodInventory(donorProfile.getBloodGroup()));
        inventory.setUnits(inventory.getUnits() + 1);
        bloodInventoryRepository.save(inventory);
        
        String subject = "Thank You for Your Blood Donation!";
        String body = "Dear " + donorProfile.getFullName() + ",\n\n" +
                      "Thank you so much for your recent blood donation. Your contribution is invaluable and will help save lives in our community.\n\n" +
                      "We look forward to seeing you again.\n\n" +
                      "Best Regards,\n" +
                      "The Blood Bank Team";
        emailService.sendSimpleMessage(donor.getEmail(), subject, body);

        return convertRecordToDto(savedRecord, donorProfile);
    }

    @Override
    public DashboardStatsDto getDashboardStats() {
        long totalDonors = userRepository.countByRole_RoleName("ROLE_DONOR");
        long totalHospitals = userRepository.countByRole_RoleName("ROLE_HOSPITAL");
        long totalPendingRequests = bloodRequestRepository.countByStatus("PENDING");
        long totalBloodUnits = bloodInventoryRepository.getTotalUnits();
        return DashboardStatsDto.builder()
                .totalDonors(totalDonors)
                .totalHospitals(totalHospitals)
                .totalPendingRequests(totalPendingRequests)
                .totalBloodUnits(totalBloodUnits)
                .build();
    }

    // --- Private Helper Methods for DTO Conversion ---
    private BloodRequestDto convertRequestToDto(BloodRequest bloodRequest) {
        HospitalDto hospitalDto = new HospitalDto();
        hospitalDto.setUserId(bloodRequest.getHospital().getUserId());
        hospitalDto.setEmail(bloodRequest.getHospital().getEmail());

        BloodRequestDto dto = new BloodRequestDto();
        dto.setRequestId(bloodRequest.getRequestId());
        dto.setHospital(hospitalDto);
        dto.setBloodGroup(bloodRequest.getBloodGroup());
        dto.setUnitsRequested(bloodRequest.getUnitsRequested());
        dto.setUrgency(bloodRequest.getUrgency());
        dto.setStatus(bloodRequest.getStatus());
        dto.setCreatedAt(bloodRequest.getCreatedAt());
        dto.setFulfilled(bloodRequest.isFulfilled());
        return dto;
    }

    private DonorInfoResponseDto convertDonorProfileToDto(DonorProfile profile) {
        DonorInfoResponseDto dto = new DonorInfoResponseDto();
        dto.setUserId(profile.getUser().getUserId());
        dto.setEmail(profile.getUser().getEmail());
        dto.setFullName(profile.getFullName());
        dto.setBloodGroup(profile.getBloodGroup());
        dto.setContactNumber(profile.getContactNumber());
        dto.setLastDonationDate(profile.getLastDonationDate());
        dto.setEligible(profile.isEligible());
        dto.setCreatedAt(profile.getCreatedAt());
        return dto;
    }
    
    private DonationAppointmentDto convertAppointmentToDto(DonationAppointment appointment) {
        DonorProfile donorProfile = donorProfileRepository.findByUser(appointment.getDonor())
                .orElseThrow(() -> new ResourceNotFoundException("DonorProfile", "user", appointment.getDonor().getUserId()));

        DonorSimpleDto donorDto = new DonorSimpleDto();
        donorDto.setUserId(appointment.getDonor().getUserId());
        donorDto.setEmail(appointment.getDonor().getEmail());
        donorDto.setFullName(donorProfile.getFullName());

        DonationAppointmentDto dto = new DonationAppointmentDto();
        dto.setAppointmentId(appointment.getAppointmentId());
        dto.setDonor(donorDto);
        dto.setAppointmentDate(appointment.getAppointmentDate());
        dto.setStatus(appointment.getStatus());
        dto.setCreatedAt(appointment.getCreatedAt());
        return dto;
    }

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
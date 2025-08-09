// File: src/main/java/com/bloodbank/repository/DonationAppointmentRepository.java

package com.bloodbank.repository;

import com.bloodbank.entity.DonationAppointment;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List; // Add this import

@Repository
public interface DonationAppointmentRepository extends JpaRepository<DonationAppointment, Long> {

    // This new method will find all appointments for a user that are still scheduled
    List<DonationAppointment> findByDonor_UserIdAndStatus(Long userId, String status);

}
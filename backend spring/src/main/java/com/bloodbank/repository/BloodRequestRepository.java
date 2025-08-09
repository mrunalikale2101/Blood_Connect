package com.bloodbank.repository;

import com.bloodbank.entity.BloodRequest;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List; // Make sure to import java.util.List

@Repository
public interface BloodRequestRepository extends JpaRepository<BloodRequest, Long> {
    
    // Add this new method:
    List<BloodRequest> findByHospital_UserId(Long hospitalId);
    
 // Add this method inside the BloodRequestRepository interface
    List<BloodRequest> findByStatus(String status);
    
 // Add this new method to the interface
    long countByStatus(String status);
}
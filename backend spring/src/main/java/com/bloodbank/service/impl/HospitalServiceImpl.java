package com.bloodbank.service.impl;

import com.bloodbank.dto.request.BloodRequestCreateRequest;
import com.bloodbank.entity.BloodInventory;
import com.bloodbank.entity.BloodRequest;
import com.bloodbank.entity.User;
import com.bloodbank.exception.BadRequestException;
import com.bloodbank.exception.ResourceNotFoundException;
import com.bloodbank.repository.BloodInventoryRepository;
import com.bloodbank.repository.BloodRequestRepository;
import com.bloodbank.repository.UserRepository;
import com.bloodbank.service.HospitalService;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.stereotype.Service;

@Service
public class HospitalServiceImpl implements HospitalService {

    @Autowired
    private BloodRequestRepository bloodRequestRepository;
    @Autowired
    private UserRepository userRepository;
    @Autowired
    private BloodInventoryRepository bloodInventoryRepository;

    @Override
    public BloodRequest createBloodRequest(BloodRequestCreateRequest requestDto) {
        // 1. Get the currently logged-in hospital user
        String hospitalEmail = SecurityContextHolder.getContext().getAuthentication().getName();
        User hospitalUser = userRepository.findByEmail(hospitalEmail)
                .orElseThrow(() -> new UsernameNotFoundException("Hospital user not found with email: " + hospitalEmail));

        // 2. Check if there are enough units in the inventory
        BloodInventory inventory = bloodInventoryRepository.findByBloodGroup(requestDto.getBloodGroup())
                .orElseThrow(() -> new ResourceNotFoundException("Blood Inventory", "bloodGroup", requestDto.getBloodGroup()));

        if (inventory.getUnits() < requestDto.getUnits()) {
           // throw new RuntimeException("Not enough units available in inventory for blood group: " + requestDto.getBloodGroup());
        
        	throw new BadRequestException("Not enough units available in inventory for blood group: " + requestDto.getBloodGroup());
        }

        // 3. Create and save the new blood request
        BloodRequest bloodRequest = new BloodRequest();
        bloodRequest.setHospital(hospitalUser);
        bloodRequest.setBloodGroup(requestDto.getBloodGroup());
        bloodRequest.setUnitsRequested(requestDto.getUnits());
        bloodRequest.setUrgency(requestDto.getUrgency());
        bloodRequest.setStatus("PENDING"); // Set initial status

        return bloodRequestRepository.save(bloodRequest);
    }
    
 // Add this new method to the class. You may need to add imports.
    @Override
    public List<BloodRequest> getMyBloodRequests() {
        String hospitalEmail = SecurityContextHolder.getContext().getAuthentication().getName();
        User hospitalUser = userRepository.findByEmail(hospitalEmail)
                .orElseThrow(() -> new UsernameNotFoundException("Hospital user not found with email: " + hospitalEmail));

        return bloodRequestRepository.findByHospital_UserId(hospitalUser.getUserId());
    }
}
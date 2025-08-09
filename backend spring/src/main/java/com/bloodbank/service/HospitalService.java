package com.bloodbank.service;

import java.util.List;

import com.bloodbank.dto.request.BloodRequestCreateRequest;
import com.bloodbank.entity.BloodRequest;

public interface HospitalService {
    BloodRequest createBloodRequest(BloodRequestCreateRequest requestDto);
    
 // Add this new method to the interface
    List<BloodRequest> getMyBloodRequests();
}
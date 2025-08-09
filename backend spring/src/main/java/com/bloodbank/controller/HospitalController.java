package com.bloodbank.controller;

import com.bloodbank.dto.request.BloodRequestCreateRequest;
import com.bloodbank.entity.BloodRequest;
import com.bloodbank.service.HospitalService;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api/hospital")
@PreAuthorize("hasRole('HOSPITAL')") // Ensures only Hospitals can access
public class HospitalController {

    @Autowired
    private HospitalService hospitalService;

    @PostMapping("/request")
    public ResponseEntity<BloodRequest> createBloodRequest(@RequestBody BloodRequestCreateRequest request) {
        BloodRequest newRequest = hospitalService.createBloodRequest(request);
        return new ResponseEntity<>(newRequest, HttpStatus.CREATED);
    }
    
 // Add this new method to the HospitalController class. You may need to add imports.
    @GetMapping("/requests")
    public ResponseEntity<List<BloodRequest>> getMyBloodRequests() {
        return ResponseEntity.ok(hospitalService.getMyBloodRequests());
    }
}
package com.bloodbank.controller;

import com.bloodbank.dto.request.DonorRegisterRequest;
import com.bloodbank.dto.request.HospitalRegisterRequest;
import com.bloodbank.dto.request.LoginRequest;
import com.bloodbank.dto.response.JwtAuthResponse;
import com.bloodbank.service.AuthService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api/auth")
public class AuthController {

    @Autowired
    private AuthService authService;

    @PostMapping("/register/donor")
    public ResponseEntity<String> registerDonor(@RequestBody DonorRegisterRequest registerRequest) {
        String response = authService.registerDonor(registerRequest);
        return new ResponseEntity<>(response, HttpStatus.CREATED);
    }

    @PostMapping("/register/hospital")
    public ResponseEntity<String> registerHospital(@RequestBody HospitalRegisterRequest registerRequest) {
        String response = authService.registerHospital(registerRequest);
        return new ResponseEntity<>(response, HttpStatus.CREATED);
    }

    @PostMapping("/login")
    public ResponseEntity<JwtAuthResponse> login(@RequestBody LoginRequest loginRequest) {
        JwtAuthResponse response = authService.login(loginRequest);
        return ResponseEntity.ok(response);
    }
}
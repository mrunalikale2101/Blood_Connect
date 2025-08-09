package com.bloodbank.service;

import com.bloodbank.dto.request.DonorRegisterRequest;
import com.bloodbank.dto.request.HospitalRegisterRequest;
import com.bloodbank.dto.request.LoginRequest;
import com.bloodbank.dto.response.JwtAuthResponse;

public interface AuthService {
    String registerDonor(DonorRegisterRequest registerRequest);
    String registerHospital(HospitalRegisterRequest registerRequest);
    JwtAuthResponse login(LoginRequest loginRequest);
}
package com.bloodbank.dto.response;

import lombok.Builder;
import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
@Builder
public class JwtAuthResponse {
    private String accessToken;
    private String tokenType = "Bearer";
    private Object userDetails; // To hold DonorProfile, HospitalProfile etc.
}
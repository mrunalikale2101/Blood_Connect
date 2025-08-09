package com.bloodbank.dto.response;

import lombok.Getter;
import lombok.Setter;

// A simple DTO to represent hospital info safely
@Getter
@Setter
public class HospitalDto {
    private Long userId;
    private String email;
    // We can add hospital name from HospitalProfile later if needed
}
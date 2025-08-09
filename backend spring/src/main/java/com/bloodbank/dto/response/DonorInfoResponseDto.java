package com.bloodbank.dto.response;

import lombok.Getter;
import lombok.Setter;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.Date;

@Getter
@Setter
public class DonorInfoResponseDto {
    private Long userId;
    private String email;
    private String fullName;
    private String bloodGroup;
    private String contactNumber;
    private LocalDate lastDonationDate; // Change the type here
    private boolean isEligible;
    private LocalDateTime createdAt;
}
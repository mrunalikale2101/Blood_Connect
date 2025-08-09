package com.bloodbank.dto.response;

import lombok.Getter;
import lombok.Setter;
import java.time.LocalDateTime;

// This is the object we will return from our API
@Getter
@Setter
public class BloodRequestDto {
    private Long requestId;
    private HospitalDto hospital; // Using our safe Hospital DTO
    private String bloodGroup;
    private int unitsRequested;
    private String urgency;
    private String status;
    private LocalDateTime createdAt;
    private boolean isFulfilled;
}
package com.bloodbank.dto.response;

import lombok.Getter;
import lombok.Setter;

import java.time.LocalDate;
import java.time.LocalDateTime;

@Getter
@Setter
public class DonationRecordDto {
    private Long recordId;
    private DonorSimpleDto donor;
    private LocalDate donationDate;
    private int unitsDonated;
    private LocalDateTime createdAt;
}
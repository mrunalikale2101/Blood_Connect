package com.bloodbank.dto.response;

import lombok.Getter;
import lombok.Setter;
import java.time.LocalDate;
import java.time.LocalDateTime;

@Getter
@Setter
public class DonationAppointmentDto {
    private Long appointmentId;
    private DonorSimpleDto donor;
    private LocalDate appointmentDate;
    private String status;
    private LocalDateTime createdAt;
}
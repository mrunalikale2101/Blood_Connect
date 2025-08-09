package com.bloodbank.dto.request;

import lombok.Getter;
import lombok.Setter;
import java.time.LocalDate;

@Getter
@Setter
public class AppointmentCreateRequest {
    private LocalDate appointmentDate;
}
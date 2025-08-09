package com.bloodbank.dto.response;

import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class DonorSimpleDto {
    private Long userId;
    private String email;
    private String fullName;
}
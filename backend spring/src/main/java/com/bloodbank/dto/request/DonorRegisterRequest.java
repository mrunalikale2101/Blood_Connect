package com.bloodbank.dto.request;

import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class DonorRegisterRequest {
    private String fullName;
    private String email;
    private String password;
    private String bloodGroup;
    private String contactNumber;
}
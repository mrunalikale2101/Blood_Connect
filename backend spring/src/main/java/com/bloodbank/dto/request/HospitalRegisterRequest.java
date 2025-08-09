package com.bloodbank.dto.request;

import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class HospitalRegisterRequest {
    private String hospitalName;
    private String email;
    private String password;
    private String address;
    private String licenseNumber;
    private String contactPerson;
}
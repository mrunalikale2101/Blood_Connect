package com.bloodbank.dto.request;

import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class BloodRequestCreateRequest {
    private String bloodGroup;
    private int units;
    private String urgency;
}
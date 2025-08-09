package com.bloodbank.dto.request;

import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class BloodInventoryUpdateRequest {
    private String bloodGroup;
    private int units;
}
package com.bloodbank.dto.request;

import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class DonorEligibilityUpdateRequest {
    private boolean eligible;
}
package com.bloodbank.dto.response;

import lombok.Builder;
import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
@Builder
public class DashboardStatsDto {
    private long totalDonors;
    private long totalHospitals;
    private long totalPendingRequests;
    private long totalBloodUnits;
}
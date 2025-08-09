package com.bloodbank.entity;

import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import org.hibernate.annotations.CreationTimestamp;

import java.time.LocalDate;
import java.time.LocalDateTime;

@Entity
@Table(name = "donation_records")
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class DonationRecord {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "record_id")
    private Long recordId;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "donor_user_id", nullable = false)
    private User donor;

    @Column(name = "donation_date", nullable = false)
    private LocalDate donationDate;

    @Column(name = "units_donated", nullable = false)
    private int unitsDonated = 1; // Default to 1 unit per donation

    @CreationTimestamp
    @Column(updatable = false)
    private LocalDateTime createdAt;
}
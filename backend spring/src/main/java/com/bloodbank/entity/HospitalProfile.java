package com.bloodbank.entity;

import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import org.hibernate.annotations.CreationTimestamp;
import org.hibernate.annotations.UpdateTimestamp;

import java.time.LocalDateTime;

@Entity
@Table(name = "hospital_profiles")
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class HospitalProfile {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "profile_id")
    private Long profileId;

    @OneToOne
    @JoinColumn(name = "user_id", nullable = false, unique = true)
    private User user;

    @Column(name = "hospital_name", nullable = false)
    private String hospitalName;

    @Column(nullable = false, columnDefinition = "TEXT")
    private String address;

    @Column(name = "license_number", nullable = false, unique = true, length = 100)
    private String licenseNumber;
    
    @Column(name = "contact_person", nullable = false)
    private String contactPerson;

    // Extra Columns from our DB design
    @CreationTimestamp
    @Column(updatable = false)
    private LocalDateTime createdAt;

    @UpdateTimestamp
    private LocalDateTime updatedAt;

    @Column(name = "is_verified", nullable = false)
    private boolean isVerified = false;
}
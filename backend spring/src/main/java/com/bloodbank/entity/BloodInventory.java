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
@Table(name = "blood_inventory")
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class BloodInventory {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "inventory_id")
    private Integer inventoryId;

    @Column(name = "blood_group", nullable = false, unique = true, length = 5)
    private String bloodGroup;

    @Column(nullable = false)
    private int units;

    // Extra Columns from our DB design
    @CreationTimestamp
    @Column(updatable = false)
    private LocalDateTime createdAt;

    @UpdateTimestamp
    private LocalDateTime updatedAt;

    @Column(name = "status_id", nullable = false)
    private int statusId = 1; // e.g., 1-OK, 2-Low, etc.

    // Custom constructor needed by our service
    public BloodInventory(String bloodGroup) {
        this.bloodGroup = bloodGroup;
    }
}
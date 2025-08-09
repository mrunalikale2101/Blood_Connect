package com.bloodbank.repository;

import com.bloodbank.entity.BloodInventory;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import java.util.Optional;

@Repository
public interface BloodInventoryRepository extends JpaRepository<BloodInventory, Integer> {
    Optional<BloodInventory> findByBloodGroup(String bloodGroup);
    
 // Add this new method to the interface
    @Query("SELECT COALESCE(SUM(b.units), 0) FROM BloodInventory b")
    Long getTotalUnits();
}
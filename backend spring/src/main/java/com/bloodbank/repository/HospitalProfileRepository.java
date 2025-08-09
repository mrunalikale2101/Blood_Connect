package com.bloodbank.repository;

import com.bloodbank.entity.HospitalProfile;
import com.bloodbank.entity.User;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.Optional;

@Repository
public interface HospitalProfileRepository extends JpaRepository<HospitalProfile, Long> {
    // This method is used in the login logic to fetch the profile
    Optional<HospitalProfile> findByUser(User user);
}
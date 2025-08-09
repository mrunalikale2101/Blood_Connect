package com.bloodbank.repository;

import com.bloodbank.entity.DonationRecord;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface DonationRecordRepository extends JpaRepository<DonationRecord, Long> {
	// Add this new method to the interface
	List<DonationRecord> findByDonor_UserIdOrderByDonationDateDesc(Long donorId);
}
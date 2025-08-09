// File: src/main/java/com/bloodbank/repository/ContactMessageRepository.java

package com.bloodbank.repository;

import com.bloodbank.entity.ContactMessage;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface ContactMessageRepository extends JpaRepository<ContactMessage, Long> {
    // JpaRepository provides all the basic CRUD methods we need.
}
package com.bloodbank;

import com.bloodbank.entity.Role;
import com.bloodbank.entity.User;
import com.bloodbank.repository.RoleRepository;
import com.bloodbank.repository.UserRepository;
import org.springframework.boot.CommandLineRunner;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.Bean;
import org.springframework.scheduling.annotation.EnableScheduling;
import org.springframework.security.crypto.password.PasswordEncoder;

import java.util.Optional;

@SpringBootApplication
@EnableScheduling // <-- ADD THIS ANNOTATION
public class BloodManagementSystemApplication {

    public static void main(String[] args) {
        SpringApplication.run(BloodManagementSystemApplication.class, args);
    }

    // This bean will run on application startup
    @Bean
    public CommandLineRunner initData(RoleRepository roleRepository, UserRepository userRepository, PasswordEncoder passwordEncoder) {
        return args -> {
            // --- 1. Seed Roles ---
            System.out.println("Seeding roles...");
            String[] roleNames = {"ROLE_ADMIN", "ROLE_DONOR", "ROLE_HOSPITAL"};
            for (String roleName : roleNames) {
                Optional<Role> role = roleRepository.findByRoleName(roleName);
                if (role.isEmpty()) {
                    Role newRole = new Role();
                    newRole.setRoleName(roleName);
                    roleRepository.save(newRole);
                    System.out.println("Created Role: " + roleName);
                }
            }

            // --- 2. Seed Default Admin User ---
            System.out.println("Checking for default admin account...");
            String adminEmail = "admin@bloodbank.com";
            if (userRepository.findByEmail(adminEmail).isEmpty()) {
                System.out.println("Default admin account not found. Creating one...");
                
                Role adminRole = roleRepository.findByRoleName("ROLE_ADMIN")
                        .orElseThrow(() -> new RuntimeException("Error: ROLE_ADMIN is not found."));

                User adminUser = User.builder()
                        .email(adminEmail)
                        .password(passwordEncoder.encode("adminpass")) // Securely hash the password
                        .role(adminRole)
                        .isActive(true)
                        .build();

                userRepository.save(adminUser);
                System.out.println("Default admin account created with email 'admin@bloodbank.com' and password 'adminpass'");
            } else {
                System.out.println("Default admin account already exists.");
            }
        };
    }
}
package com.bloodbank.service.impl;

import com.bloodbank.dto.request.DonorRegisterRequest;
import com.bloodbank.dto.request.HospitalRegisterRequest;
import com.bloodbank.dto.request.LoginRequest;
import com.bloodbank.dto.response.JwtAuthResponse;
import com.bloodbank.entity.*;
import com.bloodbank.exception.ResourceNotFoundException;
import com.bloodbank.repository.*;
import com.bloodbank.security.JwtTokenProvider;
import com.bloodbank.service.AuthService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;


@Service
public class AuthServiceImpl implements AuthService {

    @Autowired
    private AuthenticationManager authenticationManager;
    @Autowired
    private UserRepository userRepository;
    @Autowired
    private RoleRepository roleRepository;
    @Autowired
    private PasswordEncoder passwordEncoder;
    @Autowired
    private JwtTokenProvider jwtTokenProvider;
    @Autowired
    private DonorProfileRepository donorProfileRepository;
    @Autowired
    private HospitalProfileRepository hospitalProfileRepository;

    @Override
    @Transactional
    public String registerDonor(DonorRegisterRequest registerRequest) {
        if (userRepository.findByEmail(registerRequest.getEmail()).isPresent()) {
            throw new RuntimeException("Error: Email is already in use!");
        }

        Role userRole = roleRepository.findByRoleName("ROLE_DONOR")
                .orElseThrow(() -> new RuntimeException("Error: Role is not found."));

        User user = User.builder()
                .email(registerRequest.getEmail())
                .password(passwordEncoder.encode(registerRequest.getPassword()))
                .role(userRole)
                .isActive(true) // The guaranteed fix to ensure user is active on creation
                .build();
        User savedUser = userRepository.save(user);

        DonorProfile donorProfile = new DonorProfile();
        donorProfile.setUser(savedUser);
        donorProfile.setFullName(registerRequest.getFullName());
        donorProfile.setBloodGroup(registerRequest.getBloodGroup());
        donorProfile.setContactNumber(registerRequest.getContactNumber());
        donorProfileRepository.save(donorProfile);

        return "Donor registered successfully!";
    }

    @Override
    @Transactional
    public String registerHospital(HospitalRegisterRequest registerRequest) {
        if (userRepository.findByEmail(registerRequest.getEmail()).isPresent()) {
            throw new RuntimeException("Error: Email is already in use!");
        }

        Role userRole = roleRepository.findByRoleName("ROLE_HOSPITAL")
                .orElseThrow(() -> new RuntimeException("Error: Role is not found."));

        User user = User.builder()
                .email(registerRequest.getEmail())
                .password(passwordEncoder.encode(registerRequest.getPassword()))
                .role(userRole)
                .isActive(true) // The guaranteed fix to ensure user is active on creation
                .build();
        User savedUser = userRepository.save(user);

        HospitalProfile hospitalProfile = new HospitalProfile();
        hospitalProfile.setUser(savedUser);
        hospitalProfile.setHospitalName(registerRequest.getHospitalName());
        hospitalProfile.setAddress(registerRequest.getAddress());
        hospitalProfile.setLicenseNumber(registerRequest.getLicenseNumber());
        hospitalProfile.setContactPerson(registerRequest.getContactPerson());
        hospitalProfileRepository.save(hospitalProfile);

        return "Hospital registered successfully!";
    }

    @Override
    public JwtAuthResponse login(LoginRequest loginRequest) {
        Authentication authentication = authenticationManager.authenticate(
                new UsernamePasswordAuthenticationToken(loginRequest.getEmail(), loginRequest.getPassword()));

        SecurityContextHolder.getContext().setAuthentication(authentication);

        User user = userRepository.findByEmail(loginRequest.getEmail())
                .orElseThrow(() -> new ResourceNotFoundException("User", "email", loginRequest.getEmail()));

        String token = jwtTokenProvider.generateToken(user);
        
        Object userDetails = getUserSpecificProfile(user);

        return JwtAuthResponse.builder()
                .accessToken(token)
                .userDetails(userDetails)
                .build();
    }
    
    private Object getUserSpecificProfile(User user) {
        String roleName = user.getRole().getRoleName();
        if (roleName.equals("ROLE_DONOR")) {
            return donorProfileRepository.findByUser(user)
                .orElseThrow(() -> new ResourceNotFoundException("DonorProfile", "userId", user.getUserId()));
        } else if (roleName.equals("ROLE_HOSPITAL")) {
            return hospitalProfileRepository.findByUser(user)
                .orElseThrow(() -> new ResourceNotFoundException("HospitalProfile", "userId", user.getUserId()));
        }
        // For Admin or other roles, you can return the basic user object or a specific admin profile
        return user;
    }
}
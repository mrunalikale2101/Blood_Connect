// File: src/main/java/com/bloodbank/controller/PublicController.java

package com.bloodbank.controller;

import com.bloodbank.dto.request.ContactMessageRequestDto;
import com.bloodbank.entity.ContactMessage;
import com.bloodbank.service.PublicService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api/public") // A new base path for public endpoints
public class PublicController {

    @Autowired
    private PublicService publicService;

    @PostMapping("/contact")
    public ResponseEntity<String> submitContactMessage(@RequestBody ContactMessageRequestDto requestDto) {
        publicService.saveContactMessage(requestDto);
        return new ResponseEntity<>("Your message has been received. Thank you!", HttpStatus.CREATED);
    }
}
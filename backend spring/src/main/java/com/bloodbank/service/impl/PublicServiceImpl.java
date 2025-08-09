// File: src/main/java/com/bloodbank/service/impl/PublicServiceImpl.java

package com.bloodbank.service.impl;

import com.bloodbank.dto.request.ContactMessageRequestDto;
import com.bloodbank.entity.ContactMessage;
import com.bloodbank.repository.ContactMessageRepository;
import com.bloodbank.service.PublicService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class PublicServiceImpl implements PublicService {

    @Autowired
    private ContactMessageRepository contactMessageRepository;

    @Override
    public ContactMessage saveContactMessage(ContactMessageRequestDto requestDto) {
        ContactMessage message = new ContactMessage();
        message.setName(requestDto.getName());
        message.setEmail(requestDto.getEmail());
        message.setMessage(requestDto.getMessage());
        
        // The 'isRead' flag defaults to false, which is correct.
        
        return contactMessageRepository.save(message);
    }
}
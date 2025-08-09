// File: src/main/java/com/bloodbank/service/PublicService.java

package com.bloodbank.service;

import com.bloodbank.dto.request.ContactMessageRequestDto;
import com.bloodbank.entity.ContactMessage;

public interface PublicService {
    ContactMessage saveContactMessage(ContactMessageRequestDto requestDto);
}
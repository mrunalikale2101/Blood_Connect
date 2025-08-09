// File: src/main/java/com/bloodbank/dto/request/ContactMessageRequestDto.java

package com.bloodbank.dto.request;

import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class ContactMessageRequestDto {
    private String name;
    private String email;
    private String message;
}
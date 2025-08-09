package com.bloodbank.exception;

import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.ResponseStatus;

@ResponseStatus(HttpStatus.BAD_REQUEST) // This annotation tells Spring to return a 400 status
public class BadRequestException extends RuntimeException {
    public BadRequestException(String message) {
        super(message);
    }
}
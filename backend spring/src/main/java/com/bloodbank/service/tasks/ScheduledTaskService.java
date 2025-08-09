package com.bloodbank.service.tasks;

import com.bloodbank.entity.DonorProfile;
import com.bloodbank.repository.DonorProfileRepository;
import com.bloodbank.service.mail.EmailService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Service;

import java.time.LocalDate;
import java.time.temporal.ChronoUnit;
import java.util.List;

@Service
public class ScheduledTaskService {

    @Autowired
    private DonorProfileRepository donorProfileRepository;

    @Autowired
    private EmailService emailService;

    private static final int ELIGIBILITY_DAYS = 90;

    @Scheduled(cron = "0 0 9 * * ?") // Runs every day at 9:00 AM
    //@Scheduled(cron = "0 * * * * ?")
    public void sendDonationEligibilityReminders() {
        System.out.println("Running scheduled task: Checking for eligible donors...");
        List<DonorProfile> allDonors = donorProfileRepository.findAll();

        for (DonorProfile donor : allDonors) {
            if (donor.isEligible() && donor.getLastDonationDate() != null) {
                
                long daysSinceLastDonation = ChronoUnit.DAYS.between(donor.getLastDonationDate(), LocalDate.now());

                if (daysSinceLastDonation == ELIGIBILITY_DAYS) {
                    System.out.println("Found eligible donor: " + donor.getFullName() + ". Sending reminder email.");
                    
                    String subject = "It's Time to Save Lives Again!";
                    String body = "Dear " + donor.getFullName() + ",\n\n" +
                                  "It has been " + ELIGIBILITY_DAYS + " days since your last donation. You are now eligible to donate blood again!\n\n" +
                                  "Your contribution helps save lives in our community. Please consider booking a new appointment.\n\n" +
                                  "Best Regards,\nThe Blood Bank Team";
                    
                    emailService.sendSimpleMessage(donor.getUser().getEmail(), subject, body);
                }
            }
        }
        System.out.println("Scheduled task finished.");
    }
}
package org.example.firstms;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.ApplicationRunner;
import org.springframework.context.annotation.Bean;

import java.util.stream.Stream;

@SpringBootApplication
public class FirstMsApplication {

    public static void main(String[] args) {
        SpringApplication.run(FirstMsApplication.class, args);
    }

    @Bean
    ApplicationRunner start(CandidatRepo repo) {
        return args -> {
            Stream.of(
                    new Candidat("Eya", "Alaimi", "Tunis"),
                    new Candidat("Ahmed", "Mrabet", "Tunis")
            ).forEach(candidat -> {
                repo.save(candidat);
            });

            repo.findAll().forEach(System.out::println);
        };
    }
}

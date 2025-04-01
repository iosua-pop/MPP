package ro.mpp2024;

import ro.mpp2024.repository.*;
import ro.mpp2024.utils.DatabaseSeeder;

import java.io.FileReader;
import java.io.IOException;
import java.util.Properties;

public class MainBD {
    public static void main(String[] args) {
        Properties props = new Properties();
        try {
            props.load(new FileReader("bd.config"));
        } catch (IOException e) {
            System.out.println("Cannot find bd.config " + e);
        }

        FlightRepository flightRepo = new FlightDBRepository(props);
        EmployeeRepository employeeRepo = new EmployeeDBRepository(props);
        TicketRepository ticketRepo = new TicketDBRepository(props);

//        DatabaseSeeder.seedDatabase(employeeRepo, flightRepo, ticketRepo);

        System.out.println("Flight Repository: " + flightRepo.findAll() + "\n");
        System.out.println("Employee Repository: " + employeeRepo.findAll() + "\n");
        System.out.println("Ticket Repository: " + ticketRepo.findAll() + "\n");
    }
}

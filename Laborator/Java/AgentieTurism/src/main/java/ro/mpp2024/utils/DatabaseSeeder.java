package ro.mpp2024.utils;

import ro.mpp2024.domain.Employee;
import ro.mpp2024.domain.Flight;
import ro.mpp2024.domain.Ticket;
import ro.mpp2024.repository.EmployeeRepository;
import ro.mpp2024.repository.FlightRepository;
import ro.mpp2024.repository.TicketRepository;

import java.time.LocalDateTime;
import java.util.List;
import java.util.Random;

public class DatabaseSeeder {
    public static void seedDatabase(EmployeeRepository employeeRepo, FlightRepository flightRepo, TicketRepository ticketRepo) {
        System.out.println("Seeding database...");

        Employee emp1 = new Employee("Iosua Pop", "pop.iosua@example.com", "password");
        Employee emp2 = new Employee("Alexia Adiana", "alexia.adriana@example.com", "123456");
        Employee emp3 = new Employee("Catalin Popa", "catalin_popa@example.com", "secret");

        employeeRepo.add(emp1);
        employeeRepo.add(emp2);
        employeeRepo.add(emp3);

        System.out.println("Added employees.");


        Flight flight1 = new Flight("Paris", LocalDateTime.of(2025, 6, 15, 10, 30), "Charles de Gaulle", 150);
        Flight flight2 = new Flight("London", LocalDateTime.of(2025, 7, 10, 15, 45), "Heathrow", 200);
        Flight flight3 = new Flight("Los Angeles", LocalDateTime.of(2025, 8, 20, 8, 0), "IDK", 100);
        Flight flight4 = new Flight("Berlin", LocalDateTime.of(2025, 9, 5, 18, 20), "Tegel", 120);
        Flight flight5 = new Flight("Rome", LocalDateTime.of(2025, 10, 12, 12, 10), "Fiumicino", 160);

        flightRepo.add(flight1);
        flightRepo.add(flight2);
        flightRepo.add(flight3);
        flightRepo.add(flight4);
        flightRepo.add(flight5);

        System.out.println("Added flights.");

        List<Flight> flights = List.of(flight1, flight2, flight3, flight4, flight5);
        Random random = new Random();

        for (int i = 0; i < 10; i++) {
            Flight randomFlight = flights.get(random.nextInt(flights.size()));
            Ticket ticket = new Ticket(randomFlight, "Client " + (i + 1), "Tourists " + (i + 1), "Address " + (i + 1), random.nextInt(4) + 1);
            ticketRepo.add(ticket);
        }

        System.out.println("Added tickets.");
        System.out.println("Database seeding completed!");
    }
}

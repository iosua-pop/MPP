package ro.mpp2024.service;

import ro.mpp2024.domain.Employee;
import ro.mpp2024.domain.Flight;
import ro.mpp2024.domain.Ticket;
import ro.mpp2024.repository.EmployeeRepository;
import ro.mpp2024.repository.FlightRepository;
import ro.mpp2024.repository.TicketRepository;
import ro.mpp2024.utils.FlightEvent;
import ro.mpp2024.utils.Observable;
import ro.mpp2024.utils.Observer;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

public class Service implements Observable<FlightEvent> {
    private final EmployeeRepository employeeRepo;
    private final FlightRepository flightRepo;
    private final TicketRepository ticketRepo;

    private final List<Observer<FlightEvent>> observers = new ArrayList<>();

    public Service(EmployeeRepository employeeRepo, FlightRepository flightRepo, TicketRepository ticketRepo) {
        this.employeeRepo = employeeRepo;
        this.flightRepo = flightRepo;
        this.ticketRepo = ticketRepo;
    }

    public Employee logIn(String email, String password) {
        return employeeRepo.logIn(email, password);
    }

    public List<Flight> getAllFlights() {
//        List<Flight> flightsList = new ArrayList<>();
//        flightRepo.findAll().forEach(flightsList::add);

        List<Flight> flightsList = flightRepo.findFlights();
        return flightsList;
    }

    public List<Flight> findFlights(String destination, LocalDateTime departureDateTime) {
        return flightRepo.findByDestinationAndDateTime(destination, departureDateTime);
    }

    public void buyTickets(Flight flight, String clientName, String turistsName, String clientAddress, int seatsNumber) {
        int availableSeats = flight.getAvailableSeats();

        if (seatsNumber > availableSeats) {
            throw new RuntimeException("Nu sunt suficiente locuri disponibile!");
        }

        Ticket ticket = new Ticket(flight, clientName, turistsName, clientAddress, seatsNumber);
        ticketRepo.add(ticket);

        int newSeats = availableSeats - seatsNumber;
        flightRepo.updateAvailableSeats(flight.getId(), newSeats);

        notifyObservers(new FlightEvent(FlightEvent.EventType.FLIGHTS_UPDATED));
    }


    @Override
    public void addObserver(Observer<FlightEvent> observer) {
        observers.add(observer);
    }

    @Override
    public void removeObserver(Observer<FlightEvent> observer) {
        observers.remove(observer);
    }

    @Override
    public void notifyObservers(FlightEvent event) {
        for (Observer<FlightEvent> observer : observers) {
            observer.update(event);
        }
        printRed("Notific observerii...");
    }

    public List<Observer<FlightEvent>> getObservers() {
        return observers;
    }

    public static void printRed(String message) {
        System.out.println("\u001B[31m" + message + "\u001B[0m");
    }
}

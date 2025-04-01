package ro.mpp2024.repository;

import ro.mpp2024.domain.Flight;
import ro.mpp2024.domain.Ticket;

import java.time.LocalDateTime;
import java.util.List;

public interface FlightRepository extends IRepository<Integer, Flight> {
    List<Flight> findByDestinationAndDateTime(String destination, LocalDateTime departureDate);
    void updateAvailableSeats(Integer flightId, int newAvailableSeats);
    List<Flight> findFlights();
}

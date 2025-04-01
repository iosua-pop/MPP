package ro.mpp2024.repository;

import ro.mpp2024.domain.Ticket;

import java.util.List;

public interface TicketRepository extends IRepository<Integer, Ticket> {
    List<Ticket> findByFlightId(Integer flightId);
}

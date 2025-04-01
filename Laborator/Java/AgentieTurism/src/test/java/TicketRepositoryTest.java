import org.junit.jupiter.api.*;
import ro.mpp2024.domain.Flight;
import ro.mpp2024.domain.Ticket;
import ro.mpp2024.repository.FlightDBRepository;
import ro.mpp2024.repository.FlightRepository;
import ro.mpp2024.repository.TicketDBRepository;
import ro.mpp2024.repository.TicketRepository;

import java.io.FileReader;
import java.io.IOException;
import java.time.LocalDateTime;
import java.util.List;
import java.util.Properties;

import static org.junit.jupiter.api.Assertions.*;

@TestMethodOrder(MethodOrderer.OrderAnnotation.class)
public class TicketRepositoryTest {
    private static TicketRepository ticketRepo;
    private static FlightRepository flightRepo;
    private static Flight flight;
    private static Ticket ticket;

    @BeforeAll
    static void setup() {
        Properties props = new Properties();
        try {
            props.load(new FileReader("bd.config"));
        } catch (IOException e) {
            fail("Cannot find bd.config: " + e);
        }
        flightRepo = new FlightDBRepository(props);
        ticketRepo = new TicketDBRepository(props);

        flight = new Flight("New York", LocalDateTime.of(2025, 7, 20, 18, 45), "JFK", 250);
        flightRepo.add(flight);
    }

    @Test
    @Order(1)
    void testAddTicket() {
        ticket = new Ticket(flight, "John Doe", "Alice, Bob", "123 Main St", 2);
        ticketRepo.add(ticket);
        assertNotNull(ticket.getId());
    }

    @Test
    @Order(2)
    void testFindTicketById() {
        Ticket foundTicket = ticketRepo.findOne(ticket.getId());
        assertNotNull(foundTicket);
        assertEquals("John Doe", foundTicket.getClientName());
    }

    @Test
    @Order(3)
    void testFindByFlightId() {
        List<Ticket> tickets = ticketRepo.findByFlightId(flight.getId());
        assertFalse(tickets.isEmpty());
    }

    @Test
    @Order(4)
    void testDeleteTicket() {
        ticketRepo.delete(ticket.getId());
        assertNull(ticketRepo.findOne(ticket.getId()));
    }
}
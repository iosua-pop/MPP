import org.junit.jupiter.api.*;
import ro.mpp2024.domain.Flight;
import ro.mpp2024.repository.FlightDBRepository;
import ro.mpp2024.repository.FlightRepository;

import java.io.FileReader;
import java.io.IOException;
import java.time.LocalDateTime;
import java.util.List;
import java.util.Properties;

import static org.junit.jupiter.api.Assertions.*;

@TestMethodOrder(MethodOrderer.OrderAnnotation.class)
public class FlightRepositoryTest {
    private static FlightRepository flightRepo;
    private static Flight flight;

    @BeforeAll
    static void setup() {
        Properties props = new Properties();
        try {
            props.load(new FileReader("bd.config"));
        } catch (IOException e) {
            fail("Cannot find bd.config: " + e);
        }
        flightRepo = new FlightDBRepository(props);
    }

    @Test
    @Order(1)
    void testAddFlight() {
        flight = new Flight("London", LocalDateTime.of(2025, 5, 10, 15, 0), "Heathrow", 200);
        flightRepo.add(flight);
        assertNotNull(flight.getId());
    }

    @Test
    @Order(2)
    void testFindFlightById() {
        Flight foundFlight = flightRepo.findOne(flight.getId());
        assertNotNull(foundFlight);
        assertEquals("London", foundFlight.getDestination());
    }

    @Test
    @Order(3)
    void testUpdateFlight() {
        flight.setAvailableSeats(150);
        flightRepo.update(flight.getId(), flight);
        Flight updatedFlight = flightRepo.findOne(flight.getId());
        assertEquals(150, updatedFlight.getAvailableSeats());
    }

    @Test
    @Order(4)
    void testFindAllFlights() {
        List<Flight> flights = (List<Flight>) flightRepo.findAll();
        assertFalse(flights.isEmpty());
    }

    @Test
    @Order(5)
    void testDeleteFlight() {
        flightRepo.delete(flight.getId());
        assertNull(flightRepo.findOne(flight.getId()));
    }
}

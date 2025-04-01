package ro.mpp2024.repository;

import ro.mpp2024.utils.JdbcUtils;
import ro.mpp2024.domain.Flight;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import java.sql.*;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.List;
import java.util.Properties;

public class FlightDBRepository implements FlightRepository {
    private JdbcUtils dbUtils;
    private static final Logger logger = LogManager.getLogger();
    private static final DateTimeFormatter formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss");

    public FlightDBRepository(Properties props) {
        logger.info("Initializing FlightDBRepository with properties: {}", props);
        dbUtils = new JdbcUtils(props);
    }

    @Override
    public void add(Flight flight) {
        logger.traceEntry("Saving flight {}", flight);
        String sql = "INSERT INTO Flights (destination, departureDateTime, airport, availableSeats) VALUES (?, ?, ?, ?)";

        try (Connection con = dbUtils.getConnection();
             PreparedStatement stmt = con.prepareStatement(sql, Statement.RETURN_GENERATED_KEYS)) {
            DateTimeFormatter formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss");
            stmt.setString(1, flight.getDestination());
            stmt.setString(2, flight.getDepartureDateTime().format(formatter));
            stmt.setString(3, flight.getAirport());
            stmt.setInt(4, flight.getAvailableSeats());

            int affectedRows = stmt.executeUpdate();
            if (affectedRows == 0) {
                throw new SQLException("Creating flight failed, no rows affected.");
            }

            try (ResultSet generatedKeys = stmt.getGeneratedKeys()) {
                if (generatedKeys.next()) {
                    int generatedId = generatedKeys.getInt(1);
                    flight.setId(generatedId);
                    logger.trace("Flight saved successfully with ID: {}", generatedId);
                } else {
                    throw new SQLException("Creating flight failed, no ID obtained.");
                }
            }
        } catch (SQLException e) {
            logger.error("Error inserting flight", e);
        }
        logger.traceExit();
    }


    @Override
    public void update(Integer id, Flight flight) {
        logger.traceEntry("Updating flight with ID: {} to {}", id, flight);
        String sql = "UPDATE Flights SET destination = ?, departureDateTime = ?, airport = ?, availableSeats = ? WHERE id = ?";
        try (Connection con = dbUtils.getConnection();
             PreparedStatement stmt = con.prepareStatement(sql)) {
            stmt.setString(1, flight.getDestination());
            stmt.setString(2, flight.getDepartureDateTime().format(formatter));
            stmt.setString(3, flight.getAirport());
            stmt.setInt(4, flight.getAvailableSeats());
            stmt.setInt(5, id);
            stmt.executeUpdate();
            logger.trace("Flight updated successfully.");
        } catch (SQLException e) {
            logger.error(e);
            System.err.println("Error DB " + e);
        }
        logger.traceExit();
    }

    @Override
    public void delete(Integer id) {
        logger.traceEntry("Deleting flight with ID: {}", id);
        String sql = "DELETE FROM Flights WHERE id = ?";
        try (Connection con = dbUtils.getConnection();
             PreparedStatement stmt = con.prepareStatement(sql)) {
            stmt.setInt(1, id);
            int result = stmt.executeUpdate();
            logger.trace("Deleted {} instances", result);
        } catch (SQLException e) {
            logger.error(e);
            System.err.println("Error DB " + e);
        }
        logger.traceExit();
    }


    @Override
    public Flight findOne(Integer id) {
        logger.traceEntry("Finding flight by ID: {}", id);
        String sql = "SELECT * FROM Flights WHERE id = ?";
        try (Connection con = dbUtils.getConnection();
             PreparedStatement stmt = con.prepareStatement(sql)) {
            stmt.setInt(1, id);
            ResultSet rs = stmt.executeQuery();
            if (rs.next()) {
                Flight flight = extractFlightFromResultSet(rs);
                logger.traceExit(flight);
                return flight;
            }
        } catch (SQLException e) {
            logger.error(e);
        }
        return null;
    }

    @Override
    public Iterable<Flight> findAll() {
        logger.traceEntry();
        List<Flight> flights = new ArrayList<>();
        String sql = "SELECT * FROM Flights";
        try (Connection con = dbUtils.getConnection();
             PreparedStatement stmt = con.prepareStatement(sql);
             ResultSet rs = stmt.executeQuery()) {
            while (rs.next()) {
                flights.add(extractFlightFromResultSet(rs));
            }
        } catch (SQLException e) {
            logger.error(e);
            System.err.println("Error DB " + e);
        }
        logger.traceExit(flights);
        return flights;
    }

    public List<Flight> findFlights() {
        logger.traceEntry();
        List<Flight> flights = new ArrayList<>();
        String sql = "SELECT * FROM Flights WHERE availableSeats > 0";
        try (Connection con = dbUtils.getConnection();
             PreparedStatement stmt = con.prepareStatement(sql);
             ResultSet rs = stmt.executeQuery()) {
            while (rs.next()) {
                flights.add(extractFlightFromResultSet(rs));
            }
        } catch (SQLException e) {
            logger.error(e);
            System.err.println("Error DB " + e);
        }
        logger.traceExit(flights);
        return flights;
    }

//    @Override
//    public List<Flight> findByDestinationAndDateTime(String destination, LocalDateTime departureDateTime) {
//        logger.traceEntry("Finding flights by destination: {} and dateTime: {}", destination, departureDateTime);
//        List<Flight> flights = new ArrayList<>();
//        String sql = "SELECT * FROM Flights WHERE destination = ? AND departureDateTime = ?";
//        try (Connection con = dbUtils.getConnection();
//             PreparedStatement stmt = con.prepareStatement(sql)) {
//            stmt.setString(1, destination);
//            stmt.setString(2, departureDateTime.format(formatter));
//            ResultSet rs = stmt.executeQuery();
//            while (rs.next()) {
//                flights.add(extractFlightFromResultSet(rs));
//            }
//        } catch (SQLException e) {
//            logger.error(e);
//        }
//        logger.traceExit(flights);
//        return flights;
//    }

    @Override
    public List<Flight> findByDestinationAndDateTime(String destination, LocalDateTime departureDateTime) {
        logger.traceEntry("Finding flights by destination: {} and date: {}", destination, departureDateTime.toLocalDate());
        List<Flight> flights = new ArrayList<>();

        String sql = "SELECT * FROM Flights WHERE destination = ? AND SUBSTR(departureDateTime, 1, 10) = ?";

        try (Connection con = dbUtils.getConnection();
             PreparedStatement stmt = con.prepareStatement(sql)) {
            stmt.setString(1, destination);
            stmt.setString(2, departureDateTime.toLocalDate().toString());

            ResultSet rs = stmt.executeQuery();
            while (rs.next()) {
                flights.add(extractFlightFromResultSet(rs));
            }
        } catch (SQLException e) {
            logger.error(e);
        }
        logger.traceExit(flights);
        return flights;
    }

    @Override
    public void updateAvailableSeats(Integer flightId, int newAvailableSeats) {
        logger.traceEntry("Updating available seats for flight ID: {}", flightId);
        String sql = "UPDATE Flights SET availableSeats = ? WHERE id = ?";
        try (Connection con = dbUtils.getConnection();
             PreparedStatement stmt = con.prepareStatement(sql)) {
            stmt.setInt(1, newAvailableSeats);
            stmt.setInt(2, flightId);
            stmt.executeUpdate();
            logger.trace("Updated available seats successfully.");
        } catch (SQLException e) {
            logger.error(e);
            System.err.println("Error DB " + e);
        }
        logger.traceExit();
    }

    private Flight extractFlightFromResultSet(ResultSet rs) throws SQLException {

        LocalDateTime departureDateTime = LocalDateTime.parse(rs.getString("departureDateTime"), formatter);
        Flight flight = new Flight(
                rs.getString("destination"),
                departureDateTime,
                rs.getString("airport"),
                rs.getInt("availableSeats")
        );
        flight.setId(rs.getInt("id"));
        return flight;
    }
}

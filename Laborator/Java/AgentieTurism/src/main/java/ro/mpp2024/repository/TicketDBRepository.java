package ro.mpp2024.repository;

import ro.mpp2024.domain.Flight;
import ro.mpp2024.domain.Ticket;
import ro.mpp2024.utils.JdbcUtils;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import java.sql.*;
import java.util.ArrayList;
import java.util.List;
import java.util.Properties;

public class TicketDBRepository implements TicketRepository {
    private final JdbcUtils dbUtils;
    private static final Logger logger = LogManager.getLogger();

    public TicketDBRepository(Properties props) {
        logger.info("Initializing TicketDBRepository with properties: {}", props);
        dbUtils = new JdbcUtils(props);
    }

    @Override
    public void add(Ticket ticket) {
        logger.traceEntry("Saving ticket {}", ticket);
        String sql = "INSERT INTO Tickets (idFlight, clientName, turistsName, clientAddress, seatsNumber) VALUES (?, ?, ?, ?, ?)";
        try (Connection con = dbUtils.getConnection();
             PreparedStatement stmt = con.prepareStatement(sql, Statement.RETURN_GENERATED_KEYS)) {
            stmt.setInt(1, ticket.getFlight().getId());
            stmt.setString(2, ticket.getClientName());
            stmt.setString(3, ticket.getTuristsName());
            stmt.setString(4, ticket.getClientAddress());
            stmt.setInt(5, ticket.getSeatsNumber());

            int result = stmt.executeUpdate();
            ResultSet generatedKeys = stmt.getGeneratedKeys();
            if (generatedKeys.next()) {
                ticket.setId(generatedKeys.getInt(1));
            }

            logger.trace("Saved {} instances", result);
        } catch (SQLException e) {
            logger.error(e);
            System.err.println("Error DB " + e);
        }
        logger.traceExit();
    }

    @Override
    public void update(Integer id, Ticket ticket) {
        logger.traceEntry("Updating ticket with ID: {} to {}", id, ticket);
        String sql = "UPDATE Tickets SET idFlight = ?, clientName = ?, turistsName = ?, clientAddress = ?, seatsNumber = ? WHERE id = ?";
        try (Connection con = dbUtils.getConnection();
             PreparedStatement stmt = con.prepareStatement(sql)) {
            stmt.setInt(1, ticket.getFlight().getId());
            stmt.setString(2, ticket.getClientName());
            stmt.setString(3, ticket.getTuristsName());
            stmt.setString(4, ticket.getClientAddress());
            stmt.setInt(5, ticket.getSeatsNumber());
            stmt.setInt(6, id);

            int result = stmt.executeUpdate();
            logger.trace("Updated {} instances", result);
        } catch (SQLException e) {
            logger.error(e);
            System.err.println("Error DB " + e);
        }
        logger.traceExit();
    }

    @Override
    public void delete(Integer id) {
        logger.traceEntry("Deleting ticket with ID: {}", id);
        String sql = "DELETE FROM Tickets WHERE id = ?";
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
    public Ticket findOne(Integer id) {
        logger.traceEntry("Finding ticket by ID: {}", id);
        String sql = """
            SELECT t.id, t.clientName, t.turistsName, t.clientAddress, t.seatsNumber,
                   f.id AS flightId, f.destination, f.departureDateTime, f.airport, f.availableSeats
            FROM Tickets t 
            JOIN Flights f ON t.idFlight = f.id WHERE t.id = ?
        """;
        try (Connection con = dbUtils.getConnection();
             PreparedStatement stmt = con.prepareStatement(sql)) {
            stmt.setInt(1, id);
            ResultSet rs = stmt.executeQuery();
            if (rs.next()) {
                Flight flight = new Flight(
                        rs.getString("destination"),
                        rs.getTimestamp("departureDateTime").toLocalDateTime(),
                        rs.getString("airport"),
                        rs.getInt("availableSeats")
                );
                flight.setId(rs.getInt("flightId"));

                Ticket ticket = new Ticket(flight, rs.getString("clientName"), rs.getString("turistsName"),
                        rs.getString("clientAddress"), rs.getInt("seatsNumber"));
                ticket.setId(rs.getInt("id"));

                logger.traceExit(ticket);
                return ticket;
            }
        } catch (SQLException e) {
            logger.error(e);
        }
        return null;
    }

    @Override
    public List<Ticket> findByFlightId(Integer flightId) {
        logger.traceEntry("Finding tickets for flight ID: {}", flightId);
        List<Ticket> tickets = new ArrayList<>();
        String sql = """
            SELECT t.id, t.clientName, t.turistsName, t.clientAddress, t.seatsNumber,
                   f.id AS flightId, f.destination, f.departureDateTime, f.airport, f.availableSeats
            FROM Tickets t 
            JOIN Flights f ON t.idFlight = f.id WHERE f.id = ?
        """;
        try (Connection con = dbUtils.getConnection();
             PreparedStatement stmt = con.prepareStatement(sql)) {
            stmt.setInt(1, flightId);
            ResultSet rs = stmt.executeQuery();
            while (rs.next()) {
                Flight flight = new Flight(
                        rs.getString("destination"),
                        rs.getTimestamp("departureDateTime").toLocalDateTime(),
                        rs.getString("airport"),
                        rs.getInt("availableSeats")
                );
                flight.setId(rs.getInt("flightId"));

                Ticket ticket = new Ticket(flight, rs.getString("clientName"), rs.getString("turistsName"),
                        rs.getString("clientAddress"), rs.getInt("seatsNumber"));
                ticket.setId(rs.getInt("id"));
                tickets.add(ticket);
            }
        } catch (SQLException e) {
            logger.error(e);
        }
        logger.traceExit(tickets);
        return tickets;
    }

    @Override
    public Iterable<Ticket> findAll() {
        logger.traceEntry();
        List<Ticket> tickets = new ArrayList<>();
        String sql = "SELECT * FROM Tickets";
        try (Connection con = dbUtils.getConnection();
             PreparedStatement stmt = con.prepareStatement(sql);
             ResultSet rs = stmt.executeQuery()) {
            while (rs.next()) {
                Ticket ticket = findOne(rs.getInt("id"));
                if (ticket != null) {
                    tickets.add(ticket);
                }
            }
        } catch (SQLException e) {
            logger.error(e);
        }
        logger.traceExit(tickets);
        return tickets;
    }
}

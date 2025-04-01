package ro.mpp2024.domain;

import java.time.LocalDateTime;
import java.util.List;

public class Flight implements Identifiable<Integer>{
    private int id;
    private String destination;
    private LocalDateTime departureDateTime;
    private String airport;
    private int availableSeats;
//    private List<Ticket> tickets;

    public Flight(String destination, LocalDateTime departureDateTime, String airport, int availableSeats) {
        this.destination = destination;
        this.departureDateTime = departureDateTime;
        this.airport = airport;
        this.availableSeats = availableSeats;
    }

    @Override
    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public String getDestination() {
        return destination;
    }

    public void setDestination(String destination) {
        this.destination = destination;
    }

    public LocalDateTime getDepartureDateTime() {
        return departureDateTime;
    }

    public void setDepartureDateTime(LocalDateTime departureDateTime) {
        this.departureDateTime = departureDateTime;
    }

    public String getAirport() {
        return airport;
    }

    public void setAirport(String airport) {
        this.airport = airport;
    }

    public int getAvailableSeats() {
        return availableSeats;
    }

    public void setAvailableSeats(int availableSeats) {
        this.availableSeats = availableSeats;
    }

//    public List<Ticket> getTickets() {
//        return tickets;
//    }
//
//    public void setTickets(List<Ticket> tickets) {
//        this.tickets = tickets;
//    }

    @Override
    public String toString() {
        return "Flight{" +
                "id=" + id +
                ", destination='" + destination + '\'' +
                ", departureDateTime='" + departureDateTime + '\'' +
                ", airport='" + airport + '\'' +
                ", availableSeats=" + availableSeats +
//                ", Tickets=" + tickets +
                '}';
    }
}

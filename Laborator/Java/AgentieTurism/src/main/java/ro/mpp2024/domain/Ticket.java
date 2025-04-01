package ro.mpp2024.domain;

public class Ticket implements Identifiable<Integer>{
    private int id;
//    private int idFlight;

    private Flight flight;
    private String clientName;
    private String turistsName;
    private String clientAddress;
    private int seatsNumber;

    public Ticket(Flight flight, String clientName, String turistsName, String clientAddress, int seatsNumber) {
        this.flight = flight;
        this.clientName = clientName;
        this.turistsName = turistsName;
        this.clientAddress = clientAddress;
        this.seatsNumber = seatsNumber;
    }

    @Override
    public void setId(Integer id) {
        this.id = id;
    }

    @Override
    public Integer getId() {
        return id;
    }

    public Flight getFlight() {
        return flight;
    }

    public void setFlight(Flight flight) {
        this.flight = flight;
    }

    public String getClientName() {
        return clientName;
    }

    public void setClientName(String clientName) {
        this.clientName = clientName;
    }

    public String getTuristsName() {
        return turistsName;
    }

    public void setTuristsName(String turistsName) {
        this.turistsName = turistsName;
    }

    public String getClientAddress() {
        return clientAddress;
    }

    public void setClientAddress(String clientAddress) {
        this.clientAddress = clientAddress;
    }

    public int getSeatsNumber() {
        return seatsNumber;
    }

    public void setSeatsNumber(int seatsNumber) {
        this.seatsNumber = seatsNumber;
    }

    @Override
    public String toString() {
        return "Ticket{" +
                "id=" + id +
                ", flight=" + flight +
                ", clientName='" + clientName + '\'' +
                ", turistsName='" + turistsName + '\'' +
                ", clientAddress='" + clientAddress + '\'' +
                ", seatsNumber=" + seatsNumber +
                '}';
    }
}

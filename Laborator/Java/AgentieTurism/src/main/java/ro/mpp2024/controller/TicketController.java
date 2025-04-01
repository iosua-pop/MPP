package ro.mpp2024.controller;

import javafx.fxml.FXML;
import javafx.scene.control.*;
import javafx.stage.Stage;
import ro.mpp2024.domain.Flight;
import ro.mpp2024.service.Service;
import ro.mpp2024.utils.FlightEvent;

import java.time.format.DateTimeFormatter;

public class TicketController {
    private Service service;
    private Stage primaryStage;
    private Flight flight;

    @FXML
    private Label destinationLabel;
    @FXML
    private Label departureLabel;
    @FXML
    private Label airportLabel;
    @FXML
    private Label seatsAvailableLabel;

    @FXML
    private TextField clientNameField;
    @FXML
    private TextField touristsField;
    @FXML
    private TextField addressField;
    @FXML
    private Spinner<Integer> seatsSpinner;

    public void setService(Service service, Stage primaryStage, Flight flight) {
        this.service = service;
        this.primaryStage = primaryStage;
        this.flight = flight;

        destinationLabel.setText(flight.getDestination());

        DateTimeFormatter formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss");
        String formattedDateTime = flight.getDepartureDateTime().format(formatter);
        departureLabel.setText(formattedDateTime);
        airportLabel.setText(flight.getAirport());
        seatsAvailableLabel.setText(String.valueOf(flight.getAvailableSeats()));
    }

    @FXML
    public void initialize() {
        SpinnerValueFactory<Integer> valueFactory =
                new SpinnerValueFactory.IntegerSpinnerValueFactory(1, 10, 1);
        seatsSpinner.setValueFactory(valueFactory);
    }

    @FXML
    public void handleBuyTickets() {
        String clientName = clientNameField.getText();
        String tourists = touristsField.getText();
        String address = addressField.getText();
        int seats = seatsSpinner.getValue();

        try {
            service.buyTickets(flight, clientName, tourists, address, seats);
            showAlert("Bilet cumparat cu succes!");
            //service.notifyObservers(new FlightEvent(FlightEvent.EventType.FLIGHTS_UPDATED));

            closeWindow();
        } catch (RuntimeException e) {
            showAlert(e.getMessage());
        }
    }

    private void showAlert(String message) {
        Alert alert = new Alert(Alert.AlertType.INFORMATION);
        alert.setContentText(message);
        alert.show();
    }

    private void closeWindow() {
        ((Stage) clientNameField.getScene().getWindow()).close();
    }

    @FXML
    public void handleBack() {
        closeWindow();
    }
}

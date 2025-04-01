package ro.mpp2024.controller;

import javafx.beans.property.SimpleIntegerProperty;
import javafx.beans.property.SimpleObjectProperty;
import javafx.beans.property.SimpleStringProperty;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.stage.Stage;
import ro.mpp2024.domain.Flight;
import ro.mpp2024.service.Service;
import ro.mpp2024.utils.FlightEvent;
import ro.mpp2024.utils.Observer;

import java.io.IOException;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;

public class FlightsController implements Observer<FlightEvent> {
    private Service service;
    private Stage primaryStage;
    private ObservableList<Flight> flightsModel = FXCollections.observableArrayList();
    private static final DateTimeFormatter formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss");
    @FXML
    private TableView<Flight> flightsTable;
    @FXML
    private TableColumn<Flight, String> destinationColumn;
    @FXML
    private TableColumn<Flight, String> dateColumn;
    @FXML
    private TableColumn<Flight, String> airportColumn;
    @FXML
    private TableColumn<Flight, Integer> seatsColumn;
    @FXML
    private TextField searchField;
    @FXML
    private DatePicker datePicker;

    public void setService(Service service, Stage primaryStage) {
        this.service = service;
        this.service.addObserver(this);
        this.primaryStage = primaryStage;
        loadFlights();
    }

    @FXML
    public void initialize() {
        destinationColumn.setCellValueFactory(cellData ->
                new SimpleStringProperty(cellData.getValue().getDestination()));

        dateColumn.setCellValueFactory(cellData ->
                new SimpleStringProperty(cellData.getValue().getDepartureDateTime().format(formatter)));

        airportColumn.setCellValueFactory(cellData ->
                new SimpleStringProperty(cellData.getValue().getAirport()));

        seatsColumn.setCellValueFactory(cellData ->
                new SimpleIntegerProperty(cellData.getValue().getAvailableSeats()).asObject());
    }


    private void loadFlights() {
        flightsModel.setAll(service.getAllFlights());
        flightsTable.setItems(flightsModel);
    }

    @FXML
    public void handleSearch() {
        String destination = searchField.getText();
        LocalDate dateValue = datePicker.getValue();
        LocalDateTime date = (dateValue != null) ? dateValue.atStartOfDay() : null;

        if ((destination == null || destination.trim().isEmpty()) && date == null) {
            flightsModel.setAll(service.getAllFlights());
        }
        else
        {
            flightsModel.setAll(service.findFlights(destination, date));
        }
    }

    @FXML
    public void handleFlightSelection() {
        Flight selectedFlight = flightsTable.getSelectionModel().getSelectedItem();
        if (selectedFlight != null) {
            openTicketPage(selectedFlight);
        }
    }

    private void openTicketPage(Flight flight) {
        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/TicketView.fxml"));
            Parent root = loader.load();

            TicketController ticketController = loader.getController();
            ticketController.setService(service,primaryStage, flight);

            Stage stage = new Stage();
            stage.setScene(new Scene(root));
            stage.setTitle("Buy Ticket");
            stage.show();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    @FXML
    public void handleLogout() {
        service.removeObserver(this);
        ((Stage) flightsTable.getScene().getWindow()).close();
        openLoginPage();
    }

    private void openLoginPage() {
        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/LoginView.fxml"));
            Parent root = loader.load();

            LoginController loginController = loader.getController();
            loginController.setService(service, primaryStage);

            Stage stage = new Stage();
            stage.setScene(new Scene(root));
            stage.setTitle("Login");
            stage.show();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    @Override
    public void update(FlightEvent event) {
        service.printRed("update intra");
        if (event.getType() == FlightEvent.EventType.FLIGHTS_UPDATED) {
            flightsModel.setAll(service.getAllFlights());

            service.printRed("Lista de zboruri a fost actualizata!");
        }
    }
}

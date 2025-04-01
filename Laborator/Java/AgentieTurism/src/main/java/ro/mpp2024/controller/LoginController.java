package ro.mpp2024.controller;

import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.Label;
import javafx.scene.control.PasswordField;
import javafx.scene.control.TextField;
import javafx.stage.Stage;
import ro.mpp2024.domain.Employee;
import ro.mpp2024.service.Service;

import java.io.IOException;

public class LoginController {
    private Service service;
    private Stage primaryStage;

    @FXML
    private TextField emailField;
    @FXML
    private PasswordField passwordField;
    @FXML
    private Label errorLabel;

    public void setService(Service service, Stage primaryStage) {
        this.service = service;
        this.primaryStage = primaryStage;
    }

    @FXML
    public void handleLogin() {
        String email = emailField.getText();
        String password = passwordField.getText();

        Employee employee = service.logIn(email, password);
        if (employee != null) {
            openFlightsPage();
        } else {
            errorLabel.setText("Login failed! Incorrect email or password.");
        }
    }

    private void openFlightsPage() {
        try {
            FXMLLoader loader = new FXMLLoader(getClass().getResource("/views/FlightsView.fxml"));
            Parent root = loader.load();

            FlightsController flightsController = loader.getController();
            flightsController.setService(service, primaryStage);

//            service.addObserver(flightsController);

            primaryStage.setScene(new Scene(root));
            primaryStage.setTitle("Flights");
            primaryStage.show();
            ((Stage) emailField.getScene().getWindow()).close();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

}

package ro.mpp2024.repository;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import ro.mpp2024.domain.Employee;
import ro.mpp2024.utils.JdbcUtils;

import java.nio.charset.StandardCharsets;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.ArrayList;
import java.util.List;
import java.util.Properties;
import java.sql.*;


public class EmployeeDBRepository implements EmployeeRepository {
    private JdbcUtils dbUtils;
    private static final Logger logger = LogManager.getLogger();

    public EmployeeDBRepository(Properties props) {
        logger.info("Initializing EmployeeDBRepository with properties: {}", props);
        dbUtils = new JdbcUtils(props);
    }

    public static String hashPassword(String password) {
        try {
            MessageDigest digest = MessageDigest.getInstance("SHA-256");
            byte[] encodedHash = digest.digest(password.getBytes(StandardCharsets.UTF_8));

            StringBuilder hexString = new StringBuilder();
            for (byte b : encodedHash) {
                String hex = Integer.toHexString(0xff & b);
                if (hex.length() == 1) hexString.append('0');
                hexString.append(hex);
            }

            return hexString.toString();
        } catch (NoSuchAlgorithmException e) {
            throw new RuntimeException("Error hashing password", e);
        }
    }

    public void add(Employee employee) {
        logger.traceEntry("Saving employee {}", employee);
        String sql = "INSERT INTO Employees (name, email, password) VALUES (?, ?, ?)";
        try (Connection con = dbUtils.getConnection();
             PreparedStatement stmt = con.prepareStatement(sql, Statement.RETURN_GENERATED_KEYS)) {
            stmt.setString(1, employee.getName());
            stmt.setString(2, employee.getEmail());
            stmt.setString(3, hashPassword(employee.getPassword()));

            int affectedRows = stmt.executeUpdate();
            if (affectedRows == 0) {
                throw new SQLException("Creating employee failed, no rows affected.");
            }

            try (ResultSet generatedKeys = stmt.getGeneratedKeys()) {
                if (generatedKeys.next()) {
                    int generatedId = generatedKeys.getInt(1);
                    employee.setId(generatedId);
                    logger.trace("Employee saved successfully with ID: {}", generatedId);
                } else {
                    throw new SQLException("Creating employee failed, no ID obtained.");
                }
            }
        } catch (SQLException e) {
            logger.error("Error inserting employee", e);
        }
        logger.traceExit();
    }


    @Override
    public void update(Integer id, Employee employee) {
        logger.traceEntry("Updating employee with ID: {} to {}", id, employee);
        String sql = "UPDATE Employees SET name = ?, email = ?, password = ? WHERE id = ?";
        try (Connection con = dbUtils.getConnection();
             PreparedStatement stmt = con.prepareStatement(sql)) {
            stmt.setString(1, employee.getName());
            stmt.setString(2, employee.getEmail());
            stmt.setString(3, hashPassword(employee.getPassword()));
            stmt.setInt(4, id);
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
        logger.traceEntry("Deleting employee with ID: {}", id);
        String sql = "DELETE FROM Employees WHERE id = ?";
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
    public Employee findOne(Integer id) {
        logger.traceEntry("Finding employee by ID: {}", id);
        String sql = "SELECT * FROM Employees WHERE id = ?";
        try (Connection con = dbUtils.getConnection();
             PreparedStatement stmt = con.prepareStatement(sql)) {
            stmt.setInt(1, id);
            ResultSet rs = stmt.executeQuery();
            if (rs.next()) {
                Employee employee = new Employee(rs.getString("name"), rs.getString("email"), rs.getString("password"));
                employee.setId(rs.getInt("id"));
                logger.traceExit(employee);
                return employee;
            }
        } catch (SQLException e) {
            logger.error(e);
            System.err.println("Error DB " + e);
        }
        return null;
    }


    @Override
    public Employee logIn(String email, String password) {
        logger.traceEntry("Finding employee by email: {}", email);
        String sql = "SELECT * FROM Employees WHERE email = ? AND password = ?";
        try (Connection con = dbUtils.getConnection();
             PreparedStatement stmt = con.prepareStatement(sql)) {
            stmt.setString(1, email);
            stmt.setString(2, hashPassword(password));
            ResultSet rs = stmt.executeQuery();
            if (rs.next()) {
                Employee employee = new Employee(rs.getString("name"), rs.getString("email"), rs.getString("password"));
                employee.setId(rs.getInt("id"));
                logger.traceExit(employee);
                return employee;
            }
        } catch (SQLException e) {
            logger.error(e);
        }
        return null;
    }

    @Override
    public Iterable<Employee> findAll() {
        logger.traceEntry();
        List<Employee> employees = new ArrayList<>();
        String sql = "SELECT * FROM Employees";
        try (Connection con = dbUtils.getConnection();
             PreparedStatement stmt = con.prepareStatement(sql);
             ResultSet rs = stmt.executeQuery()) {
            while (rs.next()) {
                Employee employee = new Employee(rs.getString("name"), rs.getString("email"), rs.getString("password"));
                employee.setId(rs.getInt("id"));
                employees.add(employee);
            }
        } catch (SQLException e) {
            logger.error(e);
        }
        logger.traceExit(employees);
        return employees;
    }
}


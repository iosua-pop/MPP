import org.junit.jupiter.api.*;
import ro.mpp2024.domain.Employee;
import ro.mpp2024.repository.EmployeeDBRepository;
import ro.mpp2024.repository.EmployeeRepository;

import java.io.FileReader;
import java.io.IOException;
import java.util.List;
import java.util.Properties;

import static org.junit.jupiter.api.Assertions.*;

@TestMethodOrder(MethodOrderer.OrderAnnotation.class)
public class EmployeeRepositoryTest {
    private static EmployeeRepository employeeRepo;
    private static Employee employee;

    @BeforeAll
    static void setup() {
        Properties props = new Properties();
        try {
            props.load(new FileReader("bd.config"));
        } catch (IOException e) {
            fail("Cannot find bd.config: " + e);
        }
        employeeRepo = new EmployeeDBRepository(props);
    }

    @Test
    @Order(1)
    void testAddEmployee() {
        employee = new Employee("Pop Iosua", "popiosua@email.com", "password123");
        employeeRepo.add(employee);
        assertNotNull(employee.getId());
    }

    @Test
    @Order(2)
    void testFindEmployeeById() {
        Employee foundEmployee = employeeRepo.findOne(employee.getId());
        assertNotNull(foundEmployee);
        assertEquals("Pop Iosua", foundEmployee.getName());
    }

    @Test
    @Order(3)
    void testUpdateEmployee() {
        employee.setEmail("pop.iosua@email.com");
        employeeRepo.update(employee.getId(), employee);
        Employee updatedEmployee = employeeRepo.findOne(employee.getId());
        assertEquals("pop.iosua@email.com", updatedEmployee.getEmail());
    }

    @Test
    @Order(4)
    void testFindAllEmployees() {
        List<Employee> employees = (List<Employee>) employeeRepo.findAll();
        assertFalse(employees.isEmpty());
    }

    @Test
    @Order(5)
    void testLogIn() {
        Employee loggedIn = employeeRepo.logIn("pop.iosua@email.com", "password123");
        assertNotNull(loggedIn);
        assertEquals("Pop Iosua", loggedIn.getName());
    }

    @Test
    @Order(6)
    void testDeleteEmployee() {
        employeeRepo.delete(employee.getId());
        assertNull(employeeRepo.findOne(employee.getId()));
    }
}

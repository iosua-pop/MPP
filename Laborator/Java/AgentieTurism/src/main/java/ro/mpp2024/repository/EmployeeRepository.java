package ro.mpp2024.repository;

import ro.mpp2024.domain.Employee;

public interface EmployeeRepository extends IRepository<Integer, Employee> {
    Employee logIn(String email, String password);
}

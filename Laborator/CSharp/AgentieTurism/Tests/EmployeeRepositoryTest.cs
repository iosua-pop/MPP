using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using AgentieTurism.Models;
using AgentieTurism.Repository;

namespace AgentieTurism.Tests
{
    [TestFixture]
    public class EmployeeRepositoryTest
    {
        private static IEmployeeRepository employeeRepo;
        private static Employee employee;

        [OneTimeSetUp]
        public void Setup()
        {
            //var props = new Dictionary<string, string>();
            //foreach (var line in File.ReadLines("bd.config"))
            //{
            //    var parts = line.Split('=');
            //    if (parts.Length == 2)
            //        props[parts[0].Trim()] = parts[1].Trim();
            //}

            //employeeRepo = new EmployeeDbRepository(props);
            employeeRepo = new EmployeeDbRepository();
        }

        [Test, Order(1)]
        public void TestAddEmployee()
        {
            employee = new Employee("Pop Iosua", "popiosua@email.com", "password123");
            employeeRepo.Add(employee);
            Assert.That(employee.Id, Is.Not.Null);
        }

        [Test, Order(2)]
        public void TestFindEmployeeById()
        {
            var foundEmployee = employeeRepo.FindOne(employee.Id);
            Assert.That(foundEmployee, Is.Not.Null);
            Assert.That(foundEmployee.Name, Is.EqualTo("Pop Iosua"));
        }

        [Test, Order(3)]
        public void TestUpdateEmployee()
        {
            employee.Email = "pop.iosua@email.com";
            employeeRepo.Update(employee.Id, employee);
            var updatedEmployee = employeeRepo.FindOne(employee.Id);
            Assert.That(updatedEmployee.Email, Is.EqualTo("pop.iosua@email.com"));
        }

        [Test, Order(4)]
        public void TestFindAllEmployees()
        {
            var employees = employeeRepo.FindAll();
            Assert.That(employees, Is.Not.Empty);
        }

        [Test, Order(5)]
        public void TestLogIn()
        {
            var loggedIn = employeeRepo.LogIn("pop.iosua@email.com", "password123");
            Assert.That(loggedIn, Is.Not.Null);
            Assert.That(loggedIn.Name, Is.EqualTo("Pop Iosua"));
        }

        [Test, Order(6)]
        public void TestDeleteEmployee()
        {
            employeeRepo.Delete(employee.Id);
            Assert.That(employeeRepo.FindOne(employee.Id), Is.Null);
        }
    }
}

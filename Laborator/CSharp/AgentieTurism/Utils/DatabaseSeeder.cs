using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgentieTurism.Models;
using AgentieTurism.Repository;
using log4net;

namespace AgentieTurism.Utils
{
    public class DatabaseSeeder
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DatabaseSeeder));

        public static void SeedDatabase(IEmployeeRepository employeeRepo, IFlightRepository flightRepo, ITicketRepository ticketRepo)
        {
            log.Info("Seeding database...");

            var employees = new List<Employee>
            {
                new Employee("Iosua Pop", "pop.iosua@example.com", "password"),
                new Employee("Alexia Adiana", "alexia.adriana@example.com", "123456"),
                new Employee("Catalin Popa", "catalin_popa@example.com", "secret")
            };

            foreach (var employee in employees)
            {
                employeeRepo.Add(employee);
            }

            log.Info("Added employees.");

            var flights = new List<Flight>
            {
                new Flight("Paris", DateTime.ParseExact("2025-06-15 10:30", "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture), "Charles de Gaulle", 150),
                new Flight("London", DateTime.ParseExact("2025-07-10 15:45", "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture), "Heathrow", 200),
                new Flight("Los Angeles", DateTime.ParseExact("2025-08-20 08:00", "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture), "LAX", 100),
                new Flight("Berlin", DateTime.ParseExact("2025-09-05 18:20", "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture), "Tegel", 120),
                new Flight("Rome", DateTime.ParseExact("2025-10-12 12:10", "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture), "Fiumicino", 160)

            };

            foreach (var flight in flights)
            {
                flightRepo.Add(flight);
            }

            log.Info("Added flights.");

            var random = new Random();
            for (int i = 0; i < 10; i++)
            {
                var randomFlight = flights[random.Next(flights.Count)];
                var ticket = new Ticket(randomFlight, $"Client {i + 1}", $"Tourists {i + 1}", $"Address {i + 1}", random.Next(1, 4));
                ticketRepo.Add(ticket);
            }

            log.Info("Added tickets.");
            log.Info("Database seeding completed!");
        }
    }
}

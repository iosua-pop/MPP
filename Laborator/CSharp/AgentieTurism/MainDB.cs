using System;
using System.Configuration;
using System.IO;
using AgentieTurism.Repository;
using AgentieTurism.Utils;
using log4net;

namespace AgentieTurism
{
    class MainDB
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(MainDB));

        //static void Main(string[] args)
        //{
        //    log.Info("Starting application...");

        //    try
        //    {
        //        // Nu mai citim din "bd.config", folosim direct App.config
        //        string connectionString = ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;
        //        log.Info($"Using database connection: {connectionString}");
        //    }
        //    catch (ConfigurationErrorsException e)
        //    {
        //        log.Error("Cannot load database configuration", e);
        //        return;
        //    }

        //    // Inițializăm repository-urile fără `props`
        //    IFlightRepository flightRepo = new FlightDbRepository();
        //    IEmployeeRepository employeeRepo = new EmployeeDbRepository();
        //    ITicketRepository ticketRepo = new TicketDbRepository();

        //    DatabaseSeeder.SeedDatabase(employeeRepo, flightRepo, ticketRepo);

        //    Console.WriteLine("Flight Repository: " + string.Join(", ", flightRepo.FindAll()));
        //    Console.WriteLine("Employee Repository: " + string.Join(", ", employeeRepo.FindAll()));
        //    Console.WriteLine("Ticket Repository: " + string.Join(", ", ticketRepo.FindAll()));

        //    log.Info("Application finished successfully.");
        //}
    }
}

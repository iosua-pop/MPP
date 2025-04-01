using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgentieTurism.Repository;
using AgentieTurism.Services;
using AgentieTurism.Utils;
using log4net;

namespace AgentieTurism
{
    internal static class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));
            log.Info("Starting application...");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //ResetDatabase();


            IEmployeeRepository employeeRepo = new EmployeeDbRepository();
            IFlightRepository flightRepo = new FlightDbRepository();
            ITicketRepository ticketRepo = new TicketDbRepository();

            //DatabaseSeeder.SeedDatabase(employeeRepo, flightRepo, ticketRepo);

            var service = new Service(employeeRepo, flightRepo, ticketRepo);

            //Application.Run(new Form1());
            Application.Run(new LoginView(service));

            log.Info("Application finished successfully.");
        }

        public static void ResetDatabase()
        {
            var con = DbConnectionUtils.GetConnection();
            using (var cmd = con.CreateCommand())
            {
                var sql = @"
            DELETE FROM Tickets;
            DELETE FROM Flights;
            DELETE FROM Employees;
        ";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
        }

    }
}

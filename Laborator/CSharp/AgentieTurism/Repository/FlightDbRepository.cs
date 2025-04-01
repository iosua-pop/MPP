using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgentieTurism.Models;
using AgentieTurism.Utils;
using log4net;

namespace AgentieTurism.Repository
{
    public class FlightDbRepository : IFlightRepository
    {
        private static readonly ILog log = LogManager.GetLogger("FlightDbRepo");
        //private readonly IDictionary<string, string> _properties;

        //public FlightDbRepository(IDictionary<string, string> props)
        public FlightDbRepository()
        {
            log.Info("Initializing FlightDbRepository");
            //_properties = props;
        }

        public void Add(Flight flight)
        {
            log.Info($"Adding flight: {flight}");
            var sql = "INSERT INTO Flights (destination, departureDateTime, airport, availableSeats) VALUES (@Destination, @DepartureDateTime, @Airport, @AvailableSeats)";
            var parameters = new Dictionary<string, object>
            {
                { "@Destination", flight.Destination },
                { "@DepartureDateTime", flight.DepartureDateTime.ToString("yyyy-MM-dd HH:mm") },
                { "@Airport", flight.Airport },
                { "@AvailableSeats", flight.AvailableSeats }
            };


            try
            {
                var con = DbConnectionUtils.GetConnection();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;
                    foreach (var param in parameters)
                    {
                        var dbParam = cmd.CreateParameter();
                        dbParam.ParameterName = param.Key;
                        dbParam.Value = param.Value;
                        cmd.Parameters.Add(dbParam);
                    }

                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "SELECT last_insert_rowid()";
                    flight.Id = Convert.ToInt32(cmd.ExecuteScalar());
                }
                log.Info($"Flight to {flight.Destination} added successfully");
            }
            catch (Exception ex)
            {
                log.Error("Failed to add flight", ex);
            }
        }

        public void Update(int id, Flight flight)
        {
            log.Info($"Updating flight with ID: {id}");
            var sql = "UPDATE Flights SET destination = @Destination, departureDateTime = @DepartureDateTime, airport = @Airport, availableSeats = @AvailableSeats WHERE id = @Id";
            var parameters = new Dictionary<string, object>
            {
                { "@Id", id },
                { "@Destination", flight.Destination },
                { "@DepartureDateTime", flight.DepartureDateTime.ToString("yyyy-MM-dd HH:mm") },
                { "@Airport", flight.Airport },
                { "@AvailableSeats", flight.AvailableSeats }
            };
            ExecuteNonQuery(sql, parameters);
            log.Info($"Flight with ID: {id} updated successfully");
        }

        public void Delete(int id)
        {
            log.Info($"Deleting flight with ID: {id}");
            var sql = "DELETE FROM Flights WHERE id = @Id";
            var parameters = new Dictionary<string, object> { { "@Id", id } };
            ExecuteNonQuery(sql, parameters);
            log.Info($"Flight with ID: {id} deleted successfully");
        }

        public Flight FindOne(int id)
        {
            log.Info($"Finding flight by ID: {id}");
            var sql = "SELECT * FROM Flights WHERE id = @Id";
            var parameters = new Dictionary<string, object> { { "@Id", id } };
            var flight = ExecuteQuerySingle(sql, parameters, DecodeFlight);
            log.Info(flight != null ? $"Flight found: {flight}" : "Flight not found");
            return flight;
        }

        public List<Flight> FindAll()
        {
            log.Info("Retrieving all flights");
            var sql = "SELECT * FROM Flights";
            var flights = ExecuteQueryList(sql, null, DecodeFlight);
            log.Info($"Found {flights.Count} flights");
            return flights;
        }

        public List<Flight> FindFlights()
        {
            log.Info("Retrieving all flights");
            var sql = "SELECT * FROM Flights WHERE availableSeats > 0";
            var flights = ExecuteQueryList(sql, null, DecodeFlight);
            log.Info($"Found {flights.Count} flights");
            return flights;
        }

        public List<Flight> FindByDestinationAndDateTime(string destination, DateTime departureDate)
        {
            log.Info($"Finding flights by destination: '{destination}' and date: {departureDate:yyyy-MM-dd}");

            var sql = @"
                SELECT * FROM Flights
                WHERE destination LIKE @Destination
                AND SUBSTR(departureDateTime, 1, 10) = @DepartureDate";

            var parameters = new Dictionary<string, object>
        {
            { "@Destination", $"%{destination}%" },
            { "@DepartureDate", departureDate.ToString("yyyy-MM-dd") }
        };

            return ExecuteQueryList(sql, parameters, DecodeFlight);
        }

        public void UpdateAvailableSeats(int flightId, int newAvailableSeats)
        {
            log.Info($"Updating available seats for flight ID: {flightId}");
            var sql = "UPDATE Flights SET availableSeats = @AvailableSeats WHERE id = @FlightId";
            var parameters = new Dictionary<string, object>
            {
                { "@FlightId", flightId },
                { "@AvailableSeats", newAvailableSeats }
            };
            ExecuteNonQuery(sql, parameters);
            log.Info($"Updated available seats for flight ID: {flightId} to {newAvailableSeats}");
        }

        private Flight DecodeFlight(IDataReader reader)
        {
            Flight flight = new Flight(
                reader.GetString(1),
                reader.GetDateTime(2),
                reader.GetString(3),
                reader.GetInt32(4)
            );
            flight.Id = reader.GetInt32(0);

            return flight;
        }

        private void ExecuteNonQuery(string sql, Dictionary<string, object> parameters)
        {
            try
            {
                var con = DbConnectionUtils.GetConnection();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;
                    foreach (var param in parameters)
                    {
                        var dbParam = cmd.CreateParameter();
                        dbParam.ParameterName = param.Key;
                        dbParam.Value = param.Value;
                        cmd.Parameters.Add(dbParam);
                    }
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Error("Database operation failed", ex);
            }
        }

        private Flight ExecuteQuerySingle(string sql, Dictionary<string, object> parameters, Func<IDataReader, Flight> decoder)
        {
            try
            {
                var con = DbConnectionUtils.GetConnection();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            var dbParam = cmd.CreateParameter();
                            dbParam.ParameterName = param.Key;
                            dbParam.Value = param.Value;
                            cmd.Parameters.Add(dbParam);
                        }
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        return reader.Read() ? decoder(reader) : null;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Database query failed", ex);
                return null;
            }
        }

        private List<Flight> ExecuteQueryList(string sql, Dictionary<string, object> parameters, Func<IDataReader, Flight> decoder)
        {
            try
            {
                var flights = new List<Flight>();
                var con = DbConnectionUtils.GetConnection();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql;
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            var dbParam = cmd.CreateParameter();
                            dbParam.ParameterName = param.Key;
                            dbParam.Value = param.Value;
                            cmd.Parameters.Add(dbParam);
                        }
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            flights.Add(decoder(reader));
                        }
                    }
                }
                return flights;
            }
            catch (Exception ex)
            {
                log.Error("Database query failed", ex);
                return new List<Flight>();
            }
        }
    }
}

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
    public class TicketDbRepository : ITicketRepository
    {
        private static readonly ILog log = LogManager.GetLogger("TicketDbRepo");
        //private readonly IDictionary<string, string> _properties;

        //public TicketDbRepository(IDictionary<string, string> props)
        public TicketDbRepository()
        {
            log.Info("Initializing TicketDbRepository");
            //_properties = props;
        }

        public void Add(Ticket ticket)
        {
            log.Info($"Adding ticket: {ticket}");
            var sql = "INSERT INTO Tickets (idFlight, clientName, turistsName, clientAddress, seatsNumber) VALUES (@FlightId, @ClientName, @TuristsName, @ClientAddress, @SeatsNumber)";
            var parameters = new Dictionary<string, object>
            {
                { "@FlightId", ticket.Flight.Id },
                { "@ClientName", ticket.ClientName },
                { "@TuristsName", ticket.TuristsName },
                { "@ClientAddress", ticket.ClientAddress },
                { "@SeatsNumber", ticket.SeatsNumber }
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
                    ticket.Id = Convert.ToInt32(cmd.ExecuteScalar());
                }

                log.Info($"Ticket for {ticket.ClientName} added successfully");
            }
            catch (Exception ex)
            {
                log.Error("Failed to add ticket", ex);
            }
        }

        public void Update(int id, Ticket ticket)
        {
            log.Info($"Updating ticket with ID: {id}");
            var sql = "UPDATE Tickets SET idFlight = @FlightId, clientName = @ClientName, turistsName = @TuristsName, clientAddress = @ClientAddress, seatsNumber = @SeatsNumber WHERE id = @Id";
            var parameters = new Dictionary<string, object>
            {
                { "@Id", id },
                { "@FlightId", ticket.Flight.Id },
                { "@ClientName", ticket.ClientName },
                { "@TuristsName", ticket.TuristsName },
                { "@ClientAddress", ticket.ClientAddress },
                { "@SeatsNumber", ticket.SeatsNumber }
            };
            ExecuteNonQuery(sql, parameters);
            log.Info($"Ticket with ID: {id} updated successfully");
        }

        public void Delete(int id)
        {
            log.Info($"Deleting ticket with ID: {id}");
            var sql = "DELETE FROM Tickets WHERE id = @Id";
            var parameters = new Dictionary<string, object> { { "@Id", id } };
            ExecuteNonQuery(sql, parameters);
            log.Info($"Ticket with ID: {id} deleted successfully");
        }

        public Ticket FindOne(int id)
        {
            log.Info($"Finding ticket by ID: {id}");
            var sql = @"
                SELECT t.*, 
                       f.id AS flightId, f.destination, f.departureDateTime, f.airport, f.availableSeats 
                FROM Tickets t 
                JOIN Flights f ON t.idFlight = f.id 
                WHERE t.id = @Id";
            var parameters = new Dictionary<string, object> { { "@Id", id } };
            var ticket = ExecuteQuerySingle(sql, parameters, DecodeTicket);
            log.Info(ticket != null ? $"Ticket found: {ticket}" : "Ticket not found");
            return ticket;
        }

        public List<Ticket> FindAll()
        {
            log.Info("Retrieving all tickets");
            var sql = @"
                SELECT t.*, 
                       f.id AS flightId, f.destination, f.departureDateTime, f.airport, f.availableSeats 
                FROM Tickets t 
                JOIN Flights f ON t.idFlight = f.id";
            var tickets = ExecuteQueryList(sql, null, DecodeTicket);
            log.Info($"Found {tickets.Count} tickets");
            return tickets;
        }

        public List<Ticket> FindByFlightId(int flightId)
        {
            log.Info($"Finding tickets for flight ID: {flightId}");
            var sql = @"
                SELECT t.*, 
                       f.id AS flightId, f.destination, f.departureDateTime, f.airport, f.availableSeats 
                FROM Tickets t 
                JOIN Flights f ON t.idFlight = f.id 
                WHERE f.id = @FlightId";
            var parameters = new Dictionary<string, object> { { "@FlightId", flightId } };
            return ExecuteQueryList(sql, parameters, DecodeTicket);
        }

        private Ticket DecodeTicket(IDataReader reader)
        {
            Flight flight = new Flight(
                reader.GetString(reader.GetOrdinal("destination")),
                reader.GetDateTime(reader.GetOrdinal("departureDateTime")),
                reader.GetString(reader.GetOrdinal("airport")),
                reader.GetInt32(reader.GetOrdinal("availableSeats"))
            )
            { Id = reader.GetInt32(reader.GetOrdinal("flightId")) };

            return new Ticket(
                flight,
                reader.GetString(reader.GetOrdinal("clientName")),
                reader.GetString(reader.GetOrdinal("turistsName")),
                reader.GetString(reader.GetOrdinal("clientAddress")),
                reader.GetInt32(reader.GetOrdinal("seatsNumber"))
            )
            { Id = reader.GetInt32(reader.GetOrdinal("id")) };
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

        private Ticket ExecuteQuerySingle(string sql, Dictionary<string, object> parameters, Func<IDataReader, Ticket> decoder)
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

        private List<Ticket> ExecuteQueryList(string sql, Dictionary<string, object> parameters, Func<IDataReader, Ticket> decoder)
        {
            try
            {
                var tickets = new List<Ticket>();
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
                            tickets.Add(decoder(reader));
                        }
                    }
                }
                return tickets;
            }
            catch (Exception ex)
            {
                log.Error("Database query failed", ex);
                return new List<Ticket>();
            }
        }
    }
}

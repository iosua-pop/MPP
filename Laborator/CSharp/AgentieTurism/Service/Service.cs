using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgentieTurism.Models;
using AgentieTurism.Repository;
using AgentieTurism.Utils;
using log4net;

namespace AgentieTurism.Services
{
    public class Service : Observable<FlightEvent>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Service));
        private readonly IEmployeeRepository employeeRepo;
        private readonly IFlightRepository flightRepo;
        private readonly ITicketRepository ticketRepo;
        private readonly List<Observer<FlightEvent>> observers = new List<Observer<FlightEvent>>();

        public Service(IEmployeeRepository employeeRepo, IFlightRepository flightRepo, ITicketRepository ticketRepo)
        {
            log.Info("Initializing Service");
            this.employeeRepo = employeeRepo;
            this.flightRepo = flightRepo;
            this.ticketRepo = ticketRepo;
        }

        public Employee LogIn(string email, string password)
        {
            log.Info($"Logging in user with email: {email}");
            return employeeRepo.LogIn(email, password);
        }

        public List<Flight> GetAllFlights()
        {
            log.Info("Retrieving all flights");
            return flightRepo.FindFlights();
        }

        public List<Flight> FindFlights(string destination, DateTime departureDateTime)
        {
            log.Info($"Searching flights for destination: {destination} on {departureDateTime}");
            return flightRepo.FindByDestinationAndDateTime(destination, departureDateTime);
        }

        public void BuyTickets(Flight flight, string clientName, string turistsName, string clientAddress, int seatsNumber)
        {
            log.Info($"Attempting to buy {seatsNumber} tickets for {clientName} on flight {flight.Id}");

            int availableSeats = flight.AvailableSeats;
            if (seatsNumber > availableSeats)
            {
                throw new Exception("Not enough seats available!");
            }

            Ticket ticket = new Ticket(flight, clientName, turistsName, clientAddress, seatsNumber);
            ticketRepo.Add(ticket);

            int newSeats = availableSeats - seatsNumber;
            flightRepo.UpdateAvailableSeats(flight.Id, newSeats);

            NotifyObservers(new FlightEvent(FlightEvent.EventType.FLIGHTS_UPDATED));
        }

        public void AddObserver(Observer<FlightEvent> observer)
        {
            observers.Add(observer);
        }

        public void RemoveObserver(Observer<FlightEvent> observer)
        {
            observers.Remove(observer);
        }

        public void NotifyObservers(FlightEvent eventObj)
        {
            foreach (var observer in observers)
            {
                observer.Update(eventObj);
            }
            Console.WriteLine("Notifying observers...");
        }
    }
}

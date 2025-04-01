using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgentieTurism.Models;
using AgentieTurism.Repository;

namespace AgentieTurism.Repository
{
    public interface IFlightRepository : IRepository<Flight, int>
    {
        List<Flight> FindByDestinationAndDateTime(string destination, DateTime departureDateTime);
        void UpdateAvailableSeats(int flightId, int newAvailableSeats);
        List<Flight> FindFlights();
    }
}
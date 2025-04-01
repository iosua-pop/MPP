using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgentieTurism.Models;

namespace AgentieTurism.Models
{
    public class Flight : IIdentifiable<int>
    {
        public int Id { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureDateTime { get; set; }
        public string Airport { get; set; }
        public int AvailableSeats { get; set; }

        public Flight(string destination, DateTime departureDateTime, string airport, int availableSeats)
        {
            Destination = destination;
            DepartureDateTime = departureDateTime;
            Airport = airport;
            AvailableSeats = availableSeats;
        }

        public override string ToString()
        {
            return $"Flight {{ Id={Id}, Destination={Destination}, DepartureDateTime={DepartureDateTime.ToString("yyyy-MM-dd HH:mm")}, Airport={Airport}, AvailableSeats={AvailableSeats} }}";
        }
    }
}

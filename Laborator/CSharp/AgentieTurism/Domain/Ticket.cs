using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgentieTurism.Models;

namespace AgentieTurism.Models
{
    public class Ticket : IIdentifiable<int>
    {
        public int Id { get; set; }
        public Flight Flight { get; set; }
        public string ClientName { get; set; }
        public string TuristsName { get; set; }
        public string ClientAddress { get; set; }
        public int SeatsNumber { get; set; }

        public Ticket(Flight flight, string clientName, string turistsName, string clientAddress, int seatsNumber)
        {
            Flight = flight;
            ClientName = clientName;
            TuristsName = turistsName;
            ClientAddress = clientAddress;
            SeatsNumber = seatsNumber;
        }

        public override string ToString()
        {
            return $"Ticket {{ Id={Id}, Flight={Flight}, ClientName={ClientName}, TuristsName={TuristsName}, ClientAddress={ClientAddress}, SeatsNumber={SeatsNumber} }}";
        }
    }
}

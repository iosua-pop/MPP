using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using AgentieTurism.Models;
using AgentieTurism.Repository;

namespace AgentieTurism.Tests
{
    [TestFixture]
    public class TicketRepositoryTest
    {
        private static ITicketRepository ticketRepo;
        private static IFlightRepository flightRepo;
        private static Flight flight;
        private static Ticket ticket;

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

            //flightRepo = new FlightDbRepository(props);
            //ticketRepo = new TicketDbRepository(props);
            flightRepo = new FlightDbRepository();
            ticketRepo = new TicketDbRepository();

            flight = new Flight("New York", new DateTime(2025, 7, 20, 18, 45, 0), "JFK", 250);
            flightRepo.Add(flight);
        }

        [Test, Order(1)]
        public void TestAddTicket()
        {
            ticket = new Ticket(flight, "John Doe", "Alice, Bob", "123 Main St", 2);
            ticketRepo.Add(ticket);
            Assert.That(ticket.Id, Is.Not.Null);
        }

        [Test, Order(2)]
        public void TestFindTicketById()
        {
            var foundTicket = ticketRepo.FindOne(ticket.Id);
            Assert.That(foundTicket, Is.Not.Null);
            Assert.That(foundTicket.ClientName, Is.EqualTo("John Doe"));
        }

        [Test, Order(3)]
        public void TestFindByFlightId()
        {
            var tickets = ticketRepo.FindByFlightId(flight.Id);
            Assert.That(tickets, Is.Not.Empty);
        }

        [Test, Order(4)]
        public void TestDeleteTicket()
        {
            ticketRepo.Delete(ticket.Id);
            Assert.That(ticketRepo.FindOne(ticket.Id), Is.Null);
        }
    }
}

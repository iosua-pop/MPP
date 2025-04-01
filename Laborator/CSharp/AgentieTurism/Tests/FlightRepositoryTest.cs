using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using AgentieTurism.Models;
using AgentieTurism.Repository;

namespace AgentieTurism.Tests
{
    [TestFixture]
    public class FlightRepositoryTest
    {
        private static IFlightRepository flightRepo;
        private static Flight flight;

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
            flightRepo = new FlightDbRepository();
        }

        [Test, Order(1)]
        public void TestAddFlight()
        {
            flight = new Flight("London", new DateTime(2025, 5, 10, 15, 0, 0), "Heathrow", 200);
            flightRepo.Add(flight);
            Assert.That(flight.Id, Is.Not.Null);
        }

        [Test, Order(2)]
        public void TestFindFlightById()
        {
            var foundFlight = flightRepo.FindOne(flight.Id);
            Assert.That(foundFlight, Is.Not.Null);
            Assert.That(foundFlight.Destination, Is.EqualTo("London"));
        }

        [Test, Order(3)]
        public void TestUpdateFlight()
        {
            flight.AvailableSeats = 150;
            flightRepo.Update(flight.Id, flight);
            var updatedFlight = flightRepo.FindOne(flight.Id);
            Assert.That(updatedFlight.AvailableSeats, Is.EqualTo(150));
        }

        [Test, Order(4)]
        public void TestFindAllFlights()
        {
            var flights = flightRepo.FindAll();
            Assert.That(flights, Is.Not.Empty);
        }

        [Test, Order(5)]
        public void TestDeleteFlight()
        {
            flightRepo.Delete(flight.Id);
            Assert.That(flightRepo.FindOne(flight.Id), Is.Null);
        }
    }
}

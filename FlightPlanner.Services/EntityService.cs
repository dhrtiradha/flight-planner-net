using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;
using Microsoft.EntityFrameworkCore.Design;

namespace FlightPlanner.Services
{
    public class EntityService<T> : DbService, IEntityService<T> where T : Entity
    {
        public EntityService(FlightPlannerDbContext context) : base(context)
        {
        }
        
        public T GetById(int id)
        {
            return GetById<T>(id);
        }

        public ServiceResult Create(T entity)
        {
            return Create<T>(entity);
        }

        public ServiceResult Delete(T entity)
        {
            return Delete<T>(entity);
        }

        public ServiceResult Update(T entity)
        {
            return Update<T>(entity);
        }

        public IEnumerable<T> List()
        {
            return List<T>();
        }

        public bool IsFlightUnique(Flight flight)
        {
            return !_context.Flights.Any(existingFlight =>
                existingFlight.From.AirportCode == flight.From.AirportCode &&
                existingFlight.To.AirportCode == flight.To.AirportCode &&
                existingFlight.DepartureTime == flight.DepartureTime &&
                existingFlight.ArrivalTime == flight.ArrivalTime);
        }

        public IEnumerable<Airport> SearchAirports(string search)
        {
            var trimmedSearch = search.Trim().ToLower();

            return _context.Airports
                .Where(a => a.AirportCode.ToLower().Contains(trimmedSearch) ||
                            a.City.ToLower().Contains(trimmedSearch) ||
                            a.Country.ToLower().Contains(trimmedSearch))
                .ToList();
        }

        public List<Flight> GetFlightsByCriteria(string from, string to, DateTime departureDate)
        {
            var targetDate = departureDate.Date;

            return _context.Flights
                .Where(f =>
                    f.From.AirportCode.ToLower() == from.ToLower() &&
                    f.To.AirportCode.ToLower() == to.ToLower())
                .AsEnumerable()
                .Where(f =>
                    DateTime.TryParse(f.DepartureTime, out var flightDepartureTime) &&
                    flightDepartureTime.Date == targetDate)
                .ToList();
        }
    }
}

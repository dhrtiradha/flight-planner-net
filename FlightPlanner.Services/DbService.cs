using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Services
{
    public class DbService : IDbService
    {
        protected readonly FlightPlannerDbContext _context;

        public DbService(FlightPlannerDbContext context)
        {
            _context = context;
        }

        public T? GetById<T>(int id) where T : Entity
        {
            return _context.Set<T>()
                .Include("From") 
                .Include("To")    
                .SingleOrDefault(entity => entity.Id == id);
        }

        public ServiceResult Create<T>(T entity) where T : Entity
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();

            return new ServiceResult(true).Set(entity);
        }

        public ServiceResult Delete<T>(T entity) where T : Entity
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();

            return new ServiceResult(true);
        }

        public ServiceResult Update<T>(T entity) where T : Entity
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();

            return new ServiceResult(true).Set(entity);
        }

        public IEnumerable<T> List<T>() where T : Entity
        {
            return _context.Set<T>().ToList();
        }

        public bool IsEntityUnique<T>(T entity) where T : Entity
        {
            if (entity is Flight flight)
            {
                return !_context.Flights.Any(existingFlight =>
                    existingFlight.From.AirportCode == flight.From.AirportCode &&
                    existingFlight.To.AirportCode == flight.To.AirportCode &&
                    existingFlight.DepartureTime == flight.DepartureTime &&
                    existingFlight.ArrivalTime == flight.ArrivalTime);
            }

            return false;
        }

        public IEnumerable<Airport> SearchEntitiesAirport(string search)
        {
            var trimmedSearch = search.Trim().ToLower();
            return _context.Airports
                .Where(a => a.AirportCode.ToLower().Contains(trimmedSearch) ||
                            a.City.ToLower().Contains(trimmedSearch) ||
                            a.Country.ToLower().Contains(trimmedSearch))
                .ToList();
        }
        
        public IEnumerable<Flight> GetEntitiesByCriteriaFlight(string from, string to, DateTime departureDate)
        {
            var targetDate = departureDate.Date;

            var flights = _context.Flights
                .Where(f =>
                    f.From.AirportCode.ToLower() == from.ToLower() && 
                    f.To.AirportCode.ToLower() == to.ToLower())       
                .AsEnumerable()  
                .Where(f =>
                    DateTime.TryParse(f.DepartureTime, out var flightDepartureTime) &&
                    flightDepartureTime.Date == targetDate)
                .ToList();

            return flights;
        }
    }
}

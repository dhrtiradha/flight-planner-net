using FlightPlanner.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace FlightPlanner.Core.Services
{
    public interface IDbService
    {
        T? GetById<T>(int id) where T : Entity;
        ServiceResult Create<T>(T entity) where T : Entity;
        ServiceResult Delete<T>(T entity) where T : Entity;
        ServiceResult Update<T>(T entity) where T : Entity;
        IEnumerable<T> List<T>() where T : Entity;
        bool IsEntityUnique<T>(T entity) where T : Entity;
        IEnumerable<Airport> SearchEntitiesAirport(string search);
        IEnumerable<Flight> GetEntitiesByCriteriaFlight(string from, string to, DateTime departureDate);
    }
}

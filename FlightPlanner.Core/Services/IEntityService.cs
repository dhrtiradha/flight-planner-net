using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightPlanner.Core.Models;

namespace FlightPlanner.Core.Services
{
    public interface IEntityService<T> where T : Entity
    {
        T? GetById(int id);
        ServiceResult Create(T entity);
        ServiceResult Delete(T entity);
        ServiceResult Update(T entity);
        IEnumerable<T> List();
        bool IsEntityUnique(T entity);
        IEnumerable<Airport> SearchAirports(string search);
        IEnumerable<Flight> GetFlightsByCriteria(string from, string to, DateTime departureDate);
    }
}

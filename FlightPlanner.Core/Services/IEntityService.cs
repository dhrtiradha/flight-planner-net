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
        bool IsFlightUnique(Flight flight);
        public IEnumerable<Airport> SearchAirports(string search);
        public List<Flight> GetFlightsByCriteria(string from, string to, DateTime departureDate);
    }
}

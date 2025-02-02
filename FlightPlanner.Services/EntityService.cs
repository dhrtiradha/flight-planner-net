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
            return GetById<T>(id)!;
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

        public bool IsEntityUnique(T entity)
        {
            return IsEntityUnique<T>(entity);
        }

        public IEnumerable<Airport> SearchAirports(string search)
        {
            return SearchEntitiesAirport(search);
        }

        public IEnumerable<Flight> GetFlightsByCriteria(string from, string to, DateTime departureDate)
        {
            return base.GetEntitiesByCriteriaFlight(from, to, departureDate);
        }
    }
}

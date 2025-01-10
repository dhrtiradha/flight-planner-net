using FlightPlanner.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightPlanner.Core.Models;
using FlightPlanner.Data;

namespace FlightPlanner.Services
{
    public class DbClearingService : DbService, IDbClearingService
    {
        public DbClearingService(FlightPlannerDbContext context) : base(context)
        {
        }

        public ServiceResult Clear<T>() where T : Entity
        {
            _context.Set<T>().RemoveRange(_context.Set<T>());
            _context.SaveChanges();

            return new ServiceResult(true);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightPlanner.Core.Models;

namespace FlightPlanner.Core.Services
{
    public interface IDbClearingService : IDbService
    {
        ServiceResult Clear<T>() where T : Entity;
    }
}

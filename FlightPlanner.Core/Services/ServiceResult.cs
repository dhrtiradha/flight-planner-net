using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightPlanner.Core.Models;

namespace FlightPlanner.Core.Services
{
    public class ServiceResult
    {
        public ServiceResult()
        {

        }

        public ServiceResult(bool succeeded)
        {
            Succeeded = succeeded;
        }

        public Entity Entity { get; set; }

        public bool Succeeded { get; private set; }

        public List<string> Errors { get; private set; }

        public ServiceResult Set(IEnumerable<string> errors)
        {
            Errors = errors.ToList();
            return this;
        }

        public ServiceResult Set(bool succeeded)
        {
            Succeeded = succeeded;
            return this;
        }

        public ServiceResult Set(Entity entity)
        {
            Entity = entity;
            return this;
        }
    }
}

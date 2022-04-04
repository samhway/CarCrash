using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarCrash.Models
{
    public class EFCarCrashRepository : ICarCrashRepository
    {
        private CrashDbContext context { get; set; }

        public EFCarCrashRepository (CrashDbContext temp)
        {
            context = temp;
        }
        public IQueryable<Crash> Crashes => context.crashes;
        public IQueryable<Location> Locations => context.Locations;
        public IQueryable<Road> Roads => context.Roads;
    }
}

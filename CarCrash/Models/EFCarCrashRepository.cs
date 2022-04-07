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
        public IQueryable<Location> Locations => context.locations;
        public IQueryable<Road> Roads => context.roads;

        public void Save(Crash c)
        {
            if (c.CRASH_ID == 0)
            {
                context.crashes.Add(c);
            }
            
            context.SaveChanges();
        }

        public void SaveLocation(Location l)
        {
            context.locations.Add(l);
            context.SaveChanges();
        }

        public void SaveRoad(Road r)
        {
            context.roads.Add(r);
            context.SaveChanges();
        }

        public void Update(Crash c)
        {
            context.Update(c);
            context.SaveChanges();
        }

        public void DeleteIt(Crash c)
        {
            context.Remove(c);
            context.SaveChanges();
        }
    }
}

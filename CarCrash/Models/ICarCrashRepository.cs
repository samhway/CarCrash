using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarCrash.Models
{
    public interface ICarCrashRepository
    {
        IQueryable<Crash> Crashes { get; }
        IQueryable<Location> Locations { get; }
        IQueryable<Road> Roads { get; }

        public void Save(Crash c);
        public void SaveLocation(Location l);
        public void SaveRoad(Road r);
        public void Update(Crash c);
        public void DeleteIt(Crash c);
    }
}

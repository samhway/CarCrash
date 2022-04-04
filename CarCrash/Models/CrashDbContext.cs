using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarCrash.Models
{
    public class CrashDbContext : DbContext
    {
        public CrashDbContext(DbContextOptions<CrashDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Crash> crashes { get; set; }
        public virtual DbSet<Location> locations { get; set; }
        public virtual DbSet<Road> roads { get; set; }
    }
}

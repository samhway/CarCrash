using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarCrash.Models
{
    public class RoadDbContext : DbContext
    {
        public RoadDbContext(DbContextOptions<RoadDbContext> options) : base(options)
        {

        }

        public DbSet<Road> Roads { get; set; }
    }
}

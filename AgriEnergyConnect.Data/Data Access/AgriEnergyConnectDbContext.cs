using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Data.Data_Access
{
    public class AgriEnergyConnectDbContext : DbContext
    {
        public AgriEnergyConnectDbContext(DbContextOptions<AgriEnergyConnectDbContext> options) : base(options)
        {}

        public DbSet<Models.Farmer> Farmers { get; set; }
        public DbSet<Models.Product> Products { get; set; }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AgriEnergyConnect.Data.Models;

namespace AgriEnergyConnect.Data.Data_Access
{
    public class AgriEnergyConnectDbContext : DbContext
    {
        public AgriEnergyConnectDbContext(DbContextOptions<AgriEnergyConnectDbContext> options) : base(options)
        {}

        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<Product> Products { get; set; }



    }
}

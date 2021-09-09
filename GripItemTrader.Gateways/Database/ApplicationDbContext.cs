using GripItemTrader.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GripItemTrader.Gateways.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Person> Person { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<ItemTransfer> ItemTransfer { get; set; }
    }
}

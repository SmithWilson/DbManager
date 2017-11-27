using DbManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Core.DbProvider.Datacontext
{
    public class ManagerContext : DbContext
    {
        public ManagerContext() : base("DefaultConnection")
        {

        }

        public DbSet<RootPassword> Passwords { get; set; }

        public DbSet<Facility> Facilitys { get; set; }
    }
}

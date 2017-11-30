using DbManager.Models;
using System.Data.Entity;

namespace DbManager.Core.DbProvider.Datacontext
{
    public class ManagerContext : DbContext
    {
        private static ManagerContext _instance;
        private ManagerContext() : base("DefaultConnection")
        {

        }

        public static ManagerContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ManagerContext();
                }

                return _instance;
            }
        }

        public DbSet<RootPassword> Passwords { get; set; }

        public DbSet<Facility> Facilitys { get; set; }
    }
}

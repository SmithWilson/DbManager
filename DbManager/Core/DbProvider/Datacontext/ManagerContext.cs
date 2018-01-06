using DbManager.Models;
using System.Data.Entity;

namespace DbManager.Core.DbProvider.Datacontext
{
    public class ManagerContext : DbContext
    {
        /// <summary>
        /// Инстанс базы данных.
        /// </summary>
        private static ManagerContext _instance;
        /// <summary>
        /// Обьект блокировки.
        /// </summary>
        private static object _lockObject = new object();

        /// <summary>
        /// Конструктор.
        /// </summary>
        private ManagerContext() : base("DefaultConnection")
        {

        }

        /// <summary>
        /// Потокозащищенный синглтон.
        /// </summary>
        public static ManagerContext Instance
        {
            get
            {
                lock (_lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new ManagerContext();
                    }

                    return _instance; 
                }
            }
        }

        /// <summary>
        /// Таблица Паролей.
        /// </summary>
        public DbSet<RootPassword> Passwords { get; set; }

        /// <summary>
        /// Таблица сооружений(обьектов).
        /// </summary>
        public DbSet<Facility> Facilitys { get; set; }
    }
}

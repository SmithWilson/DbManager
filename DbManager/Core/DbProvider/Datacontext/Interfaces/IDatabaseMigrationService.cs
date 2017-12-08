using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Core.DbProvider.Datacontext.Interfaces
{
    public interface IDatabaseMigrationService
    {
        /// <summary>
        /// Импорт базы данных в файл.json
        /// </summary>
        /// <returns></returns>
        Task Import();


        /// <summary>
        /// Экспорт базы данных из файл.json
        /// </summary>
        /// <returns></returns>
        Task Export();
    }
}

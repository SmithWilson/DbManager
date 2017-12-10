using DbManager.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Core.DbProvider.Datacontext.Interfaces
{
    public interface IDatabaseMigrationService
    {
        /// <summary>
        /// Импорт данных из файла.xls
        /// </summary>
        /// <param name="path">Путь.</param>
        /// <returns></returns>
        Task Import(string path);
        
        /// <summary>
        /// Экспорт данных из базы данных.
        /// </summary>
        /// <param name="dataTable">Таблица.</param>
        /// <returns></returns>
        Task Export(DataTable dataTable);
    }
}

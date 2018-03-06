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
        Task<bool> Import(string path);

        /// <summary>
        /// Импорт документов.
        /// </summary>
        /// <param name="path">Путь.</param>
        /// <returns></returns>
        Task ImportFiles(List<string> paths);

        /// <summary>
        /// Экспорт данных из базы данных.
        /// </summary>
        /// <param name="dataTable">Таблица.</param>
        /// <returns></returns>
        Task Export(DataTable dataTable, List<Models.FileInfo> files);
    }
}

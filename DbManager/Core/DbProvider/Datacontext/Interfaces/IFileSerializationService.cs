using DbManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Core.DbProvider.Datacontext.Interfaces
{
    public interface IFileSerializationService
    {
        /// <summary>
        /// Запись <paramref name="facilitys"/> в файл.
        /// </summary>
        /// <param name="path">Путь.</param>
        /// <param name="fileName">Имя файла.</param>
        /// <param name="facilitys">база данных.</param>
        /// <returns></returns>
        Task WriteObjectToFile(string path, string fileName, List<Facility> facilitys);

        /// <summary>
        /// Чтение json из файла.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<List<Facility>> ReadJsonFromFile(string path, string fileName);
    }
}

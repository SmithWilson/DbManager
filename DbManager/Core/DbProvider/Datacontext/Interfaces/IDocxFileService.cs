using System.Threading.Tasks;

namespace DbManager.Core.DbProvider.Datacontext.Interfaces
{
    public interface IDocxFileService
    {
        /// <summary>
        /// Добавление файла в бд.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="path">Путь.</param>
        /// <returns></returns>
        Task PutDocxFileToDatabase(int id, string path);

        /// <summary>
        /// Пролучение файла из бд.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="fileName">Имя файла.</param>
        /// <returns></returns>
        Task GetDoxcFileFromDatabase(int id, string fileName);
    }
}

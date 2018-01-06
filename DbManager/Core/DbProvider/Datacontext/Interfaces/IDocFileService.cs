using System.Threading.Tasks;

namespace DbManager.Core.DbProvider.Datacontext.Interfaces
{
    public interface IDocFileService
    {
        /// <summary>
        /// Добавление файла в бд.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="path">Путь.</param>
        /// <returns></returns>
        Task PutDocFileToDatabase(int id, string path);

        /// <summary>
        /// Пролучение файла из бд.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="fileName">Имя файла.</param>
        /// <returns></returns>
        Task GetDoсFileFromDatabase(int id, string fileName);
    }
}

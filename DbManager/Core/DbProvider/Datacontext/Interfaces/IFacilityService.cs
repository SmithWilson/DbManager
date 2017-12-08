using DbManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DbManager.Core.DbProvider.Datacontext.Interfaces
{
    //TRUNCATE TABLE _context.Database.ExecuteSqlCommand("TRUNCATE TABLE RootPasswords");
    public interface IFacilityService
    {
        /// <summary>
        /// Получение сооружения по Id.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Сооружение.</returns>
        Task<Facility> GetById(int id);

        /// <summary>
        /// Получение сооружения по договору <see cref="Models.Facility.Treaty"/>.
        /// </summary>
        /// <param name="pattern">Строка поиска.</param>
        /// <returns>Сооружение.</returns>
        Task<List<Facility>> GetByTreaty(string pattern);

        /// <summary>
        /// Получение всех сооружений.
        /// </summary>
        /// <returns>Список сооружений.</returns>
        Task<List<Facility>> Get();

        /// <summary>
        /// Получение сооружений.
        /// </summary>
        /// <param name="count">Количество.</param>
        /// <param name="offset">Сдвиг.</param>
        /// <returns>Список сооружений.</returns>
        Task<IEnumerable<Facility>> Get(int count, int offset);

        /// <summary>
        /// Добавление сооружения.
        /// </summary>
        /// <param name="facility">Сооружение.</param>
        /// <returns></returns>
        Task Add(Facility facility);

        /// <summary>
        /// Удаление сооружения.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns></returns>
        Task Remove(int id);

        /// <summary>
        /// Изменение.
        /// </summary>
        /// <param name="facility">Новые данные.</param>
        /// <returns></returns>
        Task Change(Facility facility);

        /// <summary>
        /// Сброс таблицы.
        /// </summary>
        /// <returns></returns>
        Task Reset();
    }
}

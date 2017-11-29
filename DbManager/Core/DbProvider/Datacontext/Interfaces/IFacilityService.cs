using DbManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Core.DbProvider.Datacontext.Interfaces
{
    public interface IFacilityService
    {
        /// <summary>
        /// Получение сооружения по Id.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Сооружение.</returns>
        Task<Facility> GetById(int id);

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
    }
}

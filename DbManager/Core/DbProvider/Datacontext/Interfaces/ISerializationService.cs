using DbManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Core.DbProvider.Datacontext.Interfaces
{
    public interface ISerializationService
    {
        /// <summary>
        /// Сериализация <paramref name="obj"/> в файл.
        /// </summary>
        /// <param name="obj">Обьект.</param>
        /// <returns>JSON.</returns>
        Task<string> SerializationObject(object obj);

        /// <summary>
        /// Десериализация <paramref name="json"/>.
        /// </summary>
        /// <param name="json">JSON.</param>
        /// <returns>Список обьектов.</returns>
        Task<List<Facility>> DeserializeJson(string json);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Core.DbProvider.Datacontext.Interfaces
{
    /// <summary>
    /// Бинарная сериализация.
    /// </summary>
    public interface IBinarySerializationService
    {
        /// <summary>
        /// Сериализация <paramref name="obj"/>.
        /// </summary>
        /// <typeparam name="T">Передаваемый параметр.</typeparam>
        /// <param name="path">Путь.</param>
        /// <param name="obj">Сериализуемый обькт.</param>
        /// <returns></returns>
        Task Serialization<T>(string path, T obj);

        /// <summary>
        /// Десериализация.
        /// </summary>
        /// <typeparam name="T">Передаваемый тип.</typeparam>
        /// <param name="path">Путь.</param>
        /// <returns>Обьект.</returns>
        Task<T> Deserialization<T>(string path);
    }
}

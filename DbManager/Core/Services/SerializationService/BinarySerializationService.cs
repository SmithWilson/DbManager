using DbManager.Core.DbProvider.Datacontext.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Core.Services.SerializationService
{
    /// <summary>
    /// Бинарная сериализация.
    /// </summary>
    public class BinarySerializationService : IBinarySerializationService
    {
        /// <summary>
        /// Десериализация.
        /// </summary>
        /// <typeparam name="T">Передаваемый тип.</typeparam>
        /// <param name="path">Путь.</param>
        /// <returns></returns>
        public Task<T> Deserialization<T>(string path)
        {
            return Task.Run(() =>
            {
                try
                {
                    if (String.IsNullOrWhiteSpace(path))
                    {
                        throw new ArgumentNullException(nameof(path));
                    }

                    using (var fs = new FileStream(path, FileMode.Open))
                    {
                        var formatter = new BinaryFormatter();

                        var deserializeObject = (T)formatter.Deserialize(fs);

                        return deserializeObject;
                    }
                }
                catch (Exception ex)
                {
                    Debugger.Break();
                    return default;
                }
            });
        }

        /// <summary>
        /// Сериализация.
        /// </summary>
        /// <typeparam name="T">Передаваемый тип.</typeparam>
        /// <param name="path">Путь.</param>
        /// <param name="obj">Обьект.</param>
        /// <returns></returns>
        public Task Serialization<T>(string path, T obj)
        {
            return Task.Run(() =>
            {
                try
                {
                    if (obj == null)
                    {
                        throw new ArgumentNullException(nameof(obj));
                    }

                    using (var fs = new FileStream(path, FileMode.OpenOrCreate))
                    {
                        var formatter = new BinaryFormatter();
                        formatter.Serialize(fs, obj);
                    }
                }
                catch (Exception ex)
                {
                    Debugger.Break();
                    return;
                }
            });
        }
    }
}

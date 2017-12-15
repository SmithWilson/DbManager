using DbManager.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Core.Services.Extension
{
    public static class MigrationHelper
    {
        /// <summary>
        /// Подготовка данных <paramref name="collection"/> для экспорта.
        /// </summary>
        /// <typeparam name="T">Передаваемый тип.</typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this List<T> collection)
        {
            var dataTable = new DataTable(typeof(T).Name);

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                dataTable.Columns.Add(property.Name);
            }

            foreach (var item in collection)
            {
                var values = new object[properties.Length];
                for (var i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        /// <summary>
        /// Подготовка электронных документов для экспорта.
        /// </summary>
        /// <param name="collection">Исходные данные.</param>
        /// <returns>Файлы.</returns>
        public static List<FileInfo> ToFileInfo(this List<Facility> collection)
        {
            var exportFiles = new List<FileInfo>();
            foreach (var item in collection)
            {
                if (item.ElectronicVersion != null)
                {
                    exportFiles.Add(new FileInfo
                    {
                        ArchiveNumber = item.ArchiveNumber,
                        FileName = item.NameElectronicVersion,
                        File = item.ElectronicVersion
                    });
                }
            }

            return exportFiles;
        }
    }
}

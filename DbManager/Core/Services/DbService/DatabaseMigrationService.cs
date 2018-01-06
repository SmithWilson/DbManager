using DbManager.Core.DbProvider.Datacontext;
using DbManager.Core.DbProvider.Datacontext.Interfaces;
using DbManager.Core.Services.SerializationService;
using DbManager.Models;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Core.Services.DbService
{
    public class DatabaseMigrationService : IDatabaseMigrationService
    {
        #region Fields
        private IFacilityService _facilityService;
        private IBinarySerializationService _serializationService;

        private ManagerContext _context;
        #endregion


        #region Ctors
        public DatabaseMigrationService()
        {
            _facilityService = new FacilityService();
            _serializationService = new BinarySerializationService();

            _context = ManagerContext.Instance;
        }
        #endregion


        #region Methods
        /// <summary>
        /// Экспорт данных.
        /// </summary>
        /// <param name="dataTable">Таблица.</param>
        /// <param name="files">Файлы.</param>
        /// <returns></returns>
        public Task Export(DataTable dataTable, List<Models.FileInfo> files)
        {
            return Task.Run(() =>
            {
                try
                {
                    //Путь новой папки, для хранения данных.
                    var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), $"Export_{DateTime.Now.ToShortDateString()}_{DateTime.Now.Hour.ToString()}_Hour");
                    //Если директория(папка) отсутствует, то создается новая.
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var workbook = new Workbook();

                    var worksheet = workbook.Worksheets[0];
                    worksheet.InsertDataTable(dataTable, true, 1, 1);
                    //Сохранение бд в файл Database_backup_copy.xls.
                    workbook.SaveToFile(Path.Combine(path, $"Database_backup_copy.xls"));

                    //Бинарное преобразование файлов с последующим сохранением.
                    foreach (var file in files)
                    {
                        _serializationService.Serialization(Path.Combine(path, $"{file.ArchiveNumber}.txt"), file);
                    }
                }
                catch (Exception)
                {
                    Debugger.Break();
                    return;
                }
            });
        }

        /// <summary>
        /// Импорт бд.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task Import(string path)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                return;
            }

            try
            {
                var workbook = new Workbook();

                workbook.LoadFromFile(path);
                
                //Импорт таблицы данных.
                var dataTable = workbook.Worksheets[0].ExportDataTable();
                if (dataTable.Rows != null)
                {
                    _context.Facilitys.RemoveRange(_context.Facilitys);
                    _context.SaveChanges();
                    await _facilityService.Reset();
                }

                //Построчный проход по записям.
                for (var i = 0; i < dataTable.Rows.Count; i++)
                {
                    DataRow data = dataTable.Rows[i];

                    DateTime date;
                    //Если дата не указана, то задается по умолчанию 01.01.1970.
                    if (string.IsNullOrWhiteSpace(data["Date"].ToString()))
                    {
                        date = DateTime.Parse("01.01.1970");
                    }
                    else
                    {
                        date = DateTime.Parse(data["Date"].ToString());
                    }

                    //Считывание данных с строки.
                    var facility = new Facility
                    {
                        ArchiveNumber = Convert.ToInt32(data["ArchiveNumber"]),
                        Series = data["Series"].ToString(),
                        Client = data["Client"].ToString(),
                        Conclusion = data["Conclusion"].ToString(),
                        Executor = data["Executor"].ToString(),
                        Name = data["Name"].ToString(),
                        PlaceInArchive = data["PlaceInArchive"].ToString(),
                        Treaty = data["Treaty"].ToString(),
                        IsElectronicVersion = Convert.ToBoolean(data["IsElectronicVersion"]),
                        NameElectronicVersion = data["NameElectronicVersion"].ToString(),
                        Date = date
                    };

                    //Сохранение в бд.
                    await _facilityService.Add(facility);
                }
            }
            catch (Exception)
            {
                Debugger.Break();
                return;
            }
        }
        
        /// <summary>
        /// Импорт файлов.
        /// </summary>
        /// <param name="paths">Путь.</param>
        /// <returns></returns>
        public Task ImportFiles(List<string> paths)
        {
            return Task.Run(async() =>
            {
                if (paths == null)
                {
                    return;
                }

                try
                {
                    //Поочередное считывание файлов.
                    foreach (var path in paths)
                    {
                        //Десериализация информации о файле.
                        var file = await _serializationService.Deserialization<Models.FileInfo>(path);

                        //Если документ поврежден/неверный, то переходим к следующей итерации
                        if (file == null)
                        {
                            continue;
                        }

                        //Получение обьекта(постройки) по архивному номеру.
                        var facility = await _facilityService.GetByArchiveNumber(file.ArchiveNumber);

                        //Если отсутствует, то переходим к следующей итерации
                        if (facility == null)
                        {
                            continue;
                        }

                        //Добавление к обькту электронной версии файла.
                        facility.ElectronicVersion = file.File;
                        facility.NameElectronicVersion = file.FileName;
                        facility.IsElectronicVersion = true;

                        //Обновление обьекта в бд.
                        await _facilityService.Change(facility);
                    }
                }
                catch (Exception)
                {
                    Debugger.Break();
                    return;
                }
            });
        }
        #endregion
    }
}

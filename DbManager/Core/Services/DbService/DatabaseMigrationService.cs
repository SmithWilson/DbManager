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
        //Десериализация файлов.
        public Task Export(DataTable dataTable, List<Models.FileInfo> files)
        {
            return Task.Run(() =>
            {
                try
                {
                    var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), $"Export_{DateTime.Now.ToShortDateString()}_{DateTime.Now.Hour.ToString()}_Hour");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var workbook = new Workbook();

                    var worksheet = workbook.Worksheets[0];
                    worksheet.InsertDataTable(dataTable, true, 1, 1);
                    workbook.SaveToFile(Path.Combine(path, $"Database_backup_copy.xls"));

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

        //Сериализация файлов.
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

                var dataTable = workbook.Worksheets[0].ExportDataTable();
                if (dataTable.Rows != null)
                {
                    _context.Facilitys.RemoveRange(_context.Facilitys);
                    _context.SaveChanges();
                    await _facilityService.Reset();
                }

                for (var i = 0; i < dataTable.Rows.Count; i++)
                {
                    DataRow data = dataTable.Rows[i];

                    DateTime date;
                    if (string.IsNullOrWhiteSpace(data["Date"].ToString()))
                    {
                        date = DateTime.Parse("01.01.1970");
                    }
                    else
                    {
                        date = DateTime.Parse(data["Date"].ToString());
                    }

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

                    await _facilityService.Add(facility);
                }
            }
            catch (Exception)
            {
                Debugger.Break();
                return;
            }
        }

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
                    foreach (var path in paths)
                    {
                        var file = await _serializationService.Deserialization<Models.FileInfo>(path);
                        var facility = await _facilityService.GetByArchiveNumber(file.ArchiveNumber);

                        facility.ElectronicVersion = file.File;
                        facility.NameElectronicVersion = file.FileName;
                        facility.IsElectronicVersion = true;

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

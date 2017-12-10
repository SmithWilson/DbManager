using DbManager.Core.DbProvider.Datacontext;
using DbManager.Core.DbProvider.Datacontext.Interfaces;
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
        #endregion


        #region Ctors
        public DatabaseMigrationService()
        {
            _facilityService = new FacilityService();
        }
        #endregion


        #region Methods
        public Task Export(DataTable dataTable)
        {
            return Task.Run(() =>
            {
                try
                {
                    var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Database_backup_copy.xls");
                    var workbook = new Workbook();

                    var worksheet = workbook.Worksheets[0];
                    worksheet.InsertDataTable(dataTable, true, 1, 1);
                    workbook.SaveToFile(path);
                }
                catch (Exception)
                {
                    Debugger.Break();
                    return;
                }
            });
        }

        public Task Import(string path)
        {
            return Task.Run(async () =>
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
                        await _facilityService.Reset();
                    }

                    for (var i = 0; i < dataTable.Rows.Count; i++)
                    {
                        DataRow data = dataTable.Rows[i];

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
                            NameElectronicVersion = data["NameElectronicVersion"].ToString()
                        };

                        await _facilityService.Add(facility);
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

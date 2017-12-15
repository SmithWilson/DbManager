using DbManager.Core.DbProvider.Datacontext;
using DbManager.Core.DbProvider.Datacontext.Interfaces;
using DbManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DbManager.Core.Services.DbService
{
    public class FacilityService : IFacilityService
    {
        #region Fields
        private ManagerContext _context => ManagerContext.Instance;
        #endregion


        #region Methods
        public Task Add(Facility facility)
        {
            if (facility == null)
            {
                throw new ArgumentNullException(nameof(facility));
            }

            return Task.Run(() =>
            {
                try
                {
                    _context.Facilitys.Add(facility);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Не удалось добавить новую запись", ex);
                }
            });
        }

        public Task Change(Facility facility)
        {
            if (facility == null)
            {
                throw new ArgumentNullException(nameof(facility));
            }

            return Task.Run(() =>
            {
                try
                {
                    var newFacility = _context.Facilitys.SingleOrDefault(f => f.Id == facility.Id);
                    if (newFacility != null)
                    {
                        var faciltyType = newFacility.GetType();
                        foreach (var item in faciltyType.GetProperties().Skip(1))
                        {
                            item.SetValue(newFacility, faciltyType.GetProperty(item.Name).GetValue(facility));
                        }
                        _context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Не удалось обновить существующую запись", ex);
                }
            });
        }

        public Task<DbRawSqlQuery<Facility>> GetResultQuery()
        {
            return Task.Run(() =>
            {
                try
                {
                    return _context.Database.SqlQuery<Facility>("SELECT * FROM Facilities");
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Во время получения данных произошла ошибка", ex);
                }
            });
        }


        public Task<List<Facility>> GetList()
        {
            return Task.Run(() =>
            {
                try
                {
                    return _context.Facilitys.ToList();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Во время получения данных произошла ошибка", ex);
                }
            });
        }

        public Task<IEnumerable<Facility>> Get(int count, int offset)
        {
            return Task.Run(() =>
            {
                try
                {
                    return _context.Facilitys.ToList().Skip(offset).Take(count);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Во время получения данных произошла ошибка", ex);
                }
            });
        }

        public Task<Facility> GetById(int id) =>
            Task.Run(() => _context.Facilitys.SingleOrDefault(f => f.Id == id));

        public Task<List<Facility>> GetByTreaty(string pattern) =>
            Task.Run(() => _context.Facilitys.Where(f => f.Treaty.Contains(pattern)).ToList());

        public Task<Facility> GetByArchiveNumber(int archiveNumber) =>
            Task.Run(() => _context.Facilitys.SingleOrDefault(f => f.ArchiveNumber == archiveNumber));

        public Task Remove(int id)
        {
            return Task.Run(() =>
            {
                try
                {
                    var facility = _context.Facilitys.SingleOrDefault(f => f.Id == id);
                    _context.Facilitys.Remove(facility);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Во время удаления данных произошла ошибка", ex);
                }
            });
        }

        public Task SaveOrUpdate(Facility update)
        {
            if (update == null)
            {
                throw new ArgumentNullException(nameof(update));
            }

            return Task.Run(async() =>
            {
                try
                {
                    var facility = _context.Facilitys.SingleOrDefault(f => f.Id == update.Id);
                    if (facility == null)
                    {
                        await Add(update);
                    }
                    else
                    {
                        await Change(update);
                    }
                }
                catch (Exception ex)
                {
                    Debugger.Break();
                    throw new InvalidOperationException($"Во время обновления данных произошла ошибка", ex);
                }
            });
        }

        public Task Reset()
            => Task.Run(() =>
                {
                    _context.Database.ExecuteSqlCommand("TRUNCATE TABLE Facilities");
                    _context.SaveChanges();
                }); 
        #endregion
    }
}

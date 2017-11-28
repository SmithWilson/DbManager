using DbManager.Core.DbProvider.Datacontext.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbManager.Models;
using DbManager.Core.DbProvider.Datacontext;
using System.Diagnostics;

namespace DbManager.Core.Services.DbService
{
    public class FacilityService : IFacilityService
    {
        public Task Add(Facility facility)
        {
            return Task.Run(() =>
            {
                return Task.Run(() =>
                {
                    try
                    {
                        if (facility == null)
                        {
                            return;
                        }

                        ManagerContext.Instance.Facilitys.Add(facility);
                        ManagerContext.Instance.SaveChanges();
                    }
                    catch (Exception)
                    {
                        Debugger.Break();
                    }
                });
            });
        }

        public Task Change(Facility facility)
        {
            return Task.Run(() =>
            {
                try
                {
                    var obj = ManagerContext.Instance.Facilitys.SingleOrDefault(f => f.Id == facility.Id);
                    if (obj == null)
                    {
                        return;
                    }

                    //TODO: Переписать это говно на рефлексию, которая будет менять в бд.
                    obj.Name = facility.Name;
                    obj.Series = facility.Series;
                    obj.Treaty = facility.Treaty;
                    obj.Executor = facility.Executor;
                    obj.Data = facility.Data;
                    obj.Conclusion = facility.Conclusion;
                    obj.Client = facility.Client;
                    obj.ArchiveNumber = facility.ArchiveNumber;

                    ManagerContext.Instance.SaveChanges();
                }
                catch (Exception)
                {
                    Debugger.Break();
                }
            });
        }

        public Task<List<Facility>> Get()
        {
            return Task.Run(() =>
            {
                try
                {
                    return ManagerContext.Instance.Facilitys.ToList();
                }
                catch (Exception)
                {
                    Debugger.Break();
                    return null;
                }
            });
        }

        public Task<List<Facility>> Get(int count, int offset)
        {
            return Task.Run(() =>
            {
                try
                {
                    return ManagerContext.Instance.Facilitys
                    .Skip(offset)
                    .Take(count)
                    .ToList();
                }
                catch (Exception)
                {
                    Debugger.Break();
                    return null;
                }
            });
        }

        public Task<Facility> GetById(int id)
        {
            return Task.Run(() =>
            {
                try
                {
                    return ManagerContext.Instance.Facilitys.SingleOrDefault(f => f.Id == id);
                }
                catch (Exception)
                {
                    Debugger.Break();
                    return null;
                }
            });
        }

        public Task Remove(int id)
        {
            return Task.Run(() =>
            {
                try
                {
                    var facility = ManagerContext.Instance.Facilitys.SingleOrDefault(f => f.Id == id);
                    ManagerContext.Instance.Facilitys.Remove(facility);
                    ManagerContext.Instance.SaveChanges();
                }
                catch (Exception)
                {
                    Debugger.Break();
                }
            });
        }
    }
}

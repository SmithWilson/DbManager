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
        }

        public Task Change(Facility facility)
        {
            return Task.Run(() =>
            {
                try
                {
                    if (ManagerContext.Instance.Facilitys.SingleOrDefault(f => f.Id == facility.Id) == null)
                    {
                        return;
                    }

                    foreach (var item in ManagerContext.Instance.Facilitys.SingleOrDefault(f => f.Id == facility.Id).GetType().GetProperties().Skip(1))
                    {
                        item.SetValue(ManagerContext.Instance.Facilitys.SingleOrDefault(f => f.Id == facility.Id), facility.GetType().GetProperty(item.Name).GetValue(facility));
                    }

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

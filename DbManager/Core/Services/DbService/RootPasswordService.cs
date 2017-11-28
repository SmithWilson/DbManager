using DbManager.Core.DbProvider.Datacontext;
using DbManager.Core.DbProvider.Datacontext.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbManager.Models;

namespace DbManager.Core.Services.DbServices
{
    public class RootPasswordService : IRootPasswordService
    {

        public Task<RootPassword> Get()
        {
            return Task.Run(() =>
            {
                return ManagerContext.Instance.Passwords.FirstOrDefault();
            });
        }

        public Task Change(string before, string after)
        {
            return Task.Run(() =>
            {
                ManagerContext.Instance.Passwords.SingleOrDefault(p => p.Password == before).Password = after;
                ManagerContext.Instance.SaveChanges();
            });
        }

        public Task<bool> Rigth(string password)
        {
            return Task.Run(() =>
            {
                return ManagerContext.Instance.Passwords.FirstOrDefault().Password == password ? true : false;
            });
        }
    }
}

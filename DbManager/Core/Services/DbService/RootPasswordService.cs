using DbManager.Core.DbProvider.Datacontext;
using DbManager.Core.DbProvider.Datacontext.Interfaces;
using DbManager.Models;
using System.Linq;
using System.Threading.Tasks;

namespace DbManager.Core.Services.DbServices
{
    public class RootPasswordService : IRootPasswordService
    {
        private ManagerContext _context => ManagerContext.Instance;

        public Task<RootPassword> Get() =>
            Task.Run(() => _context.Passwords.FirstOrDefault());

        public Task Change(string before, string after)
        {
            return Task.Run(() =>
            {
                var record = _context.Passwords.SingleOrDefault(p => p.Password == before) ?? throw new RecordNotFoundException();
                record.Password = after;
                _context.SaveChanges();
            });
        }

        public Task<bool> Rigth(string password)
        {
            return Task.Run(() =>
            {
                return _context.Passwords.FirstOrDefault()?.Password == password ? true : false;
            });
        }
    }
}

using DbManager.Core.DbProvider.Datacontext;
using DbManager.Core.DbProvider.Datacontext.Interfaces;
using DbManager.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DbManager.Core.Services.DbService
{
    public class RootPasswordService : IRootPasswordService
    {
        #region Fields
        private ManagerContext _context => ManagerContext.Instance;
        #endregion

        #region Methods
        /// <summary>
        /// Получение пароля.
        /// </summary>
        /// <returns></returns>
        public Task<RootPassword> Get() =>
            Task.Run(() => _context.Passwords.FirstOrDefault());

        /// <summary>
        /// Изменение пароля.
        /// </summary>
        /// <param name="before">Старый.</param>
        /// <param name="after">Новый.</param>
        /// <returns></returns>
        public Task<bool> Change(string before, string after)
        {
            return Task.Run(() =>
            {
                try
                {
                    var record = _context.Passwords.SingleOrDefault(p => p.Password == before);
                    if (record == null)
                    {
                        return false;
                    }
                    record.Password = after;
                    _context.SaveChanges();

                    return true;
                }
                catch (System.Exception)
                {
                    Debugger.Break();
                    return false;
                }
            });
        }

        /// <summary>
        /// Правильность пароля.
        /// </summary>
        /// <param name="password">Пароль.</param>
        /// <returns></returns>
        public Task<bool> Rigth(string password)
        {
            return Task.Run(() =>
            {
                return _context.Passwords.FirstOrDefault()?.Password == password ? true : false;
            });
        }

        /// <summary>
        /// Сброс пароля.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public Task Reset(string password)
        {
            return Task.Run(() =>
            {
                var pass = _context.Passwords.SingleOrDefault(p => p.Password == password) ?? throw new RecordNotFoundException();
                if (!(pass.Password == password))
                {
                    return;
                }

                pass.Password = DbManager.Properties.Settings.Default.defaultPassword;
                _context.SaveChanges();
            });
        } 
        #endregion
    }
}

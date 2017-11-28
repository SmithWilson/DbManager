using DbManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Core.DbProvider.Datacontext.Interfaces
{
    public interface IRootPasswordService
    {
        /// <summary>
        /// Получение пароля.
        /// </summary>
        /// <returns>Пароль.</returns>
        Task<RootPassword> Get();

        /// <summary>
        /// Изменение.
        /// </summary>
        /// <param name="before">Старый пароль.</param>
        /// <param name="after">Новый пароль.</param>
        /// <returns></returns>
        Task Change(string before, string after);

        /// <summary>
        /// Проверка пароля.
        /// </summary>
        /// <param name="password">Пароль.</param>
        /// <returns>Логическое значение.</returns>
        Task<bool> Rigth(string password);
    }
}

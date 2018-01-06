using DbManager.Models;
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
        Task<bool> Change(string before, string after);

        /// <summary>
        /// Проверка пароля.
        /// </summary>
        /// <param name="password">Пароль.</param>
        /// <returns>Логическое значение.</returns>
        Task<bool> Rigth(string password);

        /// <summary>
        /// Сброс пароля.
        /// </summary>
        /// <param name="password">Текущий пароль.</param>
        /// <returns></returns>
        Task Reset(string password);
    }
}

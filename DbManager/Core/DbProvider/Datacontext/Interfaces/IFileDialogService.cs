using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Core.DbProvider.Datacontext.Interfaces
{
    public interface IFileDialogService
    {
        /// <summary>
        /// Открытие OpenFileDialog.
        /// </summary>
        /// <returns>Путь к файлу.</returns>
        Task<string> OpenDialog();
    }
}

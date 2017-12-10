using System.Threading.Tasks;
using DbManager.Core.DbProvider.Datacontext.Interfaces;
using Microsoft.Win32;
using System;

namespace DbManager.Core.Services.FileService
{
    public class FileDialogService : IFileDialogService
    {
        #region Methods
        public Task<string> OpenDialog()
        {
            return Task.Run(() =>
            {
                var dialog = new OpenFileDialog();
                dialog.Filter = "Документы(*.doc;*.docx;*.pdf;*.txt)|**.doc;*.docx;*.pdf;*.txt" + "|Книга-xls (*.xls)|*.xls" + "|Все файлы (*.*)|*.*";
                dialog.CheckFileExists = true;
                dialog.Multiselect = false;
                if (dialog.ShowDialog() == true)
                {
                    return dialog.FileName ?? "";
                }

                return "";
            });
        } 
        #endregion
    }
}

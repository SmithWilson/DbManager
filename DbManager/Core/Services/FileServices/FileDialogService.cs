using System.Threading.Tasks;
using DbManager.Core.DbProvider.Datacontext.Interfaces;
using Microsoft.Win32;
using System;

namespace DbManager.Core.Services.FileService
{
    public class FileDialogService : IFileDialogService
    {
        public Task<string> OpenDialog()
        {
            return Task.Run(() =>
            {
                var dialog = new OpenFileDialog();
                dialog.Filter = "Файл(*.doc;*.docx;*.pdf;*.txt)|**.doc;*.docx;*.pdf;*.txt" + "|Все файлы (*.*)|*.*";
                dialog.CheckFileExists = true;
                dialog.Multiselect = false;
                if (dialog.ShowDialog() == true)
                {
                    return dialog.FileName;
                }

                return "";
            });
        }
    }
}

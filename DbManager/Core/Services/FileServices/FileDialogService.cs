using System.Threading.Tasks;
using DbManager.Core.DbProvider.Datacontext.Interfaces;
using Microsoft.Win32;
using System;
using System.IO;
using System.Collections.Generic;

namespace DbManager.Core.Services.FileService
{
    public class FileDialogService : IFileDialogService
    {
        /// <summary>
        /// Путь в мои документы.
        /// </summary>
        private string _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "export");

        #region Methods
        public Task<string> OpenDialog()
        {
            return Task.Run(() =>
            {
                var dialog = new OpenFileDialog();
                dialog.Filter = "Документы(*.doc;*.docx;*.pdf;*.txt)|**.doc;*.docx;*.pdf;*.txt" + "|Книга-xls (*.xls)|*.xls" + "|Все файлы (*.*)|*.*";
                dialog.InitialDirectory = _path;
                dialog.CheckFileExists = true;
                dialog.Multiselect = false;

                if (dialog.ShowDialog() == true)
                {
                    return dialog.FileName ?? "";
                }

                return "";
            });
        }

        public Task<List<string>> OpenDialogGetFiles()
        {
            return Task.Run(() =>
            {
                var dialog = new OpenFileDialog();
                dialog.Filter = "Документы(*.txt)|*.txt" + "|Все файлы (*.*)|*.*";
                dialog.InitialDirectory = _path;
                dialog.CheckFileExists = true;
                dialog.Multiselect = true;

                if (dialog.ShowDialog() == true)
                {
                    var files = new List<string>();
                    foreach (var fileNames in dialog.FileNames)
                    {
                        files.Add(fileNames);
                    }
                    return files ?? null;
                }

                return null;
            });
        }
        #endregion
    }
}

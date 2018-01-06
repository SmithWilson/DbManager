using DbManager.Core.DbProvider.Datacontext;
using DbManager.Core.DbProvider.Datacontext.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DbManager.Core.Services.FileService
{
    public class DocFileService : IDocFileService
    {
        #region Fields
        private ManagerContext _context;
        #endregion


        #region Ctors
        public DocFileService()
        {
            _context = ManagerContext.Instance;
        }
        #endregion


        #region Methods
        /// <summary>
        /// Получение документа из бд.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="fileName">Имя файла.</param>
        /// <returns></returns>
        public Task GetDoсFileFromDatabase(int id, string fileName)
        {
            var file = _context.Facilitys.SingleOrDefault(f => f.Id == id)?.ElectronicVersion ?? throw new NullReferenceException();

            return Task.Run(() =>
            {
                try
                {
                    using (var fs = new FileStream(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName), FileMode.Create))
                    {
                        fs.Write(file, 0, file.Length);
                        fs.Close();
                    }
                }
                catch (Exception)
                {
                    Debugger.Break();
                }
            });
        }

        /// <summary>
        /// Добавление файла в бд.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public Task PutDocFileToDatabase(int id, string path)
        {
            var file = _context.Facilitys.SingleOrDefault(f => f.Id == id) ?? throw new FileNotFoundException();
            if (String.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException();
            }

            return Task.Run(() =>
            {
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    file.ElectronicVersion = new byte[fs.Length];
                    fs.Read(file.ElectronicVersion, 0, (int)fs.Length);
                    fs.Close();

                    file.NameElectronicVersion = Path.GetFileName(path);
                    file.IsElectronicVersion = true;

                    _context.SaveChanges();
                }
            });
        } 
        #endregion
    }
}

﻿using DbManager.Core.DbProvider.Datacontext;
using DbManager.Core.DbProvider.Datacontext.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DbManager.Core.Services.FileService
{
    public class DocxFileService : IDocxFileService
    {
        private ManagerContext _context;
        public DocxFileService()
        {
            _context = ManagerContext.Instance;
        }

        //TODO: Ловить его на руже.
        public Task GetDoxcFileFromDatabase(int id, string fileName)
        {
            var file = _context.Facilitys.SingleOrDefault(f => f.Id == id)?.ElectronicVersion ?? throw new NullReferenceException();

            return Task.Run(() =>
            {
                try
                {
                    using (var fs = new FileStream(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName),FileMode.CreateNew))
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

        public Task PutDocxFileToDatabase(int id, string path)
        {
            var file = _context.Facilitys.SingleOrDefault(f => f.Id == id) ?? throw new FileNotFoundException();

            return Task.Run(() =>
            {
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    file.ElectronicVersion = new byte[fs.Length];
                    fs.Read(file.ElectronicVersion, 0, (int)fs.Length);
                    fs.Close();
                    file.IsElectronicVersion = true;
                    _context.SaveChanges();
                }
            });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Models
{
    /// <summary>
    /// Модель для импорта/экспорта электронныъ версий.
    /// </summary>
    [DebuggerDisplay("ArchiveNumber - {ArchiveNumber}, FileName - {FileName}, File - {File}")]
    [Serializable]
    public class FileInfo
    {
        /// <summary>
        /// Архиный номер документа, к котому принадлежит файл.
        /// </summary>
        public int ArchiveNumber { get; set; }

        /// <summary>
        /// Название файла.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Файл.
        /// </summary>
        public byte[] File { get; set; }
    }
}

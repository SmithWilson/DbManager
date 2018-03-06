using System;
using System.ComponentModel;
using System.Diagnostics;

namespace DbManager.Models
{
    [DebuggerDisplay("Id - {Id}, Treaty - {Treaty}")]
    public class Facility : INotifyPropertyChanged
    {
        public int Id { get; set; }

        /// <summary>
        /// Архивный №
        /// </summary>
        public int ArchiveNumber { get; set; }

        /// <summary>
        /// Наименование объекта
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Заказчик
        /// </summary>
        public string Client { get; set; }

        /// <summary>
        /// № Договора
        /// </summary>
        public string Treaty { get; set; }

        /// <summary>
        /// Серия. этажность
        /// </summary>
        public string Series { get; set; }

        /// <summary>
        /// Заключение, архив №
        /// </summary>
        public string Conclusion { get; set; }

        /// <summary>
        /// Дата сдачи в архив
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        ///  Исполнитель
        /// </summary>
        public string Executor { get; set; }

        /// <summary>
        /// Наличие электронной версии
        /// </summary>
        public bool IsElectronicVersion { get; set; }

        /// <summary>
        /// Имя.Расширение электронной версии.
        /// </summary>
        public string NameElectronicVersion { get; set; } = "Электронная версия отсутствует";

        /// <summary>
        /// Электронная версия
        /// </summary>
        public byte[] ElectronicVersion { get; set; }

        /// <summary>
        /// Место в архиве
        /// </summary>
        public string PlaceInArchive { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

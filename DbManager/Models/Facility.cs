using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Models
{
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
        public DateTime Date { get; set; }

        /// <summary>
        ///  Исполнитель
        /// </summary>
        public string Executor { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

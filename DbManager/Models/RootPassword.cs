using System.ComponentModel;
using System.Diagnostics;

namespace DbManager.Models
{
    [DebuggerDisplay("Id - {Id}, Password - {Password}")]
    public class RootPassword : INotifyPropertyChanged
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

using System.ComponentModel;

namespace DbManager.Models
{
    public class RootPassword : INotifyPropertyChanged
    {
        public int Id { get; set; }

        public string Password { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

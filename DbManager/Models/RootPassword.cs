using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager.Models
{
    public class RootPassword : INotifyPropertyChanged
    {
        public int Id { get; set; }

        public string Password { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

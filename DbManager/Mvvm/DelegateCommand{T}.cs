using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DbManager.Mvvm
{
    public class DelegateCommand<T> : ICommand
    {
        #region Fields
        private Action<T> _action;
        #endregion


        #region Ctors
        public DelegateCommand(Action<T> action)
           => _action = action ?? throw new ArgumentNullException(nameof(action));
        #endregion


        #region ICommand Implementation
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
            => true;

        public void Execute(object parameter)
        {
            try
            {
                _action((T)parameter);
            }
            catch (Exception)
            {
                Debugger.Break();
            }
        } 
        #endregion
    }
}

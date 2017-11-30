using DbManager.Core.DbProvider.Datacontext;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DbManager
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ManagerContext _context;
        public App()
        {
            _context = ManagerContext.Instance;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            if (!_context.Passwords.Any())
            {
                _context.Passwords.Add(new Models.RootPassword
                {
                    Password = DbManager.Properties.Settings.Default.defaultPassword
                });
                _context.SaveChanges();
            }
        }
    }
}

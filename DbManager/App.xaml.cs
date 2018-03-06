using DbManager.Core.DbProvider.Datacontext;
using System.Linq;
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
            try
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
            catch (System.Exception)
            {
                OnStartup(e);
            }
        }
    }
}

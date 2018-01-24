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
                MessageBox.Show("Начало создания базы данных");
                if (!_context.Passwords.Any())
                {
                    MessageBox.Show("Я не нашел паролей.");
                    _context.Passwords.Add(new Models.RootPassword
                    {
                        Password = DbManager.Properties.Settings.Default.defaultPassword
                    });
                    MessageBox.Show("Я добавил пароль пытаюсь сохранить.");
                    _context.SaveChanges();
                    MessageBox.Show("База данных создана.");
                }
            }
            catch (System.Exception)
            {
                MessageBox.Show("Я выпадаю при старте и инициализации БД!!!");
                OnStartup(e);
            }
        }
    }
}

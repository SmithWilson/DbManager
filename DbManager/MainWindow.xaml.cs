using DbManager.Core.DbProvider.Datacontext.Interfaces;
using DbManager.Core.Services.DbService;
using DbManager.ViewModels;
using System;
using System.Windows;

namespace DbManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Method();
            var vm = new MainViewModel();
            DataContext = vm;
           
        }

        public async void Method()
        {
            IFacilityService facility = new FacilityService();
            IRootPasswordService root = new RootPasswordService(); 

            //for (var i = 0; i < 10; i++)
            //{
            //    await facility.Add(new Models.Facility
            //    {
            //        ArchiveNumber = i*2,
            //        Series = $"{i} series",
            //        Client = $"{i} client",
            //        Conclusion = $"{i} conclusion",
            //        Date = DateTime.Now,
            //        Executor = $"{i} executor",
            //        Name = $"{i} name",
            //        PlaceInArchive = $"{i} полка {i * 3} ряд",
            //        Treaty = $"{i} treaty"
            //    });
            //}
        }
    }
}

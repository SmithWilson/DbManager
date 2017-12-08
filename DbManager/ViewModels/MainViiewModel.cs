using DbManager.Core.DbProvider.Datacontext;
using DbManager.Core.DbProvider.Datacontext.Interfaces;
using DbManager.Core.Services.DbService;
using DbManager.Core.Services.FileService;
using DbManager.Models;
using DbManager.Mvvm;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DbManager.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Fields
        private ICommand _pushFileToDatabase;
        private ICommand _getFileFromDatabase;

        private IFacilityService _facilityService;
        private IRootPasswordService _rootPasswordService;
        private IFileDialogService _fileDialogService;
        private IDocxFileService _docxFileService;

        private ManagerContext _context;
        #endregion


        #region Ctors
        public MainViewModel()
        {
            _facilityService = new FacilityService();
            _rootPasswordService = new RootPasswordService();
            _fileDialogService = new FileDialogService();
            _docxFileService = new DocxFileService();

            _context = ManagerContext.Instance;

            Facilitys = new ObservableCollection<Facility>();

            Initialization();
        }
        #endregion


        #region Properties
        public ObservableCollection<Facility> Facilitys { get; set; }

        [DoNotNotify]
        public Facility ItemFacility { get; set; }

        public string Search { get; set; }
        #endregion


        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        public async void OnSearchChanged()
        {
                if (String.IsNullOrWhiteSpace(Search))
                {
                    Facilitys = new ObservableCollection<Facility>(await _facilityService.Get());
                    return;
                }
            Facilitys = new ObservableCollection<Facility>(await _facilityService.GetByTreaty(Search));
        }
        #endregion


        #region Commands
        public ICommand PushFileToDatabase => _pushFileToDatabase ??
            (_pushFileToDatabase = new DelegateCommand(async () => await PushFile()));

        public ICommand GetFileFromDatabase => _getFileFromDatabase ??
            (_getFileFromDatabase = new DelegateCommand(async () => await GetFile()));
        #endregion


        #region Non-public Methods
        private async void Initialization()
        {
            foreach (var item in await _facilityService.Get())
            {
                Facilitys.Add(item);
            }

            //await _rootPasswordService.Change("password", "heh");

            //await _rootPasswordService.Reset("heh");
        }

        private async Task PushFile()
        {
            if (ItemFacility == null)
            {
                return;
            }

            try
            {
                var path = await _fileDialogService.OpenDialog();
                await _docxFileService.PutDocxFileToDatabase(ItemFacility.Id, path);
            }
            catch (System.Exception)
            {
                return;
            }
        }

        private async Task GetFile()
        {
            if (ItemFacility == null)
            {
                return;
            }

            try
            {
                await _docxFileService.GetDoxcFileFromDatabase(ItemFacility.Id, ItemFacility.NameElectronicVersion);
            }
            catch (System.Exception)
            {
                Debugger.Break();
            }
        }
        #endregion
    }
}

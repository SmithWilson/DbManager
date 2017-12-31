using DbManager.Core.DbProvider.Datacontext;
using DbManager.Core.DbProvider.Datacontext.Interfaces;
using DbManager.Core.Services.DbService;
using DbManager.Core.Services.Extension;
using DbManager.Core.Services.FileService;
using DbManager.Models;
using DbManager.Mvvm;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DbManager.Core.Services.Printing;

namespace DbManager.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Fields
        private ICommand _pushFileToDatabase;
        private ICommand _getFileFromDatabase;
        private ICommand _addNewFacility;
        private ICommand _saveOrChange;
        private ICommand _annulment;
        private ICommand _remove;
        private ICommand _export;
        private ICommand _import;
        private ICommand _print;

        private IFacilityService _facilityService;
        private IRootPasswordService _rootPasswordService;
        private IFileDialogService _fileDialogService;
        private IDocxFileService _docxFileService;
        private IDatabaseMigrationService _migrationService;

        private ManagerContext _context;
        #endregion


        #region Ctors
        public MainViewModel()
        {
            _facilityService = new FacilityService();
            _rootPasswordService = new RootPasswordService();
            _fileDialogService = new FileDialogService();
            _docxFileService = new DocxFileService();
            _migrationService = new DatabaseMigrationService();

            _context = ManagerContext.Instance;

            Facilitys = new ObservableCollection<Facility>();

            Initialization();
        }
        #endregion


        #region Properties
        public ObservableCollection<Facility> Facilitys { get; set; }

        public Facility ItemFacility { get; set; }

        public string Search { get; set; }

        public bool SearchVisibility { get; set; } = true;
        #endregion


        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        public async void OnSearchChanged()
        {
                if (String.IsNullOrWhiteSpace(Search))
                {
                    SearchVisibility = true;
                    Facilitys = new ObservableCollection<Facility>(await _facilityService.GetResultQuery());
                    return;
                }
            SearchVisibility = false;
            Facilitys = new ObservableCollection<Facility>(await _facilityService.GetByTreaty(Search));
        }
        #endregion


        #region Commands
        public ICommand PushFileToDatabaseCommand => _pushFileToDatabase ??
            (_pushFileToDatabase = new DelegateCommand(async () => await PushFile()));

        public ICommand GetFileFromDatabaseCommand => _getFileFromDatabase ??
            (_getFileFromDatabase = new DelegateCommand(async () => await GetFile()));

        public ICommand AddNewFacilityCommnd => _addNewFacility ??
            (_addNewFacility = new DelegateCommand(() => AddNewFacility()));

        public ICommand SaveOrChangeCommand => _saveOrChange ??
            (_saveOrChange = new DelegateCommand(async() => await SaveOrChange()));

        public ICommand AnnulmentCommand => _annulment ??
            (_annulment = new DelegateCommand(() => Annulment()));

        public ICommand RemoveCommand => _remove ??
            (_remove = new DelegateCommand(async() => await Remove()));

        public ICommand ImportCommand => _import ??
            (_import = new DelegateCommand(async () => await ImportMethod()));

        public ICommand ExportCommand => _export ??
            (_export = new DelegateCommand(async () => await ExportMethod()));

        public ICommand PrintCommand => _print ??
            (_print = new DelegateCommand(async () => await Print()));
        #endregion


        #region Non-public Methods
        private async void Initialization()
        {
            //for (int i = 0; i < 100; i++)
            //{
            //    await _facilityService.Add(new Facility
            //    {
            //        ArchiveNumber = i * 2,
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
            Facilitys.Clear();
            foreach (var item in await _facilityService.GetResultQuery())
            {
                Facilitys.Add(item);
            }

            //await _rootPasswordService.Change("password", "heh");

            //await _rootPasswordService.Reset("heh");
        }

        private void AddNewFacility()
        {
            try
            {
                var facility = new Facility();
                Facilitys.Add(facility);
                ItemFacility = facility;
            }
            catch (Exception ex)
            {
                Debugger.Break();
                return;
            }
        }

        private async Task SaveOrChange()
        {
            try
            {
                if (ItemFacility == null)
                {
                    return;
                }

                await _facilityService.SaveOrUpdate(ItemFacility);
            }
            catch (Exception ex)
            {
                Debugger.Break();
                return;
            }
        }

        private void Annulment()
        {
            try
            {
                Facilitys.Clear(); 
                Initialization();
                ItemFacility = null;
            }
            catch (Exception ex)
            {
                Debugger.Break();
                return;
            }
        }

        private async Task Remove()
        {
            try
            {
                await _facilityService.Remove(ItemFacility.Id);
                Facilitys.Remove(ItemFacility);
            }
            catch (Exception ex)
            {
                Debugger.Break();
                return;
            }
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
                Initialization();
            }
            catch (System.Exception)
            {
                Debugger.Break();
                return;
            }
        }

        private async Task GetFile()
        {
            if (ItemFacility == null || ItemFacility.ElectronicVersion == null)
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
                return;
            }
        }

        private async Task ImportMethod()
        {
            try
            {
                await _migrationService.Import(await _fileDialogService.OpenDialog());
                await _migrationService.ImportFiles(await _fileDialogService.OpenDialogGetFiles());
                Initialization();
            }
            catch (Exception)
            {
                Debugger.Break();
                return;
            }
        }

        private async Task ExportMethod()
        {
            try
            {
                var facilitys = await _facilityService.GetList();
                await _migrationService.Export(facilitys.ToDataTable(), facilitys.ToFileInfo());
            }
            catch (Exception)
            {
                Debugger.Break();
                return;
            }
        }

        private async Task Print()
        {
            try
            {
                PrintData print = new PrintData();
                var facilitys = await _facilityService.GetList();
                print.Print(facilitys.ToDataTable());
            }
            catch (Exception)
            {
                Debugger.Break();
                return;
            }
        }
        #endregion
    }
}
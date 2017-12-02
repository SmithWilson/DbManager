using DbManager.Core.DbProvider.Datacontext;
using DbManager.Core.DbProvider.Datacontext.Interfaces;
using DbManager.Core.Services.DbService;
using DbManager.Core.Services.FileService;
using DbManager.Models;
using DbManager.Mvvm;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DbManager.ViewModels
{
    public class MainViewModel
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

        public Facility ItemFacility { get; set; }
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
        }

        private async Task PushFile()
        {
            var path = await _fileDialogService.OpenDialog();
            await _docxFileService.PutDocxFileToDatabase(ItemFacility.Id, path);
        }

        private async Task GetFile()
        {
            await _docxFileService.GetDoxcFileFromDatabase(ItemFacility.Id, ItemFacility.Name);
        }
        #endregion
    }
}

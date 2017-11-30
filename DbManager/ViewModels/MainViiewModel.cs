using DbManager.Core.DbProvider.Datacontext;
using DbManager.Core.DbProvider.Datacontext.Interfaces;
using DbManager.Core.Services.DbService;
using DbManager.Models;
using System.Collections.ObjectModel;

namespace DbManager.ViewModels
{
    public class MainViewModel
    {
        #region Fields
        private IFacilityService _facilityService;
        private IRootPasswordService _rootPasswordService;

        private ManagerContext _context;
        #endregion

        #region Ctors
        public MainViewModel()
        {
            _facilityService = new FacilityService();
            _rootPasswordService = new RootPasswordService();

            _context = ManagerContext.Instance;
            Facilitys = new ObservableCollection<Facility>();
            Initialization();
        }
        #endregion

        #region Properties
        public ObservableCollection<Facility> Facilitys { get; set; }
        #endregion

        #region Commands

        #endregion

        #region Non-public Methods
        private async void Initialization()
        {
            foreach (var item in await _facilityService.Get())
            {
                Facilitys.Add(item);
            }
        }
        #endregion
    }
}

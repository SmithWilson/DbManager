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
using System.Windows;

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
        private ICommand _printByDate;

        private ICommand _loginAdmin;
        private ICommand _logGuest;
        private ICommand _changePassword;

        private ICommand _openLoginPopUp;
        private ICommand _openChangePopUp;
        private ICommand _openExitPopUp;
        private ICommand _openRemovePopUp;
        private ICommand _printPopUp;
        private ICommand _cancelPopUp;

        private IFacilityService _facilityService;
        private IRootPasswordService _rootPasswordService;
        private IFileDialogService _fileDialogService;
        private IDocFileService _docxFileService;
        private IDatabaseMigrationService _migrationService;

        private ManagerContext _context;
        #endregion


        #region Ctors
        public MainViewModel()
        {
            _facilityService = new FacilityService();
            _rootPasswordService = new RootPasswordService();
            _fileDialogService = new FileDialogService();
            _docxFileService = new DocFileService();
            _migrationService = new DatabaseMigrationService();

            _context = ManagerContext.Instance;

            Facilitys = new ObservableCollection<Facility>();

            Initialization();
        }
        #endregion


        #region Properties
        /// <summary>
        /// Статус пользователя гость/админ.
        /// </summary>
        public bool Root { get; set; }

        /// <summary>
        /// Главная коллекция элементов.
        /// </summary>
        public ObservableCollection<Facility> Facilitys { get; set; }

        /// <summary>
        /// Выбранный элемент коллекции.
        /// </summary>
        public Facility ItemFacility { get; set; }

        /// <summary>
        /// Поиск.
        /// </summary>
        public string Search { get; set; }

        /// <summary>
        /// Отображение подсказки.
        /// </summary>
        public bool SearchVisibility { get; set; } = true;


        /// <summary>
        /// Год для печати.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Пароль для входа.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Смена пароля. Старый пароль.
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// Смена пароля. Новый пароль.
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// Кнопка для входа как администратор.
        /// </summary>
        public bool LoginWithPassword { get; set; } = true;

        /// <summary>
        /// Состояния отображения всплывающих окон true/false.
        /// </summary>
        public bool LoginPopup { get; set; }

        public bool ChangePasswordPopup { get; set; }

        public bool RemovePopup { get; set; }

        public bool PrintPopUp { get; set; }

        public bool ExitPopup { get; set; }
        /// <summary>
        /// END Состояния отображения всплывающих окон true/false.
        /// </summary>
        #endregion


        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Поиск по номеру договора.
        /// </summary>
        //TODO:ДОБАВИТЬ ПОИСК ПО NAME
        public async void OnSearchChanged()
        {
            try
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
            catch (Exception)
            {
				MessageBox.Show(
					$"Во время поиска произошла ошибка. Повторите попытку.",
					"Ошибка",
					MessageBoxButton.OK,
					MessageBoxImage.Warning);
				Search = string.Empty;
                return;
            }
        }
        #endregion


        #region Commands
        public ICommand PushFileToDatabaseCommand => _pushFileToDatabase ??
            (_pushFileToDatabase = new DelegateCommand(async () => await PushFile()));

        public ICommand GetFileFromDatabaseCommand => _getFileFromDatabase ??
            (_getFileFromDatabase = new DelegateCommand(async () => await GetFile()));

        public ICommand AddNewFacilityCommnd => _addNewFacility ??
            (_addNewFacility = new DelegateCommand(AddNewFacility));

        public ICommand SaveOrChangeCommand => _saveOrChange ??
            (_saveOrChange = new DelegateCommand(async() => await SaveOrChange()));

        public ICommand AnnulmentCommand => _annulment ??
            (_annulment = new DelegateCommand(Annulment));

        public ICommand RemoveCommand => _remove ??
            (_remove = new DelegateCommand(async() => await Remove()));

        public ICommand ImportCommand => _import ??
            (_import = new DelegateCommand(async () => await ImportMethod()));

        public ICommand ExportCommand => _export ??
            (_export = new DelegateCommand(async () => await ExportMethod()));

        public ICommand PrintCommand => _print ??
            (_print = new DelegateCommand(async () => await Print()));

        public ICommand PrintByDateCommand => _printByDate ??
            (_printByDate = new DelegateCommand(async () => await PrintByDate()));
        

        public ICommand LoginCommand => _loginAdmin ??
            (_loginAdmin = new DelegateCommand(async () => await Login()));

        public ICommand ChangePasswordCommand => _changePassword ??
            (_changePassword = new DelegateCommand(async () => await ChangePassword()));

        public ICommand LogGuestCommand => _logGuest ??
            (_logGuest = new DelegateCommand(LoginGuest));


        public ICommand OpenLoginPopUp => _openLoginPopUp ??
            (_openLoginPopUp = new DelegateCommand(() => LoginPopup = true));

        public ICommand OpenChangePasswordPopUp => _openChangePopUp ??
            (_openChangePopUp = new DelegateCommand(() => ChangePasswordPopup = true));

        public ICommand OpenExitPopUp => _openExitPopUp ??
            (_openExitPopUp = new DelegateCommand(() => ExitPopup = true));

        public ICommand OpenRemovePopUp => _openRemovePopUp ??
            (_openRemovePopUp = new DelegateCommand(() => RemovePopup = true));
        
        public ICommand OpenPrintPopUp => _printPopUp ??
            (_printPopUp = new DelegateCommand(() => PrintPopUp = true));

        public ICommand PopupCancelCommand => _cancelPopUp ??
            (_cancelPopUp = new DelegateCommand(PopupCancel));
        #endregion


        #region Non-public Methods
        /// <summary>
        /// Обновление коллекции.
        /// </summary>
        private async void Initialization()
        {
            Facilitys.Clear();
            foreach (var item in await _facilityService.GetResultQuery())
            {
                Facilitys.Add(item);
            }
        }

        /// <summary>
        /// Добавление нового обьекта.
        /// </summary>
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
				MessageBox.Show(
					$"Во время дабавления произошла ошибка. Проверьте данные и повторите попытку.",
					"Ошибка",
					MessageBoxButton.OK,
					MessageBoxImage.Warning);
				return;
            }
        }

        /// <summary>
        /// Сохранение или изменение обьекта.
        /// </summary>
        /// <returns></returns>
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
				PopupCancel();
				MessageBox.Show(
					$"Произошла ошибка при обновлении данных. Повторите попытку.\nДетали - {ex.Message}",
					"Ошибка",
					MessageBoxButton.OK,
					MessageBoxImage.Warning);
				return;
            }
        }

        /// <summary>
        /// Отмена.
        /// </summary>
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
				MessageBox.Show(
					$"Повторите попытку.\nДетали - {ex.Message}",
					"Ошибка",
					MessageBoxButton.OK,
					MessageBoxImage.Warning);
				return;
            }
        }

        /// <summary>
        /// Удаление обьекта.
        /// </summary>
        /// <returns></returns>
        private async Task Remove()
        {
            try
            {
                await _facilityService.Remove(ItemFacility.Id);
                Facilitys.Remove(ItemFacility);
                PopupCancel();
            }
            catch (Exception ex)
			{
				PopupCancel();
				MessageBox.Show(
					$"Во время удаления произошла ошибка. Повторите попытку.\nДетали - {ex.Message}",
					"Ошибка",
					MessageBoxButton.OK,
					MessageBoxImage.Warning);
				return;
            }
        }

        /// <summary>
        /// Добавление электронного документа к обьекту.
        /// </summary>
        /// <returns></returns>
        private async Task PushFile()
        {
            if (ItemFacility == null)
            {
                return;
            }

            try
            {
                var path = await _fileDialogService.OpenDialog();
                await _docxFileService.PutDocFileToDatabase(ItemFacility.Id, path);
                Initialization();
            }
            catch (System.Exception ex)
            {
				MessageBox.Show(
					$"Во время загрузки файла произошла ошибка. Повторите попытку.\nДетали - {ex.Message}",
					"Ошибка",
					MessageBoxButton.OK,
					MessageBoxImage.Warning);
				return;
            }
        }

        /// <summary>
        /// Сохранение электронного документа на рабочий стол.
        /// </summary>
        /// <returns></returns>
        private async Task GetFile()
        {
            if (ItemFacility == null || ItemFacility.ElectronicVersion == null)
            {
                return;
            }

            try
            {
                await _docxFileService.GetDoсFileFromDatabase(ItemFacility.Id, ItemFacility.NameElectronicVersion);
            }
            catch (System.Exception ex)
            {
				MessageBox.Show(
					$"Во время получения файла произошла ошибка. Повторите попытку.\nДетали - {ex.Message}",
					"Ошибка",
					MessageBoxButton.OK,
					MessageBoxImage.Warning);
				return;
            }
        }

        /// <summary>
        /// Импорт бд и электронных документов.
        /// </summary>
        /// <returns></returns>
        private async Task ImportMethod()
        {
            try
            {
                var result = await _migrationService.Import(await _fileDialogService.OpenDialog());
				if (result)
				{
					await _migrationService.ImportFiles(await _fileDialogService.OpenDialogGetFiles()); 
				}
                Initialization();
            }
            catch (Exception ex)
            {
				MessageBox.Show(
					$"Во время импорта произошла ошибка. Проверьте данные и повторите попытку.\nДетали - {ex.Message}",
					"Ошибка",
					MessageBoxButton.OK,
					MessageBoxImage.Warning,
					MessageBoxResult.OK,
					MessageBoxOptions.ServiceNotification);
				return;
            }
        }

        /// <summary>
        /// Экспорт бд и электронных документов в Мои Документы Export_Дата_Часы_Hour.
        /// </summary>
        /// <returns></returns>
        private async Task ExportMethod()
        {
            try
            {
                var facilitys = await _facilityService.GetList();
                await _migrationService.Export(facilitys.ToDataTable(), facilitys.ToFileInfo());
            }
            catch (Exception ex)
			{
				MessageBox.Show(
					$"Во время экспорта произошла ошибка. Проверьте данные и повторите попытку.\nДетали - {ex.Message}",
					"Ошибка",
					MessageBoxButton.OK,
					MessageBoxImage.Warning);
				return;
            }
        }

        /// <summary>
        /// Печать.
        /// </summary>
        /// <returns></returns>
        private async Task Print()
        {
            try
            {
                PrintData print = new PrintData();
                var facilitys = await _facilityService.GetList();
                print.Print(facilitys.ToDataTable());
            }
            catch (Exception ex)
			{
				PopupCancel();
				MessageBox.Show(
					$"Во время печати произошла ошибка. Проверьте принтер, соединение, данные и повторите попытку.\nДетали - {ex.Message}",
					"Ошибка",
					MessageBoxButton.OK,
					MessageBoxImage.Warning);
				return;
            }
        }

        /// <summary>
        /// Печать по дате.
        /// </summary>
        /// <returns></returns>
        private async Task PrintByDate()
        {
            try
            {
                PopupCancel();
                PrintData print = new PrintData();
                var facilitys = await _facilityService.GetList();
                print.Print(facilitys.Where(f => f.Date.Value.Year == Year).ToList().ToDataTable());
                Year = 0;
            }
            catch (Exception ex)
			{
				PopupCancel();
				MessageBox.Show(
					$"Во время печати произошла ошибка. Проверьте введеный год и повторите попытку.\nДетали - {ex.Message}",
					"Ошибка",
					MessageBoxButton.OK,
					MessageBoxImage.Warning);
				return;
            }
        }

        /// <summary>
        /// Закрытие всех всплывающих окон и сброс переменных.
        /// </summary>
        private void PopupCancel()
        {
            LoginPopup = false;
            ChangePasswordPopup = false;
            RemovePopup = false;
            ExitPopup = false;
            PrintPopUp = false;
            OldPassword = string.Empty;
            NewPassword = string.Empty;
            Password = string.Empty;
        }

        /// <summary>
        /// Вход как администратор.
        /// </summary>
        /// <returns></returns>
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Password))
            {
                return;
            }

            try
            {
                var right = await _rootPasswordService.Rigth(Password);
                if (!right)
                {
					PopupCancel();
					MessageBox.Show(
					   $"Неверный пароль.",
					   "Ошибка",
					   MessageBoxButton.OK,
					   MessageBoxImage.Warning);
				}
                else
                {
                    LoginPopup = false;
                    LoginWithPassword = false;
                    Root = true;
                    Password = string.Empty;
                }
            }
            catch (Exception)
            {
                Debugger.Break();
                return;
            }
        }

        /// <summary>
        /// Смена пароля.
        /// </summary>
        /// <returns></returns>
        private async Task ChangePassword()
        {
            try
            {
                var result = await _rootPasswordService.Change(OldPassword, NewPassword);
                if (!result)
				{
					PopupCancel();
					MessageBox.Show(
					$"Неверный пароль.",
					"Ошибка",
					MessageBoxButton.OK,
					MessageBoxImage.Warning);
				}
                else
                {
                    ChangePasswordPopup = false;
                    OldPassword = string.Empty;
                    NewPassword = string.Empty;
                }
            }
            catch (Exception)
            {
                Debugger.Break();
                return;
            }
        }

        /// <summary>
        /// Выход.
        /// </summary>
        private void LoginGuest()
        {
            try
            {
                LoginWithPassword = true;
                Root = false;
                ExitPopup = false;
            }
            catch (Exception ex)
			{
				PopupCancel();
				MessageBox.Show(
					$"Ошибка авторизации, как гость.\nДетали - {ex.Message}",
					"Ошибка",
					MessageBoxButton.OK,
					MessageBoxImage.Warning);
				return;
            }
        }
        #endregion
    }
}
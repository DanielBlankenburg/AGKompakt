using BGH_Kompakt.Classes._LookUp.UserLookUps;
using BGH_Kompakt.Classes.UserClasses;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.Services.UserService;
using BGH_Kompakt.Views.UserLogin;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows.Input;

namespace BGH_Kompakt.ViewModel.SystemSettings
{
    public class AdminViewModel : ViewModelBase
    {
        public UserDBContext userDBContext = new UserDBContext();

        private ObservableCollection<User> _UserList = new ObservableCollection<User>();
        public ObservableCollection<User> UserList { get { return _UserList; } }

        private User _selectedUser;

        public User SelectedUser
        {
            get { return _selectedUser; }
            set 
            { 
                SetProperty<User>(ref _selectedUser, value);
                SetAdminUser();
                SetAdminAll();
            }
        }

        private ObservableCollection<AdminStatus> _AdminAll = new ObservableCollection<AdminStatus>();
        public ObservableCollection<AdminStatus> AdminAll { get { return _AdminAll; } }

        private AdminStatus _SelectedAdminAll;

        public AdminStatus SelectedAdminAll
        {
            get { return _SelectedAdminAll; }
            set
            {
                SetProperty<AdminStatus>(ref _SelectedAdminAll, value);
            }
        }

        private ObservableCollection<AdminStatus> _AdminUser = new ObservableCollection<AdminStatus>();
        public ObservableCollection<AdminStatus> AdminUser { get { return _AdminUser; } }

        private AdminStatus _SelectedAdminUser;

        public AdminStatus SelectedAdminUser
        {
            get { return _SelectedAdminUser; }
            set
            {
                SetProperty<AdminStatus>(ref _SelectedAdminUser, value);
            }
        }



        public ICommand AddCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand SenatAddCommand { get; set; }
        public ICommand SenatRemoveCommand { get; set; }
        public ICommand QuitCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand ListViewDblClickCommand { get; set; }


        public AdminViewModel() 
        {
            AddCommand = new RelayCommand(AddExecute, AddCanExecute);
            RemoveCommand = new RelayCommand(RemoveExecute, RemoveCanExecute);
            QuitCommand = new RelayCommand(QuitExecute);
            SaveCommand = new RelayCommand(SaveExecute);
            ListViewDblClickCommand = new RelayCommand(ListViewDblClickExecute);
            var query = userDBContext.Users.Include(x => x.AdminStatus).OrderBy(x => x.NachName).ThenBy(x => x.VorName);
            foreach (var user in query) UserList.Add(user);
        }

        private void ListViewDblClickExecute(object obj)
        {
            ViewManager.PageInfo.SelectedUser = SelectedUser;
            ViewManager.PageInfo.UserSettingType = 1;
            ViewManager.ShowPageOnMainView<UserPropertyView>();
        }


        private void SaveExecute(object obj)
        {
            userDBContext.SaveChanges();
            //UserManager.RegistratedUser.ShowSitzungsunterlagenSet();
            //UserManager.RegistratedUser.ShowMontagspostSet();
            //UserManager.RegistratedUser.ShowActivityRequestsSet();
            //ViewManager.MainWindowViewModel.ShowSitzungsunterlagen = UserManager.RegistratedUser.ShowSitzungsunterlagen;
            //ViewManager.MainWindowViewModel.ShowMontagspost = UserManager.RegistratedUser.ShowMontagspost;
            //ViewManager.MainWindowViewModel.ShowNebentaetigkeiten = UserManager.RegistratedUser.ShowActivityRequests;
            ViewManager.ShowMainInfoFlyout("Die Änderungen wurden gespeichert.", false);
        }

        private void QuitExecute(object obj)
        {
            //ViewManager.ShowPageOnMainView<SettingsView>();
        }

        private bool AddCanExecute(object obj)
        {
            return SelectedAdminAll != null;
        }
        private bool RemoveCanExecute(object obj)
        {
            return SelectedAdminUser != null;
        }

        private void AddExecute(object obj)
        {
            try
            {
                _AdminUser.Add(SelectedAdminAll);
                SelectedUser.AdminStatus.Add(SelectedAdminAll);
                _AdminUser.OrderBy(s => s.AdminStatusID);
                if (SelectedUser.UserId == UserManager.RegistratedUser.UserId)
                {
                    User AddUser = userDBContext.Users.Find(SelectedUser.UserId);
                    if (AddUser != null)
                    {
                        AdminStatus AddStatus = userDBContext.AdminStatus.Find(SelectedAdminAll.AdminStatusID);
                        AddUser.AdminStatus.Add(AddStatus);
                        UserManager.RegistratedUser = AddUser;
                    }

                }
                _AdminAll.Remove(SelectedAdminAll);
            }
            catch (Exception ex)
            {
                ViewManager.ShowMainInfoFlyout($"Die Rolle konnte nicht zugewiesen werden. Es ist folgender Fehler aufgetreten: {ex.Message}", false);
            } 
        }
        private void RemoveExecute(object obj)
        {
            try
            {
                _AdminAll.Add(SelectedAdminUser);
                SelectedUser.AdminStatus.Remove(SelectedAdminUser);
                _AdminAll.OrderBy(s => s.AdminStatusID);
                _AdminUser.Remove(SelectedAdminUser);
                if (SelectedUser.UserId == UserManager.RegistratedUser.UserId) UserManager.RegistratedUser.AdminStatus.Remove(SelectedAdminUser);
            }
            catch (Exception ex)
            {
                ViewManager.ShowMainInfoFlyout($"Die Rolle konnte nicht entfernt werden. Es ist folgender Fehler aufgetreten: {ex.Message}", false);
            }            
        }



        private void SetAdminUser()
        {
            AdminUser.Clear();
            var query = SelectedUser.AdminStatus;
            if (query != null) foreach (var Status in query) AdminUser.Add(Status);
        }

        private void SetAdminAll()
        {
            AdminAll.Clear();
            var query = userDBContext.AdminStatus;
            foreach (AdminStatus Status in query) if (AdminUser.Where(x => x.AdminStatusID == Status.AdminStatusID).FirstOrDefault() == null) AdminAll.Add(Status);
                
        }


    }
}

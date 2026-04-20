using BGH_Kompakt.Classes._LookUp.UserLookUps;
using BGH_Kompakt.Classes.Helper;
using BGH_Kompakt.Classes.UserClasses;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.Services.UserService;
using BGH_Kompakt.Views.Start;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows.Input;

namespace BGH_Kompakt.ViewModel.Userlogin
{
    public partial class UserPropertyViewModel : ViewModelBase
    {

        public string Titel { get; set; }

        public UserDBContext userDBcontext = new UserDBContext();

        public string Introduction { get; set; }
        public ICommand QuitCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand NewUserDienstbezeichnungenCommand { get; set; }
        public ICommand AddUserDienstbezeichnungenCommand { get; set; }
        public ICommand DeleteUserDienstbezeichnungenCommand { get; set; }


        public User CurrenUser { get; set; }
        public Titel SelectedTitel { get; set; }
        public ObservableCollection<Titel> TitelList
        {
            get
            {
                var tempList = new ObservableCollection<Titel>(userDBcontext.Titel.ToList());
                SelectedTitel = tempList.FirstOrDefault(item => item.TitelId == CurrenUser.TitelId);
                return tempList;
            }
        }
        public Geschlecht SelectedGeschlecht { get; set; }
        public ObservableCollection<Geschlecht> GeschlechtList
        {
            get
            {
                var tempList = new ObservableCollection<Geschlecht>(userDBcontext.Geschlechter.ToList());
                SelectedGeschlecht = tempList.FirstOrDefault(item => item.GeschlechtID == CurrenUser.GeschlechtID);
                return tempList;
            }
        }
        public Status SelectedStatus { get; set; }
        public ObservableCollection<Status> StatusList
        {
            get
            {
                var tempList = new ObservableCollection<Status>(userDBcontext.Status.ToList());
                SelectedStatus = tempList.FirstOrDefault(item => item.StatusId== CurrenUser.StatusId);
                return tempList;
            }
        }

        private Position _SelectedPosition;
        public Position SelectedPosition
        {
            get { return _SelectedPosition; }
            set 
            { 
                SetProperty(ref _SelectedPosition, value);
                ShowDienstbezeichnung = (value.PositionId  == 1);
            }
        }

        public ObservableCollection<Position> PositionList
        {
            get
            {
                var tempList = new ObservableCollection<Position>(userDBcontext.Positions.ToList());
                SelectedPosition = tempList.FirstOrDefault(item => item.PositionId== CurrenUser.PositionId);
                return tempList;
            }
        }

        private Dienstbezeichnung _SelectedDienstbezeichnung;
        public Dienstbezeichnung SelectedDienstbezeichnung
        {
            get { return _SelectedDienstbezeichnung; }
            set { SetProperty(ref _SelectedDienstbezeichnung, value); }
        }


        public ObservableCollection<Dienstbezeichnung> DienstbezeichnungList
        {
            get
            {
                var tempList = new ObservableCollection<Dienstbezeichnung>(userDBcontext.Dienstbezeichnungen.ToList());
                //SelectedDienstbezeichnung = tempList.FirstOrDefault(item => item.DienstbezeichnungId== CurrenUser.DienstbezeichnungId);
                return tempList;
            }
        }
        public ObservableCollection<UserDienstbezeichnung> UserDienstbezeichnungenList { get; set; }

        private UserDienstbezeichnung _SelectedUserDienstbezeichnung = new UserDienstbezeichnung();
        public UserDienstbezeichnung SelectedUserDienstbezeichnung
        {
            get { return _SelectedUserDienstbezeichnung; }
            set { SetProperty(ref _SelectedUserDienstbezeichnung, value); }
        }

        private bool _ShowDienstbezeichnung;
        public bool ShowDienstbezeichnung
        {
            get { return _ShowDienstbezeichnung; }
            set { SetProperty(ref _ShowDienstbezeichnung, value);}
        }

        public UserPropertyViewModel()
        {
            Titel = "Benutzerdaten ändern";


            Introduction = $"Bitte ändern Sie {(ViewManager.PageInfo.UserSettingType == 0 ? "Ihre" : "die")} Nutzerdaten in den Feldern ab.";
            int UserID = ViewManager.PageInfo.UserSettingType == 0 ? UserManager.RegistratedUser.UserId : ViewManager.PageInfo.SelectedUser.UserId;
            CurrenUser = userDBcontext.Users.Where(a => a.UserId == UserID).FirstOrDefault();


            QuitCommand = new RelayCommand(QuitExecute);
            SaveCommand = new RelayCommand(SaveExecute);
            AddUserDienstbezeichnungenCommand = new RelayCommand(AddUserDienstbezeichnungenExecute);
            NewUserDienstbezeichnungenCommand = new RelayCommand(NewUserDienstbezeichnungenExecute);
            DeleteUserDienstbezeichnungenCommand = new RelayCommand(DeleteUserDienstbezeichnungenExecute);

            FillUserDienstleistungen();
        }

        private void FillUserDienstleistungen()
        {
            var Query = userDBcontext.UserDienstbezeichnungen.Where(ud => ud.UserId == CurrenUser.UserId).Include(ud => ud.Dienstbezeichnung);
            UserDienstbezeichnungenList = new ObservableCollection<UserDienstbezeichnung>();
            foreach (var item in Query) UserDienstbezeichnungenList.Add(item);
        }

        private void DeleteUserDienstbezeichnungenExecute(object obj)
        {
            bool antwort = ViewManager.ShowQuestionWindow("Soll der Eintrag gelöscht werden?", "Ja");
            if (antwort)
            {
                try
                {
                    UserDienstbezeichnung DeleteItem = userDBcontext.UserDienstbezeichnungen.FirstOrDefault(s => s.UserDienstbezeichnungId == SelectedUserDienstbezeichnung.UserDienstbezeichnungId);
                    if (DeleteItem != null)
                    {
                        userDBcontext.UserDienstbezeichnungen.Remove(DeleteItem);
                        userDBcontext.SaveChanges();
                        UserDienstbezeichnungenList.Remove(SelectedUserDienstbezeichnung);
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage.CreateExceptionWithFlyOutMessage("DeleteUserDienstbezeichnungenExecute", ex);
                }
            }

        }

        private void NewUserDienstbezeichnungenExecute(object obj)
        {
            SelectedUserDienstbezeichnung = new UserDienstbezeichnung();
        }

        private void AddUserDienstbezeichnungenExecute(object obj)
        {
            if ( SelectedUserDienstbezeichnung.GültigAb < new DateTime(2000, 1, 1))
            {
                ViewManager.ShowMainInfoFlyout("Das Datum muss größer als der 1. Januar 2020 sein.", false);
                return;
            }
            if (SelectedUserDienstbezeichnung.Dienstbezeichnung  == null)
            {
                ViewManager.ShowMainInfoFlyout("Bitte wählen Sie eine Dienstbezeichnung aus.", false);
                return;
            }
            try
            {
                UserDienstbezeichnung AddUserDienstbezeichnung = new UserDienstbezeichnung { Dienstbezeichnung = SelectedUserDienstbezeichnung.Dienstbezeichnung, GültigAb = SelectedUserDienstbezeichnung.GültigAb, User=CurrenUser};
                userDBcontext.UserDienstbezeichnungen.AddOrUpdate(AddUserDienstbezeichnung);
                userDBcontext.SaveChanges();
                UserDienstbezeichnungenList.Add(AddUserDienstbezeichnung);
            }
            catch (Exception ex)
            {
                ErrorMessage.CreateExceptionWithFlyOutMessage("AddUserDienstbezeichnungenExecute", ex);
            }
        }

        private DbSet<User> userSet;

        private void SaveExecute(object obj)
        {
               
            //DbSet<User> userDB = new DbSet<User>();
            try
            {
                if (SelectedTitel  != null)
                {
                    CurrenUser.TitelId = SelectedTitel.TitelId;
                    CurrenUser.Titel = TitelList.Where(a => a.TitelId == CurrenUser.TitelId).FirstOrDefault();
                }
                if (SelectedGeschlecht != null)
                {
                    CurrenUser.GeschlechtID = SelectedGeschlecht.GeschlechtID;
                    CurrenUser.Geschlecht = GeschlechtList.FirstOrDefault(g => g.GeschlechtID == CurrenUser.GeschlechtID);
                }
                if (SelectedPosition != null)
                {
                    CurrenUser.PositionId= SelectedPosition.PositionId;
                    CurrenUser.Position = PositionList.Where(a => a.PositionId== CurrenUser.PositionId).FirstOrDefault();
                    //if (SelectedPosition.PositionId == 1)
                    //{
                    //    //CurrenUser.DienstbezeichnungId = SelectedDienstbezeichnung.DienstbezeichnungId;
                    //    //Dienstbezeichnung dienstbezeichnung = DienstbezeichnungList.FirstOrDefault(d => d.DienstbezeichnungId == CurrenUser.DienstbezeichnungId);
                    //    //if (dienstbezeichnung != null)
                    //    //    CurrenUser.Dienstbezeichnung = dienstbezeichnung;
                    //}
                }
                if (SelectedStatus != null)
                {
                    CurrenUser.StatusId = SelectedStatus.StatusId;
                    Status status = StatusList.FirstOrDefault(g => g.StatusId == CurrenUser.StatusId);
                    if (status != null)
                        CurrenUser.Status= StatusList.FirstOrDefault(g => g.StatusId == CurrenUser.StatusId);
                }


                userSet = userDBcontext.Set<User>();
                userSet.AddOrUpdate(CurrenUser);
                userDBcontext.SaveChanges();
                ViewManager.ShowMainInfoFlyout("Die Änderungen wurden gespeichert", false);
                if (ViewManager.PageInfo.UserSettingType == 0)
                {
                    UserManager.RegistratedUser = CurrenUser;
                    ViewManager.MainWindowViewModel.LoginUser = CurrenUser.Fullname;
                    ViewManager.ShowPageOnMainView<StartView>();
                }
                else
                {
                    //ViewManager.ShowPageOnMainView<AdminView>();
                }
            }
            catch (Exception ex)
            {
                ViewManager.ShowMainInfoFlyout($"Die Änderungen konnten nicht gespeichert werden. Es ist folgender Fehler aufgetreten: {ex.Message}", false);
            }
        }


        private void QuitExecute(object obj)
        {
            //if (ViewManager.PageInfo.UserSettingType == 0) ViewManager.ShowPageOnMainView<StartView>(); else ViewManager.ShowPageOnMainView<AdminView>();
        }
    }
}

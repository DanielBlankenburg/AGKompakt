using BGH_Kompakt.Classes._LookUp.UserLookUps;
using BGH_Kompakt.Classes.Helper;
using BGH_Kompakt.Classes.UserClasses;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Dtos;
using BGH_Kompakt.Enums;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.Extentions;
using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.Services.UserService;
using BGH_Kompakt.Views.Start;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BGH_Kompakt.ViewModel.Userlogin
{
    public partial class UserLoginViewModel : ViewModelBase
    {
        private string _titel;
        private string _Introduction;
        private ObservableCollection<Titel> _TitelList = new ObservableCollection<Titel>();
        private ObservableCollection<Geschlecht> _GeschlechtList = new ObservableCollection<Geschlecht>();
        private ObservableCollection<Position> _PositionList = new ObservableCollection<Position>();
        private ObservableCollection<Status> _StatusList = new ObservableCollection<Status>();
        private ObservableCollection<Dienstbezeichnung> _DienstbezeichnungList = new ObservableCollection<Dienstbezeichnung>();
        private UserDBContext userDBContext = new UserDBContext();
        public string Titel { get { return _titel; } }
        public string Introduction { get { return _Introduction; } }

        private string _Nachname = string.Empty;
        public string Nachname
        {
            get => _Nachname;
            set 
            { 
                SetProperty<string>(ref _Nachname, value);
                if (EMail == string.Empty) EMail = $"{Nachname.RemoveDiacritics().ToLower()}-{Vorname.RemoveDiacritics().ToLower()}@bgh.bund.de";
            }
        }
        public string Vorname { get; set; }

        private string _Email = string.Empty;
        public string EMail
        {
            get => _Email;
            set { SetProperty<string>(ref _Email, value); }
        }

        public ObservableCollection<Titel> Titellist { get { return _TitelList; } }
        private Titel selectedTitel;
        public Titel SelectedTitel
        {
            get { return selectedTitel; }
            set { selectedTitel = value; }
        }

        public ObservableCollection<Geschlecht> GeschlechtList { get { return _GeschlechtList; } }
        private Geschlecht selectedGeschlecht;
        public Geschlecht SelectedGeschlecht
        {
            get { return selectedGeschlecht; }
            set { selectedGeschlecht = value; }
        }
        public ObservableCollection<Position> PositionList { get { return _PositionList; } }
        private Position selectedPosition;
        public Position SelectedPosition
        {
            get { return selectedPosition; }
            set 
            { 
                selectedPosition = value;
                ShowDienstbezeichnung = selectedPosition.PositionId == 1;
            }
        }

        public ObservableCollection<Status> StatusList { get { return _StatusList; } }
        private Status selectedStatus;
        public Status SelectedStatus
        {
            get { return selectedStatus; }
            set { selectedStatus = value; }
        }

        public ObservableCollection<Dienstbezeichnung> DienstbezeichungList { get { return _DienstbezeichnungList; } }
        private Dienstbezeichnung selectedDienstbezeichnung;
        public Dienstbezeichnung SelectedDienstbezeichnung
        {
            get { return selectedDienstbezeichnung; }
            set { selectedDienstbezeichnung = value; }
        }

        #region Show
        private bool _Show_PersonData = true;
        public bool Show_PersonData
        {
            get { return _Show_PersonData; }
            set
            {
                SetProperty<bool>(ref _Show_PersonData, value);
                if (_Show_PersonData)
                {
                    Show_SenatData = false;
                }
            }
        }

        private bool _Show_SenatData = false;
        public bool Show_SenatData
        {
            get { return _Show_SenatData; }
            set
            {
                SetProperty<bool>(ref _Show_SenatData, value);
                if (_Show_SenatData)
                {
                    Show_PersonData = false;
                }
            }
        }

        private bool _Show_SenatsChoise = true;
        public bool Show_SenatsChoise
        {
            get { return _Show_SenatsChoise; }
            set { SetProperty<bool>(ref _Show_SenatsChoise, value);}
        }

        private bool _ShowDienstbezeichnung = false;
        public bool ShowDienstbezeichnung
        {
            get { return _ShowDienstbezeichnung; }
            set { SetProperty(ref _ShowDienstbezeichnung, value); }
        }

        #endregion

        #region Executes
        public ICommand QuitCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand ExpanderCommand { get; set; }

        #endregion


        public UserLoginViewModel()
        {
            QuitCommand = new RelayCommand(QuitExecute);
            LoginCommand = new RelayCommand(LoginExecute);
            ExpanderCommand = new RelayCommand(ExpanderExecute);

            //UserDBContext userDBcontext = new UserDBContext();
            //Titel füllen
            try
            {
                var Titel_Query = userDBContext.Titel;
                foreach (var item in Titel_Query) _TitelList.Add(item);
                if (_TitelList.Count > 0) SelectedTitel = _TitelList[0];
                //Geschlecht füllen
                var Geschelcht_Query = userDBContext.Geschlechter;
                foreach (var item in Geschelcht_Query) _GeschlechtList.Add(item);
                //Position füllen
                var Position_Query = userDBContext.Positions;
                foreach (var item in Position_Query) _PositionList.Add(item);
                //Status füllen
                //var Status_Query = userDBcontext.Status;
                //foreach (var item in Status_Query) _StatusList.Add(item);
                //enumDienstbezeichnungen füllen
                var Dienstbezeichnung_Query = userDBContext.Dienstbezeichnungen;
                foreach (var item in Dienstbezeichnung_Query) _DienstbezeichnungList.Add(item);
                //Senate füllen

                _titel = "Benutzerregistrierung";
                _Introduction = "Die Funktionen des Programms werden über den Nutzernamen des Benutzers gesteuert." +
                                    Environment.NewLine + "Für Ihren Benutzernamen konnte kein Benutzer gefunden werden." +
                                    Environment.NewLine + Environment.NewLine + "Bitte tragen Sie Ihre Daten ein und registrieren sich.";
            }
            catch (Exception ex)
            {
                ErrorMessage.CreateExceptionWithoutMessage("UserLoginViewModel", ex);
            }

        }


        private void QuitExecute(object obj)
        {
            System.Environment.Exit(0);
        }

        private void LoginExecute(object obj)
        {
            LogIn();
        }

        private void ExpanderExecute(object obj)
        {
            if (obj != null)
            {
                switch ((string)obj)
                {
                    case "VorData":
                        Show_PersonData = false;
                        Show_SenatData = true;
                        break;
                    case "BackData":
                        Show_PersonData = true;
                        Show_SenatData= false;
                        break;
                }
            }
        }


        private void LogIn()
        {
            if (SelectedTitel == null)
            {
                ViewManager.ShowMainInfoFlyout("Bitte wählen Sie einen Eintrag bei der Kategorie Titel aus.", false);
                return;
            }
            if (selectedGeschlecht == null)
            {
                ViewManager.ShowMainInfoFlyout("Bitte wählen Sie ein Geschlecht aus.", false);
                return;
            }
            if (SelectedPosition== null)
            {
                ViewManager.ShowMainInfoFlyout("Bitte wählen Sie eine Diestgruppe aus.", false);
                return;
            }
            if (SelectedDienstbezeichnung== null && selectedPosition.PositionId == 1)
            {
                ViewManager.ShowMainInfoFlyout("Bitte wählen Sie eine Dienstbezeichnung aus.", false);
                return;
            }
            //if (SelectedStatus == null)
            //{
            //    ViewManager.ShowMainInfoFlyout("Bitte wählen Sie einen Status aus.", false);
            //    return;
            //}
            UserDBContext userDBcontext = new UserDBContext();
            Status status = userDBcontext.Status.FirstOrDefault(x => x.StatusText == UserEnums.EnumUserStatus.aktiv.ToString());
            DBResponse resp = UserManager.RegisterUser(Nachname, Vorname, EMail, Environment.UserName, SelectedTitel, SelectedGeschlecht, status ?? null, SelectedPosition, SelectedDienstbezeichnung ?? null, Show_SenatsChoise);
            if (resp.Success)
            {
                
                    //Neubestimmung des RegistratedUser nur  bei Eintragung über die Startmaske
                    User responseUser = resp.Data as User;
                    if (UserManager.LoginUser(responseUser.ComputerName))
                    {
                        ViewManager.MainWindowViewModel.LoginUser = UserManager.RegistratedUser.Fullname;
                        ViewManager.MainWindowViewModel.ShowSitzungsunterlagen = UserManager.RegistratedUser.ShowSitzungsunterlagen;
                        ViewManager.MainWindowViewModel.ShowMontagspost = UserManager.RegistratedUser.ShowMontagspost;
                        ViewManager.MainWindowViewModel.ShowNebentaetigkeiten = UserManager.RegistratedUser.ShowActivityRequests;
                        ViewManager.ShowPageOnMainView<StartView>();
                    }
                    else
                    {
                        ViewManager.ShowMainInfoFlyout($"Der Nutzer konnte nicht geladen werden. Wenden Sie sich bitte an den Administrator.", false);
                    }
            }
            else 
            {
                ViewManager.ShowMainInfoFlyout("Der Nutzer konnte nicht eingetragen werden.", false);
            }              
        }
    }
}

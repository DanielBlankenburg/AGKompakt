using BGH_Kompakt.Classes._LookUp.MP;
using BGH_Kompakt.Classes._LookUp.UserLookUps;
using BGH_Kompakt.Classes.Helper;
using BGH_Kompakt.Classes.MP;
using BGH_Kompakt.Classes.UserClasses;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Dtos;
using BGH_Kompakt.Enums;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.MontagspostService;
using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.Services.UserService;
using BGH_Kompakt.Views.Montagspost;
using BGH_Kompakt.Views.Start;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.WebControls.Expressions;
using System.Windows;
using System.Windows.Input;
using static BGH_Kompakt.Enums.SettingEnums;

namespace BGH_Kompakt.ViewModel
{
    public partial class MontagsPostViewModel : ViewModelBase
    {
        private MPDBContext mPDBContext = new MPDBContext();

        #region Value
        private UserDBContext userDBContext = new UserDBContext();
        private ObservableCollection<MPWeek> _MPWeekList = new ObservableCollection<MPWeek>();
        public ObservableCollection<MPWeek> MPWeekList { get { return _MPWeekList; } }
        private ObservableCollection<MPUserDecision> _MPUserDecisionList = new ObservableCollection<MPUserDecision>();
        public ObservableCollection<MPUserDecision> MPUserDecisionList { get { return _MPUserDecisionList; } }
        private MPWeek _SelectedMPWeek;
        public MPWeek SelectedMPWeek
        {
            get { return _SelectedMPWeek; }
            set
            {
                SetProperty<MPWeek>(ref _SelectedMPWeek, value);
                showSenate = (_SelectedMPWeek != null);
                SetMPWeek();
            }
        }

        private void SetMPWeek()
        {
            if (MontagsPostManager.RecoverStatus)
            {
                int counter = -1;
                foreach (MPWeek week in MPWeekList)
                {
                    counter++;
                    if (week.MPWeekID == SelectedMPWeek.MPWeekID) break;
                }
                SelectedMPWeekIndex = counter;
            }

            SenateFill();
        }

        private int _SelectedMPWeekIndex;
        public int SelectedMPWeekIndex
        {
            get { return _SelectedMPWeekIndex; }
            set { SetProperty(ref _SelectedMPWeekIndex, value); }
        }
        private ObservableCollection<MPSenat> _MPSenate = new ObservableCollection<MPSenat>();
        public ObservableCollection<MPSenat> MPSenateList { get { return _MPSenate; } }
        private bool EnableDissionFill = true;
        private MPSenat _SelectedMPSenat;
        public MPSenat SelectedMPSenat
        {
            get { return _SelectedMPSenat; }
            set
            {
                if (EnableDissionFill)
                {
                    SetProperty<MPSenat>(ref _SelectedMPSenat, value);
                    ShowDetailArea(ShowSelectedDetailsPdf, ShowSelectedDetailsData);
                    if (MontagsPostManager.RecoverStatus)
                    {
                        int counter = -1;
                        foreach (MPSenat senat in MPSenateList)
                        {
                            counter++;
                            if (senat.MPSenatID == SelectedMPSenat.MPSenatID) break;
                        }
                        SelectedMPSenatIndex = counter;
                    }
                    DecisionFill();
                    if (MPDecisionList.Count > 0)
                    {
                        SelectedMPDecision = MPDecisionList[0];
                        SelectedMPDecisionIndex = 0;
                    }
                    else
                    {
                        SelectedMPDecisionIndex = -1;
                    }

                }
            }
        }
        private int _SelectedMPSenatIndex;
        public int SelectedMPSenatIndex
        {
            get { return _SelectedMPSenatIndex; }
            set { SetProperty(ref _SelectedMPSenatIndex, value); }
        }
        private ObservableCollection<MPDecision> _MPDecisions = new ObservableCollection<MPDecision>();
        public ObservableCollection<MPDecision> MPDecisionList { get { return _MPDecisions; } }
        private MPDecision _SelectedMPDecision;
        public MPDecision SelectedMPDecision
        {
            get { return _SelectedMPDecision; }
            set
            {
                SetProperty<MPDecision>(ref _SelectedMPDecision, value);
                //MessageBox.Show("Auswahl der Entscheidung");
                if (SelectedMPDecision != null)
                {
                    if (ShowDetailsPdf)
                    {
                        ShowWebbrowser = true;
                        if (SelectedMPDecision.VermerkAnzeige)
                        {
                            ShowWebbrowserVermerk = true;
                            ShowWebbrowserData = false;
                            Vermerktext = SelectedMPDecision.Vermerk ?? string.Empty;
                        }
                        else if (SelectedMPDecision.PathName != null && SelectedMPDecision.FileName != null)
                        {
                            ShowWebbrowserData = true;
                            ShowWebbrowserVermerk = false;
                            string Path = $"{SelectedMPDecision.PathName}{SelectedMPDecision.FileName}";
                            try
                            {
                                URLAdress = File.Exists(Path) ? new Uri(Path) : new Uri("about:blank");
                            }
                            catch (Exception ex)
                            {
                                ViewManager.ShowMainInfoFlyout($"Die Entscheidung kann nicht angezeigt werden. Es ist folgender Fehler aufgetreten: {ex.Message}", false);
                            }
                            Debug.WriteLine(URLAdress);
                        }
                    }
                    if (MontagsPostManager.RecoverStatus)
                    {
                        int counter = -1;
                        foreach (MPDecision decision in MPDecisionList)
                        {
                            counter++;
                            if (decision.MPDecisionID == SelectedMPDecision.MPDecisionID) break;
                        }
                        SelectedMPDecisionIndex = counter;
                    }
                }
            }
        }
        private int _SelectedMPDecisionIndex;
        public int SelectedMPDecisionIndex
        {
            get { return _SelectedMPDecisionIndex; }
            set { SetProperty(ref _SelectedMPDecisionIndex, value); }
        }
        private string _Vermerktext;
        public string Vermerktext
        {
            get { return _Vermerktext; }
            set { SetProperty(ref _Vermerktext, value); }
        }
        private Uri _URLAdress;
        public Uri URLAdress
        {
            get { return _URLAdress; }
            set { SetProperty(ref _URLAdress, value); }
        }
        private UserFilterMP _FilterMP;
        public UserFilterMP FilterMP
        {
            get { return _FilterMP; }
            set { SetProperty<UserFilterMP>(ref _FilterMP, value); }
        }
        private ObservableCollection<int> _VintageList = new ObservableCollection<int>();
        public ObservableCollection<int> VintageList { get { return _VintageList; } }
        private int _SelectedVintage;
        public int SelectedVintage
        {
            get { return _SelectedVintage; }
            set
            {
                SetProperty<int>(ref _SelectedVintage, value);
                WeekFill(MPSorting);
            }
        }
        //public int VintageIndex { get; set; }
        private bool _Pdf_Umwandlung = false;
        public bool Pdf_Umwandlung
        {
            get { return _Pdf_Umwandlung; }
            set { SetProperty(ref _Pdf_Umwandlung, value); }
        }
        private bool _Pdf_Vergleich = true;
        public bool Pdf_Vergleich
        {
            get { return _Pdf_Vergleich; }
            set { SetProperty(ref _Pdf_Vergleich, value); }
        }
        private bool _EMailNotification;
        public bool EMailNotification
        {
            get { return _EMailNotification; }
            set
            {
                SetProperty<bool>(ref _EMailNotification, value);
                SetEMailNotification(value, false, true);
            }
        }
        bool DataInitial { get; set; } = true;
        private bool _MPSorting;
        public bool MPSorting
        {
            get { return _MPSorting; }
            set
            {
                SetProperty<bool>(ref _MPSorting, value);
                if(!DataInitial) WeekFill(value);
            }
        }
        private bool _MPSortingAsc;
        public bool MPSortingAsc
        {
            get { return _MPSortingAsc; }
            set
            {
                SetProperty<bool>(ref _MPSortingAsc, value);
                MPSorting = value;
                if(value) MPSortingDesc = false;
                if (value && !DataInitial) SaveSortingAsync(value);
            }
        }
        private bool _MPSortingDesc;
        public bool MPSortingDesc
        {
            get { return _MPSortingDesc; }
            set
            {
                SetProperty<bool>(ref _MPSortingDesc, value);
                MPSorting = !value;
                if (value) MPSortingAsc = false;
                if (value && !DataInitial) SaveSortingAsync(false);
            }
        }
        private bool _MPBSCWNoSubFolders = true;
        public bool MPBSCWSubFolders
        {
            get { return _MPBSCWNoSubFolders; }
            set { 
                SetProperty(ref _MPBSCWNoSubFolders, value);
                SetBSCWSubFolder(value, false, true);
            }
        }


        private async void SaveSortingAsync(bool value)
        {
            Task<DBResponse> task = SaveSorting(value);
            await task;
            if (!task.Result.Success) ViewManager.ShowMainInfoFlyout(task.Result.Message, false);

        }
        private Task<DBResponse> SaveSorting(bool value)
        {
            Task<DBResponse> task = Task.Run<DBResponse>(() =>
            {
                DBResponse response = new DBResponse();
                try
                {
                    UserManager.RegistratedUser.FilterMP.AscSorting = value;
                    User currentuser = userDBContext.Users.Include(x => x.FilterMP).FirstOrDefault(x => x.UserId == UserManager.RegistratedUser.UserId);
                    currentuser.FilterMP.AscSorting = value;
                    userDBContext.Users.AddOrUpdate(currentuser);
                    userDBContext.SaveChanges();
                    response.Success = true;
                    return response;
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Message = $"Die Einstellung konnte nicht gespeichert werden. Es ist folgender Fehler aufgetreten: {ex.Message}";
                    return response;
                }
            });
            return task;
        }


        #endregion

        #region ICommands
        public ICommand MPWeekDeleteCommand { get; set; }
        public ICommand MPWeekEditCommand { get; set; }
        public ICommand ImportMPDecisionCommand { get; set; }
        public ICommand OwnerStageCommand { get; set; }
        public ICommand OpenFileCommand { get; set; }
        public ICommand GesamtDateiCommand { get; set; }
        public ICommand BECommand { get; set; }
        public ICommand VermerkCommand { get; set; }
        public ICommand WordReadCommand { get; set; }
        public ICommand AddKWCommand { get; set; }
        public ICommand TestCommand { get; set; }
        public ICommand BSCWServerCommand { get; set; }
        public ICommand DetailDoubleClickCommand { get; set; }
        #endregion

        #region Visibility
        private bool _showWebbrowser = false;
        public bool ShowWebbrowser
        {
            get { return _showWebbrowser; }
            set { SetProperty<bool>(ref _showWebbrowser, value); }
        }

        private bool _showZivilsenate = false;
        public bool ShowZivilsenate
        {
            get { return _showZivilsenate; }
            set { SetProperty<bool>(ref _showZivilsenate, value); }
        }
        private bool _showSelectedZivilsenate = false;
        public bool ShowSelectedZivilsenate
        {
            get { return _showSelectedZivilsenate; }
            set
            {
                SetProperty<bool>(ref _showSelectedZivilsenate, value);
                if (_showSelectedZivilsenate) ShowSenate(2);
            }
        }

        private bool _showStrafsenate = false;
        public bool ShowStrafsenate
        {
            get { return _showStrafsenate; }
            set { SetProperty<bool>(ref _showStrafsenate, value); }
        }

        private bool _showSelectedStrafsenate = false;
        public bool ShowSelectedStrafsenate
        {
            get { return _showSelectedStrafsenate; }
            set
            {
                SetProperty<bool>(ref _showSelectedStrafsenate, value);
                if (_showSelectedStrafsenate) ShowSenate(3);
            }
        }

        private bool _showSondersenate = false;
        public bool ShowSondersenate
        {
            get { return _showSondersenate; }
            set { SetProperty<bool>(ref _showSondersenate, value); }
        }

        private bool _showSelectedSondersenate = false;
        public bool ShowSelectedSondersenate
        {
            get { return _showSelectedSondersenate; }
            set
            {
                SetProperty<bool>(ref _showSelectedSondersenate, value);
                if (_showSelectedSondersenate) ShowSenate(4);
            }
        }

        private bool _showSelectedAlleSenate = false;
        public bool ShowSelectedAlleSenate
        {
            get { return _showSelectedAlleSenate; }
            set
            {
                SetProperty<bool>(ref _showSelectedAlleSenate, value);
                if (_showSelectedAlleSenate) ShowSenate(0);
            }
        }

        private bool _showDetailsPdf = false;
        public bool ShowDetailsPdf
        {
            get { return _showDetailsPdf; }
            set
            {
                SetProperty<bool>(ref _showDetailsPdf, value);
                if (value) ShowDetailsData = false;
            }
        }

        private bool _showSelectedDetailsPdf = true;
        public bool ShowSelectedDetailsPdf
        {
            get { return _showSelectedDetailsPdf; }
            set
            {
                SetProperty<bool>(ref _showSelectedDetailsPdf, value);
                if (value) ShowDetailArea(showDetailsPdf: value);
                //ShowDetailsData = !value;
                //ShowDetailsPdf = value;
                //ShowWebbrowser = value;
            }
        }

        private bool _showDetailsData = false;
        public bool ShowDetailsData
        {
            get { return _showDetailsData; }
            set { SetProperty<bool>(ref _showDetailsData, value); }
        }

        private bool _showSelectedDetailsData = false;
        public bool ShowSelectedDetailsData
        {
            get { return _showSelectedDetailsData; }
            set
            {
                SetProperty<bool>(ref _showSelectedDetailsData, value);
                if (value) ShowDetailArea(showDetailsData: value);
                //ShowDetailsPdf = !value;
                //ShowDetailsData = value;
                //ShowWebbrowser = !value;
            }
        }

        //private bool _showDecisionsAll = true;
        //public bool ShowDecisionsAll
        //{
        //    get { return _showDecisionsAll; }
        //    set { SetProperty<bool>(ref _showDecisionsAll, value);}
        //}

        private bool _showSelectedDecisionsAll = true;
        public bool ShowSelectedDecisionsAll
        {
            get { return _showSelectedDecisionsAll; }
            set
            {
                SetProperty<bool>(ref _showSelectedDecisionsAll, value);
                if (value)
                {
                    ShowSelectedDecisionsUser = false;
                    WeekFill(MPSorting);
                }
            }
        }

        //private bool _showDecisionsUser = false;
        //public bool ShowDecisionsUser
        //{
        //    get { return _showDecisionsUser; }
        //    set { SetProperty<bool>(ref _showDecisionsUser, value);}
        //}

        private bool _showSelectedDecisionsUser = false;
        public bool ShowSelectedDecisionsUser
        {
            get { return _showSelectedDecisionsUser; }
            set
            {
                SetProperty<bool>(ref _showSelectedDecisionsUser, value);
                if (value)
                {
                    ShowSelectedDecisionsAll = false;
                    OwnerStorageFill();
                    WeekFill(MPSorting);
                }
            }
        }

        private bool _showAdmin = false;
        public bool ShowAdmin
        {
            get { return _showAdmin; }
            set { SetProperty<bool>(ref _showAdmin, value); }
        }

        private bool _ShowBE = false;
        public bool ShowBE
        {
            get { return _ShowBE; }
            set { SetProperty<bool>(ref _ShowBE, value); }
        }

        private bool _showSenate = false;
        public bool showSenate
        {
            get { return _showSenate; }
            set { SetProperty<bool>(ref _showSenate, value); }
        }

        //private bool _showDetails = false;
        //public bool showDetails
        //{
        //    get { return _showDetails; }
        //    set { SetProperty<bool>(ref _showDetails, value); }
        //}

        private void ShowDetailArea(bool showDetailsPdf = false, bool showDetailsData = false)
        {
            if (SelectedMPSenat != null)
            {
                ShowDetailsPdf = showDetailsPdf;
                ShowDetailsData = showDetailsData;
                ShowWebbrowser = (showDetailsPdf && SelectedMPDecision != null);
            }
        }

        private bool _showWebbrowserData = true;
        public bool ShowWebbrowserData
        {
            get { return _showWebbrowserData; }
            set { SetProperty<bool>(ref _showWebbrowserData, value); }
        }

        private bool _showWebbrowserVermerk = false;
        public bool ShowWebbrowserVermerk
        {
            get { return _showWebbrowserVermerk; }
            set { SetProperty<bool>(ref _showWebbrowserVermerk, value); }
        }

        private bool _SelectedOutlineAll = false;
        public bool SelectedOutlineAll
        {
            get { return _SelectedOutlineAll; }
            set { 
                SetProperty<bool>(ref _SelectedOutlineAll, value); 
                if (value) showSenate = false;
            }
        }

        private bool _SelectedOutlineSenate = true;
        public bool SelectedOutlineSenate
        {
            get { return _SelectedOutlineSenate; }
            set { 
                SetProperty<bool>(ref _SelectedOutlineSenate, value); 
                if (value) showSenate = true;
            }
        }

        #endregion

        //Constructor
        public MontagsPostViewModel()
        {
            MPWeekDeleteCommand = new RelayCommand(MPWeekDeleteExecute);
            OwnerStageCommand = new RelayCommand(OwnerStageExecute);
            OpenFileCommand = new RelayCommand(OpenFileExecute);
            GesamtDateiCommand = new RelayCommand(GesamtDateiExecute);
            BECommand = new RelayCommand(BEExecute);
            VermerkCommand = new RelayCommand(VermerkExecute);
            WordReadCommand = new RelayCommand(WordReadExecute);
            AddKWCommand = new RelayCommand(AddKWExecute);
            BSCWServerCommand = new RelayCommand(BSCWExecute);
            TestCommand = new RelayCommand(TestExecute);
            DetailDoubleClickCommand = new RelayCommand(DetailDoubleClickExecute);
            MPWeekEditCommand = new RelayCommand(MPWerkEditExecute);

            //ReadMPState = MPStateText;
            MPDBContext mPDBContext = new MPDBContext();

            try
            {
                SetSorting();
                var MPVintages_Query = mPDBContext.MPWeeks.Select(x => x.MPWeekYear).Distinct();
                foreach (var Vintage in MPVintages_Query) _VintageList.Add(Vintage);
                if (VintageList.Count > 0) SelectedVintage = VintageList.LastOrDefault();

                SetFilter();
                ShowFilter();

                if (UserManager.RegistratedUser.MPBSCW_Server_Drive != null) SelectedDrive = UserManager.RegistratedUser.MPBSCW_Server_Drive.Value;

                SetAdminStatus();
                SetMontagespost();
                SetEMailNotification(UserManager.RegistratedUser.MPEMailNotification, true, false);
                SetBSCWSubFolder(UserManager.RegistratedUser.MPBSCWSubFolders, true, false);

            }
            catch (Exception ex)
            {
                ViewManager.ShowMainInfoFlyout($"Beim Öffnen der Montagspost ist folgender Fehler aufgetreten: {ex.Message}", false);
                ViewManager.ShowPageOnMainView<StartView>();
                return;
            }
            DataInitial = false;
        }

        private void SetSorting()
        {
            try
            {
                MPSorting = UserManager.RegistratedUser.FilterMP.AscSorting;
                if (MPSorting) MPSortingAsc = true; else MPSortingDesc = true;
            }
            catch (Exception ex)
            {
                ViewManager.ShowMainInfoFlyout($"Es ist folgender Fehler aufgetreten: {ex.Message}", false);
            }
        }

        private void SetEMailNotification(bool value, bool setValue, bool saveValue)
        {
            if (setValue) EMailNotification = value;
            if (saveValue)
            {
                try
                {
                    UserManager.RegistratedUser.MPEMailNotification = EMailNotification;
                    User SaveUser = userDBContext.Users.FirstOrDefault(u => u.UserId == UserManager.RegistratedUser.UserId);
                    if (SaveUser != null)
                    {
                        SaveUser.MPEMailNotification = EMailNotification;
                        userDBContext.Users.AddOrUpdate(SaveUser);
                        userDBContext.SaveChanges();
                    }
                }
                catch (Exception)
                {
                    ViewManager.ShowMainInfoFlyout("Die Änderungen konnten nicht gespeichert werden.", false);
                }
            }
        }
        private void SetBSCWSubFolder(bool value, bool setValue, bool saveValue)
        {
            if (setValue) MPBSCWSubFolders = value;
            if (saveValue)
            {
                try
                {
                    UserManager.RegistratedUser.MPBSCWSubFolders = MPBSCWSubFolders;
                    User SaveUser = userDBContext.Users.FirstOrDefault(u => u.UserId == UserManager.RegistratedUser.UserId);
                    if (SaveUser != null)
                    {
                        SaveUser.MPBSCWSubFolders = MPBSCWSubFolders;
                        userDBContext.Users.AddOrUpdate(SaveUser);
                        userDBContext.SaveChanges();
                    }
                }
                catch (Exception)
                {
                    ViewManager.ShowMainInfoFlyout("Die Änderungen konnten nicht gespeichert werden.", false);
                }
            }
        }



        private void SetMontagespost()
        {
            if (MontagsPostManager.RecoverStatus && (MontagsPostManager.SavedWeekStatus || MontagsPostManager.SavedSenatStatus || MontagsPostManager.SavedDecisionStatus))
            {
                SelectedVintage = MontagsPostManager.SavedVintage;
                SelectedMPWeek = MontagsPostManager.SavedWeek;
                if (MontagsPostManager.SavedSenatStatus || MontagsPostManager.SavedDecisionStatus) SelectedMPSenat = MontagsPostManager.SavedSenat;
                if (MontagsPostManager.SavedDecisionStatus) SelectedMPDecision = MontagsPostManager.SavedDecision;
                MontagsPostManager.RecoverStatus = false;
            }
        }
        private void SetAdminStatus()
        {
            if (UserManager.RegistratedUser.AdminStatus != null)
            {
                foreach (AdminStatus Status in UserManager.RegistratedUser.AdminStatus) if (Status.AdminStatusText == UserEnums.EnumAdminStatus.MontagspostAdmin.ToString()) ShowAdmin = true;
                foreach (AdminStatus Status in UserManager.RegistratedUser.AdminStatus) if (Status.AdminStatusText == UserEnums.EnumAdminStatus.Präsidentin.ToString()) ShowBE = true;
            }
        }
        private void SetFilter()
        {
            UserFilterMP FilterMP = userDBContext.FilterMP.Where(x => x.User.UserId == UserManager.RegistratedUser.UserId).FirstOrDefault();
            if (FilterMP == null)
            {
                UserFilterMP newFilter = new UserFilterMP { User = userDBContext.Users.Where(x => x.UserId == UserManager.RegistratedUser.UserId).FirstOrDefault() };
                userDBContext.FilterMP.Add(newFilter);
                userDBContext.SaveChanges();
                FilterMP = newFilter;
            }
            _FilterMP = FilterMP;
        }
        private void ShowFilter()
        {
            if (FilterMP.SonderGesamt)
            {
                ShowSondersenate = true;
                ShowSelectedSondersenate = true;
            }
            if (FilterMP.StrafGesamt)
            {
                ShowStrafsenate = true;
                ShowSelectedStrafsenate = true;
                ShowSelectedSondersenate = false;
            }
            if (FilterMP.ZivilGesamt)
            {
                ShowZivilsenate = true;
                ShowSelectedZivilsenate = true;
                ShowSelectedStrafsenate = false;
                ShowSelectedSondersenate = false;
            }
        }

        #region Executes

        private void MPWerkEditExecute(object obj)
        {
            MessageBox.Show("Diese Funktion wurde noch nicht integriert.");
        }

        private void BSCWExecute(object obj)
        {
            BSCW_Check();
            //Test();
        }

        private void MPWeekDeleteExecute(object obj)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Soll die Kalenderwoche gelöscht werden?", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                MPWeek DeleteWeek = mPDBContext.MPWeeks.Where(x => x.MPWeekID == SelectedMPWeek.MPWeekID).FirstOrDefault();
                if (DeleteWeek != null)
                {
                    try
                    {
                        mPDBContext.MPWeeks.Remove(DeleteWeek);
                        mPDBContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        ViewManager.ShowMainInfoFlyout($"Die Kalenderwoche konnte nicht gelöscht werden. Es ist folgender Fehler aufgetreten: {ex.Message}", false);
                        return;
                    }
                    _MPWeekList.Remove(SelectedMPWeek);
                    try
                    {
                        string deleteMPWeek = (DeleteWeek.MPWeekNumber <= 9) ? $"0{DeleteWeek.MPWeekNumber}" : DeleteWeek.MPWeekNumber.ToString();
                        string path = $"{BGHKompaktSystemInfo.PathDokstelleDFS}{BGHKompaktSystemInfo.PathMontagspost}{DeleteWeek.MPWeekYear}\\KW{deleteMPWeek}\\";
                        Directory.Delete(path, true);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show($"Der Dateipfad konnte nicht gelöscht werden. Es ist folgende Fehler aufgetreten: {ex.Message}", "Löschen", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Es ist beim Löschen ein Fehler aufgetreten. Die Woche wurde nicht auf der Datenbank gefunden.", "Löschen", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void OwnerStageExecute(object obj)
        {
            if (obj != null)
            {
                MPDecision decision = mPDBContext.MPDecisions.Where(x => x.MPDecisionID == SelectedMPDecision.MPDecisionID).FirstOrDefault();
                if (decision != null)
                {
                    switch ((string)obj)
                    {
                        case "Add":
                            OwnerStageAddDecision(decision);
                            break;
                        case "Remove":
                            OwnerStageRemoveDecision(decision);
                            break;
                    }
                }
            }
        }

        private void OwnerStageRemoveDecision(MPDecision decision)
        {
            MPUserDecision RemoveDecision = mPDBContext.MPUserDecisions.Where(x => x.User == UserManager.RegistratedUser.UserId && x.Decision.MPDecisionID == decision.MPDecisionID).FirstOrDefault();
            if (RemoveDecision != null)
            {
                mPDBContext.MPUserDecisions.Remove(RemoveDecision);
                mPDBContext.SaveChanges();
                SelectedMPSenat.MPDecisions.Remove(SelectedMPDecision);
                SelectedMPWeek.MPDecisions.Remove(SelectedMPDecision);
                MPDecisionList.Remove(SelectedMPDecision);
            }
            else
            {
                ViewManager.ShowMainInfoFlyout("Die Entscheidung konnte nicht entfernt werden.", false);
            }
        }

        private void OwnerStageAddDecision(MPDecision decision)
        {
            MPUserDecision mPUserDecision = new MPUserDecision
            {
                User = UserManager.RegistratedUser.UserId,
                Decision = decision
            };
            if (mPDBContext.MPUserDecisions.Where(x => x.User == mPUserDecision.User && x.Decision.MPDecisionID == decision.MPDecisionID).FirstOrDefault() == null)
            {
                mPDBContext.MPUserDecisions.Add(mPUserDecision);
                mPDBContext.SaveChanges();
                ViewManager.ShowMainInfoFlyout("Die Entscheidung wurde in die eigene Mappe gelegt.", false);
            }
            else
            {
                ViewManager.ShowMainInfoFlyout("Die Entscheidung liegt bereits in der eigenen Mappe.", false);
            }
        }

        private void OpenFileExecute(object obj)
        {
            if (SelectedMPDecision != null)
            {
                Process.Start(new ProcessStartInfo(SelectedMPDecision.PathName + SelectedMPDecision.FileName)
                {
                    UseShellExecute = true
                });
            }
        }
        private void BEExecute(object obj)
        {
            MontagsPostManager.SaveWeek(SelectedVintage, SelectedMPWeek);
            ViewManager.ShowPageOnMainView<MontagsPostBE>();
        }
        private void VermerkExecute(object obj)
        {
            MontagsPostManager.SaveDecision(SelectedVintage, SelectedMPWeek, SelectedMPSenat, SelectedMPDecision);
            ViewManager.ShowPageOnMainView<MontagsPostVermerkView>();
        }
        private void WordReadExecute(object obj)
        {
            //Word_Datei_Auslesen();
        }
        private void GesamtDateiExecute(object obj)
        {
            //foreach (Kalenderwoche selectedItem in (IEnumerable)dg_KW.SelectedItems)
            //{
            //    strKalenderwoche = selectedItem.Rohdaten;
            //}
            String Bereich = string.Empty;
            if (ShowSelectedZivilsenate)
            {
                Bereich = "Zivilsenate";
            }
            if (ShowSelectedStrafsenate)
            {
                Bereich = "Strafsenate";
            }
            if (ShowSelectedSondersenate)
            {
                Bereich = "Sondersenate";
            }

            string KW = string.Empty;
            if (SelectedMPWeek.MPWeekNumber < 9)
            {
                KW = "0" + SelectedMPWeek.MPWeekNumber;
            }
            else
            {
                KW = SelectedMPWeek.MPWeekNumber.ToString();
            }


            try
            {
                Process.Start(new ProcessStartInfo($"{BGHKompaktSystemInfo.PathDokstelleDFS}{BGHKompaktSystemInfo.PathMontagspost}{SelectedMPWeek.MPWeekYear}\\KW{KW}\\{Bereich}\\{Bereich}.pdf")
                {
                    UseShellExecute = true
                });
            }
            catch (System.Exception ex)
            {

                MessageBox.Show("Die Liste konnte nicht geöffnet werden. Es ist folgender Fehler aufgetreten: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddKWExecute(object obj) => ViewManager.ShowPageOnMainView<MontagspostImportView>();

        private void DetailDoubleClickExecute(object obj)
        {
            if (obj != null)
            {
                switch ((string)obj)
                {
                    case "PDF":
                        ShowSelectedDetailsData = true;
                        break;
                    case "List":
                        ShowSelectedDetailsPdf = true;
                        break;
                }
            }
        }


        private void TestExecute(object obj)
        {
            //ObservableCollection<MPDecision> MPTestList = new ObservableCollection<MPDecision>();
            //MPDBContext mPDBContext = new MPDBContext();
            //var Test_Query = mPDBContext.MPUserDecisions.Include(x => x.Decision).Include(x => x.Decision.MPWeek).Where(x => x.User == UserManager.RegistratedUser.UserId);
            //foreach (var item in Test_Query) MPTestList.Add(item.Decision);
            //foreach (var item in MPTestList) MessageBox.Show(item.MPWeek.MPWeekName);
            //ObservableCollection<MPWeek> MPTestList = new ObservableCollection<MPWeek>();
            //MPDBContext mPDBContext = new MPDBContext();
            //var Test_Query = mPDBContext.MPUserDecisions.Include(x => x.Decision).Include(x => x.Decision.MPWeek).Where(x => x.User == UserManager.RegistratedUser.UserId);
            //var Test_MPWeek = Test_Query.Select(x => x.Decision.MPWeek).Distinct();
            //foreach (var item in Test_MPWeek) MPTestList.Add(item);
            //foreach (var item in MPTestList) MessageBox.Show(item.MPWeekName);
            //MessageBox.Show(SelectedMPDecision.Aktenzeichen);
        }

        #endregion

        private void OwnerStorageFill()
        {
            MPUserDecisionList.Clear();
            MPDBContext mPDBContext = new MPDBContext();
            var Query = mPDBContext.MPUserDecisions.Include(x => x.Decision).Include(x => x.Decision.MPWeek).Where(x => x.User == UserManager.RegistratedUser.UserId).ToList();
            foreach (var item in Query) MPUserDecisionList.Add(item);
        }
        private void WeekFill(bool SortAsc)
        {
            MPWeekList.Clear();
            if (ShowSelectedDecisionsAll)
            {
                if (SelectedVintage > 0)
                {
                    MPDBContext mPDBContext = new MPDBContext();
                    var MPWeek_Query = mPDBContext.MPWeeks.Include(x => x.MPDecisions).Where(x => x.MPWeekYear == SelectedVintage);
                    if (SortAsc) MPWeek_Query = MPWeek_Query.OrderBy(x => x.MPWeekNumber); else MPWeek_Query = MPWeek_Query.OrderByDescending(x => x.MPWeekNumber);
                    foreach (var item in MPWeek_Query) MPWeekList.Add(item);
                }
                return;
            }
            else
            {
                if (SelectedVintage > 0)
                {
                    //var MPWeek_Query = mPDBContext.MPWeeks.Include(x => x.MPDecisions).Where(x => x.MPWeekYear == SelectedVintage).OrderBy(x => x.MPWeekNumber);
                    var MPWeek_Query = MPUserDecisionList.Select(x => x.Decision.MPWeek).Distinct();
                    if (SortAsc) MPWeek_Query = MPWeek_Query.OrderBy(x => x.MPWeekNumber); else MPWeek_Query = MPWeek_Query.OrderByDescending(x => x.MPWeekNumber);
                    foreach (var item in MPWeek_Query) _MPWeekList.Add(item);
                }
                return;
            }
        }
        private void SenateFill()
        {
            MPSenateList.Clear();
            if (SelectedMPWeek != null)
            {
                if (SelectedMPWeek.MPDecisions.Count > 0)
                {
                    try
                    {
                        List<MPSenat> TempSenatList = new List<MPSenat>();
                        //IList<MPDecision> mPDecisions = new List<MPDecision>();
                        var mPDecisions = SelectedMPWeek.MPDecisions;
                        MPDBContext mPDBContext = new MPDBContext();
                        int Bereich = 0;
                        if (ShowSelectedZivilsenate) { Bereich = 2; }
                        else if (ShowSelectedStrafsenate) { Bereich = 3; }
                        else { Bereich = 4; };

                        foreach (MPDecision dec in mPDecisions)
                        {

                            int senatsID = dec.SenatID;
                            if (AnzeigeSenat(senatsID))
                            {
                                MPSenat Senat = TempSenatList.Where(x => x.MPSenatID == senatsID).FirstOrDefault();
                                if (Senat != null)
                                {
                                    MPDecision newDecision = dec;
                                    Senat.MPDecisions.Add(newDecision);
                                }
                                else
                                {
                                    var Decision = mPDBContext.MPDecisions.Include(x => x.Senat).Include(x => x.BE).Where(x => x.MPDecisionID == dec.MPDecisionID).FirstOrDefault();
                                    bool anzeige = ShowSelectedAlleSenate || Decision.Senat.MPCategorieID == Bereich;
                                    if (anzeige)
                                    {
                                        MPSenat SenatNeu = Decision.Senat;
                                        TempSenatList.Add(SenatNeu);
                                    }
                                }
                            }
                        }
                        TempSenatList = TempSenatList.OrderBy(x => x.MPCategorieID).ThenBy(x => x.MPSenatSorting).ToList();
                        EnableDissionFill = false;
                        foreach (var item in TempSenatList)
                        {
                            MPSenateList.Add(item);
                        }
                        SelectedMPSenatIndex = -1;
                        EnableDissionFill = true;
                    }
                    catch (Exception ex)
                    {
                        ViewManager.ShowMainInfoFlyout($"Es ist folgender Fehler aufgetreten: {ex.Message}", false);
                    }
                }

            }
            ShowWebbrowser = false;
        }
        private bool AnzeigeSenat(int senatsID)
        {
            if (senatsID > 1 && senatsID < 16) //Zivilsenate
            {
                if (!FilterMP.ZivilGesamt) return false;
                switch (senatsID)
                {
                    case 2:
                        return FilterMP.ZivilSenat1;
                    case 3:
                        return FilterMP.ZivilSenat2;
                    case 4:
                        return FilterMP.ZivilSenat3;
                    case 5:
                        return FilterMP.ZivilSenat4;
                    case 6:
                        return FilterMP.ZivilSenat5;
                    case 7:
                        return FilterMP.ZivilSenat6;
                    case 8:
                        return FilterMP.ZivilSenat6a;
                    case 9:
                        return FilterMP.ZivilSenat7;
                    case 10:
                        return FilterMP.ZivilSenat8;
                    case 11:
                        return FilterMP.ZivilSenat9;
                    case 12:
                        return FilterMP.ZivilSenat10;
                    case 13:
                        return FilterMP.ZivilSenat11;
                    case 14:
                        return FilterMP.ZivilSenat12;
                    case 15:
                        return FilterMP.ZivilSenat13;
                }
            }
            else if (senatsID >= 16 && senatsID < 22) //Strafsenate
            {
                if (!FilterMP.StrafGesamt) return false;
                switch (senatsID)
                {
                    case 16:
                        return FilterMP.StrafSenat1;
                    case 17:
                        return FilterMP.StrafSenat2;
                    case 18:
                        return FilterMP.StrafSenat3;
                    case 19:
                        return FilterMP.StrafSenat4;
                    case 20:
                        return FilterMP.StrafSenat5;
                    case 21:
                        return FilterMP.StrafSenat6;
                }

            }
            else if (senatsID >= 22)// Sondersenate
            {
                if (!FilterMP.SonderGesamt) return false;
                switch (senatsID)
                {
                    case 22:
                        return FilterMP.Anwaltssenat;
                    case 23:
                        return FilterMP.Notarsenat;
                    case 24:
                        return FilterMP.Landwirtschaftssenat;
                    case 25:
                        return FilterMP.Patentanwaltssenat;
                    case 26:
                        return FilterMP.Steuerberater;
                    case 27:
                        return FilterMP.GmSOG;
                    case 28:
                        return FilterMP.GZS;
                    case 29:
                        return FilterMP.GStS;
                    case 30:
                        return FilterMP.Dienstgericht;
                    case 31:
                        return FilterMP.Kartellsenat;
                }
            }

            return true;
        }
        private void DecisionFill()
        {
            MPDecisionList.Clear();
            if (SelectedMPSenat != null)
            {
                if (SelectedMPSenat.MPDecisions.Count > 0)
                {
                    try
                    {
                        var mPDecisions = SelectedMPSenat.MPDecisions;
                        foreach (MPDecision dec in mPDecisions)
                        {
                            switch (dec.Typ)
                            {
                                case 1:
                                    if (FilterMP.Urteile)
                                    {
                                        if (dec.Leitsatz == string.Empty) {if (AnzeigeSenatLS(dec.SenatID)) MPDecisionList.Add(dec);}
                                        else MPDecisionList.Add(dec);
                                    }
                                    break;
                                case 2:
                                    if (FilterMP.Beschluesse)
                                    {
                                        if (dec.Leitsatz == string.Empty) { if (AnzeigeSenatLS(dec.SenatID)) MPDecisionList.Add(dec); }
                                        else MPDecisionList.Add(dec);
                                    }
                                    break;
                                case 0:
                                    MPDecisionList.Add(dec);
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewManager.ShowMainInfoFlyout($"Es ist folgender Fehler ausgetreten: {ex.Message}", false);
                    }
                }
            }
            ShowWebbrowser = false;
        }

        private bool AnzeigeSenatLS(int senatsID)
        {
            if (senatsID > 1 && senatsID < 16) //Zivilsenate
            {
                if (FilterMP.ZivilGesamtLS) return false;
                switch (senatsID)
                {
                    case 2:
                        return !FilterMP.ZivilSenat1LS;
                    case 3:
                        return !FilterMP.ZivilSenat2LS;
                    case 4:
                        return !FilterMP.ZivilSenat3LS;
                    case 5:
                        return !FilterMP.ZivilSenat4LS;
                    case 6:
                        return !FilterMP.ZivilSenat5LS;
                    case 7:
                        return !FilterMP.ZivilSenat6LS;
                    case 8:
                        return !FilterMP.ZivilSenat6aLS;
                    case 9:
                        return !FilterMP.ZivilSenat7LS;
                    case 10:
                        return !FilterMP.ZivilSenat8LS;
                    case 11:
                        return !FilterMP.ZivilSenat9LS;
                    case 12:
                        return !FilterMP.ZivilSenat10LS;
                    case 13:
                        return !FilterMP.ZivilSenat11LS;
                    case 14:
                        return !FilterMP.ZivilSenat12LS;
                    case 15:
                        return !FilterMP.ZivilSenat13LS;
                }
            }
            else if (senatsID >= 16 && senatsID < 22) //Strafsenate
            {
                if (FilterMP.StrafGesamtLS) return false;
                switch (senatsID)
                {
                    case 16:
                        return !FilterMP.StrafSenat1LS;
                    case 17:
                        return !FilterMP.StrafSenat2LS;
                    case 18:
                        return !FilterMP.StrafSenat3LS;
                    case 19:
                        return !FilterMP.StrafSenat4LS;
                    case 20:
                        return !FilterMP.StrafSenat5LS;
                    case 21:
                        return !FilterMP.StrafSenat6LS;
                }

            }
            else if (senatsID >= 22)// Sondersenate
            {
                if (FilterMP.SonderGesamtLS) return false;
                switch (senatsID)
                {
                    case 22:
                        return !FilterMP.AnwaltssenatLS;
                    case 23:
                        return !FilterMP.NotarsenatLS;
                    case 24:
                        return !FilterMP.LandwirtschaftssenatLS;
                    case 25:
                        return !FilterMP.PatentanwaltssenatLS;
                    case 26:
                        return !FilterMP.SteuerberaterLS;
                    case 27:
                        return !FilterMP.GmSOGLS;
                    case 28:
                        return !FilterMP.GZSLS;
                    case 29:
                        return !FilterMP.GStSLS;
                    case 30:
                        return !FilterMP.DienstgerichtLS;
                    case 31:
                        return !FilterMP.KartellsenatLS;
                    case 32:
                        return !FilterMP.VGSLS;
                    case 33:
                        return !FilterMP.SteuerberaterLS;
                }
            }
            return true;
        }




        public string ScrubHtml(string value)
        {
            var step1 = Regex.Replace(value, @"<[^>]+>|&nbsp;", "").Trim();
            var step2 = Regex.Replace(step1, @"\s{2,}", " ");
            return step2;

        }
        public static string AZVergleich(MPDecisionFile File)
        {
            string Senat;
            string RegZeichen;
            string laufendeNummer;
            string Jahr;
            string Anzeige;

            string name_voll = File.FileName;
            int DateiEndungAnzahl = 4;
            //Senat und RegZeichen bestimmen
            name_voll = name_voll.Substring(0, name_voll.Length - DateiEndungAnzahl);
            Senat = name_voll.Substring(0, name_voll.IndexOf("_"));
            if (Senat == "AnwZ(Brfg)") Senat = "AnzW (Brfg)";
            string NameRest;
            switch (File.Bereich)
            {
                case 1:
                    NameRest = name_voll.Substring(name_voll.IndexOf("_") + 1);
                    RegZeichen = NameRest.Substring(0, NameRest.IndexOf("_"));
                    NameRest = NameRest.Substring(NameRest.IndexOf("_") + 1);
                    NameRest = NameRest.Replace("_", "");
                    laufendeNummer = NameRest.Substring(0, NameRest.IndexOf("-"));
                    Jahr = NameRest.Substring(NameRest.IndexOf("-") + 1, 2);
                    Anzeige = Senat + " " + RegZeichen + " " + laufendeNummer + "/" + Jahr;
                    break;
                case 2:
                    NameRest = name_voll.Substring(name_voll.IndexOf("_") + 1);
                    RegZeichen = NameRest.Substring(0, NameRest.IndexOf("_"));
                    NameRest = NameRest.Substring(NameRest.IndexOf("_") + 1);
                    NameRest = NameRest.Replace("_", "");
                    laufendeNummer = NameRest.Substring(0, NameRest.IndexOf("-"));
                    Jahr = NameRest.Substring(NameRest.IndexOf("-") + 1, 2);
                    Anzeige = Senat + " " + RegZeichen + " " + laufendeNummer + "/" + Jahr;
                    break;
                case 3:
                    NameRest = name_voll.Substring(name_voll.IndexOf("_") + 1);
                    NameRest = NameRest.Substring(NameRest.IndexOf("_") + 1);
                    NameRest = NameRest.Replace("_", "");
                    laufendeNummer = NameRest.Substring(0, NameRest.IndexOf("-"));
                    Jahr = NameRest.Substring(NameRest.IndexOf("-") + 1, 2);
                    Anzeige = Senat + " " + laufendeNummer + "/" + Jahr;
                    break;
                default:
                    Anzeige = "";
                    break;

            }
            return Anzeige;



        }
        public void ShowSenate(int intBereich)
        {
            switch (intBereich)
            {
                case 2: //Zivil
                    ShowSelectedStrafsenate = false;
                    ShowSelectedSondersenate = false;
                    ShowSelectedAlleSenate = false;
                    break;
                case 3: //Straf
                    ShowSelectedZivilsenate = false;
                    ShowSelectedSondersenate = false;
                    ShowSelectedAlleSenate = false;
                    break;
                case 4: //Sondersenat
                    ShowSelectedStrafsenate = false;
                    ShowSelectedZivilsenate = false;
                    ShowSelectedAlleSenate = false;
                    break;
            }
            SenateFill();
        }

        #region BSCW-Server
        public IEnumerable<Drives> DriveList
        {
            get { return Enum.GetValues(typeof(Drives)).Cast<Drives>(); }
        }
        private Drives _SelectedDrive;
        public Drives SelectedDrive
        {
            get { return _SelectedDrive; }
            set
            {
                SetProperty(ref _SelectedDrive, value);
                try
                {
                    UserManager.RegistratedUser.MPBSCW_Server_Drive = value;
                    User EditUser = userDBContext.Users.FirstOrDefault(u => u.UserId == UserManager.RegistratedUser.UserId);
                    if (EditUser != null) EditUser.MPBSCW_Server_Drive = value;
                    userDBContext.Users.AddOrUpdate(EditUser);
                    userDBContext.SaveChanges();
                }
                catch (Exception)
                {
                    ViewManager.ShowMainInfoFlyout("Das Laufwerk konnte nicht gespeichert werden.", false);
                    throw;
                }
            }
        }
        private async void BSCW_Check()
        {
            if (SelectedMPWeek != null)
            {
                try
                {
                    //string BSCW_Server_Path = $"{UserManager.SenatSettings.BSCW_Server_Drive}:\\{SelectedVintage}\\{SelectedMPWeek.MPWeekNumber}\\";
                    if (Directory.Exists($"{SelectedDrive}:\\"))
                    {
                        if (Directory.Exists($"{SelectedDrive}:\\Montagspost\\"))
                        {
                            Task task = BSCWCopy();
                            ViewManager.ShowMainInfoFlyout($"Die Daten werden eingelesen. Bitte warten Sie.", false);
                            ViewManager.MainWindowViewModel.ActionList.Clear();
                            ViewManager.MainWindowViewModel.ActionList.Add(new ActionStatus { ActionsStatusName = "Übertragung BSCW-Server" });
                            ViewManager.MainWindowViewModel.ShowStatusBar = true;
                            await task;
                            ViewManager.ShowMainInfoFlyout($"Die Daten wurden erfolgreich auf den BSCW-Server übertragen.", false);
                            ViewManager.MainWindowViewModel.ShowStatusBar = false;
                        }
                        else
                        {
                            ViewManager.ShowMainInfoFlyout($"Auf dem BSCW-Server konnte der Ordner 'Montagspost' nicht gefunden werden.", false);
                        }
                    }
                    else
                    {
                        ViewManager.ShowMainInfoFlyout($"Das Laufwerk {SelectedDrive}:\\ konnte nicht gefunden werden.", false);
                    }
                }
                catch (Exception ex)
                {
                    ViewManager.ShowMainInfoFlyout($"Es ist folgender Fehler aufgetreten: {ex.Message}", false);
                }
            }
            else
            {
                ViewManager.ShowMainInfoFlyout("Bitte wählen Sie eine Kalenderwoche aus.", false);
            }
        }
        private Task BSCWCopy()
        {
            Task task = Task.Run(() =>
            {
                List<string> ErrorList = new List<string>();
                foreach (MPDecision Decision in SelectedMPWeek.MPDecisions.OrderBy(x => x.SenatID))
                {
                    bool error = false;
                    string exportpath = (!MPBSCWSubFolders) ?
                        $"{SelectedDrive}:\\Montagspost\\{SelectedMPWeek.MPWeekYear}\\KW{SelectedMPWeek.MPWeekNumber}\\" :
                        $"{SelectedDrive}:\\{Decision.PathName.Substring(Decision.PathName.IndexOf(BGHKompaktSystemInfo.PathMontagspost))}"; ;
                    error = CreateFolder(exportpath);
                    if (!error)
                    {
                        string exportfile = $"{exportpath}{Decision.FileName}";
                        //MessageBox.Show(exportpath);
                        try
                        {
                            File.Copy($"{Decision.PathName}{Decision.FileName}", exportfile, true);
                            error = false;
                        }
                        catch (Exception)
                        {
                            error = true;
                        }
                    }
                    if (error) ErrorList.Add(Decision.PathName);
                }

                string text = string.Empty;
                foreach (string item in ErrorList) text += $"{item}";
                if (ErrorList.Count > 0) MessageBox.Show($"Folgende Dokumente konnte nicht auf den Server geladen werden: {text}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);

            });
            return task;
        }
        private bool CreateFolder(string pathName)
        {
            try
            {
                string folder = $"{SelectedDrive}:\\{BGHKompaktSystemInfo.PathMontagspost}";
                string[] creationPath = pathName.Split(new[] { Path.DirectorySeparatorChar });
                for (int i = 2; i < creationPath.Length - 1; i++)
                {
                    folder += $"{creationPath[i]}\\";
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                }
                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }

        #endregion


    }

}

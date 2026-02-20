using BGH_Kompakt.Classes.ActivityRequestClasses;
using BGH_Kompakt.Classes.Helper;
using BGH_Kompakt.Classes.Senate;
using BGH_Kompakt.Classes.UserClasses;
using BGH_Kompakt.Commands;
using BGH_Kompakt.Dtos;
using BGH_Kompakt.Enums;
using BGH_Kompakt.Services;
using BGH_Kompakt.Services.ActivityRequestService;
using BGH_Kompakt.Services.DBContexts;
using BGH_Kompakt.Services.SystemComponents;
using BGH_Kompakt.Services.UserService;
using BGH_Kompakt.Views;
using BGH_Kompakt.Views.Start;
using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Exception = System.Exception;


namespace BGH_Kompakt.ViewModel
{
    public partial class NebentaetigkeitenListViewModel : ViewModelBase
    {
        private readonly bool pageIsLoaded = false;
        public string Titel { get; set; } = string.Empty;
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SendCommand { get; set; }
        public ICommand RejectCommand { get; set; }
        public ICommand NewRequestCommand { get; set; }
        public ICommand SortingCommand { get; set; }
        public ICommand ReSetFilterCommand { get; set; }
        public ICommand DuplicateCommand { get; set; }


        //private ObservableCollection<ActivityRequest> _activities;
        public ActivityRequestDBContext dBContext = new ActivityRequestDBContext();

        public ObservableCollection<ActivityRequest> ActivityRequestsView { get; } = new ObservableCollection<ActivityRequest>();
        private bool _ShowBin = true;
        public bool ShowBin
        {
            get => _ShowBin;
            set { SetProperty<bool>(ref _ShowBin, value); }
        }
        private bool _ShowSend = true;
        public bool ShowSend
        {
            get => _ShowSend;
            set { SetProperty<bool>(ref _ShowSend, value); }
        }
        private bool _ShowDuplicate = true;
        public bool ShowDuplicate
        {
            get => _ShowDuplicate;
            set { SetProperty<bool>(ref _ShowDuplicate, value); }
        }
        private bool _ShowReject = false;
        public bool ShowReject
        {
            get => _ShowReject;
            set { SetProperty<bool>(ref _ShowReject, value); }
        }
        private bool _ShowWord = false;
        public bool ShowWord
        {
            get => _ShowWord;
            set { SetProperty<bool>(ref _ShowWord, value); }
        }
        private bool _ShowUser = false;
        public bool ShowUser
        {
            get => _ShowUser;
            set { SetProperty<bool>(ref _ShowUser, value); }
        }

        private bool _ShowSign = false;
        public bool ShowSign
        {
            get => _ShowSign;
            set { SetProperty<bool>(ref _ShowSign, value); }
        }


        private int _maxUsernameLength = 0;
        private double _WidthUser = 0;
        public double WidthUser
        {
            get => _WidthUser;
            set { SetProperty(ref _WidthUser, value); }
        }
        public string ToolTipSend { get; set; }
        #region MenuItems
        private bool _SortingAscanding = true;
        public bool SortingAscanding
        {
            get => _SortingAscanding;
            set { SetProperty(ref _SortingAscanding, value); }
        }
        private bool _SortingDate = true;
        public bool SortingDate
        {
            get => _SortingDate;
            set { SetProperty(ref _SortingDate, value); }
        }
        private bool _SortingTitle = false;
        public bool SortingTitle
        {
            get => _SortingTitle;
            set { SetProperty(ref _SortingTitle, value); }
        }
        private bool _SortingMeldeart = false;
        public bool SortingMeldeart
        {
            get => _SortingMeldeart;
            set { SetProperty(ref _SortingMeldeart, value); }
        }
        private bool _SortingTaetigkeitsart = false;
        public bool SortingTaetigkeitsart
        {
            get => _SortingTaetigkeitsart;
            set { SetProperty(ref _SortingTaetigkeitsart, value); }
        }
        private bool _SortingVerguetung = false;
        public bool SortingVerguetung
        {
            get => _SortingVerguetung;
            set { SetProperty(ref _SortingVerguetung, value); }
        }
        private bool _SortingZeitaufwand = false;
        public bool SortingZeitaufwand
        {
            get => _SortingZeitaufwand;
            set { SetProperty(ref _SortingZeitaufwand, value); }
        }
        private bool _SortingStatus = false;
        public bool SortingStatus
        {
            get => _SortingStatus;
            set { SetProperty(ref _SortingStatus, value); }
        }
        private bool _Status_Check = true;
        public bool Status_Check
        {
            get => _Status_Check;
            set { SetProperty(ref _Status_Check, value); }
        }
        private bool _Status_Exclamation = true;
        public bool Status_Exclamation
        {
            get => _Status_Exclamation;
            set { SetProperty(ref _Status_Exclamation, value); }
        }
        private bool _Status_Rejected = true;
        public bool Status_Rejected
        {
            get => _Status_Rejected;
            set { SetProperty(ref _Status_Rejected, value); }
        }
        #endregion

        #region Comboboxes

        private readonly string keinFilterBezeichnung = "Kein Filter";
        public ObservableCollection<ComboboxItem> FilterMeldeartList { get; set; } = new ObservableCollection<ComboboxItem>();
        public ObservableCollection<ComboboxItem> FilterTaetigkeitsartList { get; set; } = new ObservableCollection<ComboboxItem>();
        public ObservableCollection<ComboboxItem> FilterStatusList { get; set; } = new ObservableCollection<ComboboxItem>();
        public ObservableCollection<ComboboxItem> FilterAntragstellerList { get; set; } = new ObservableCollection<ComboboxItem>();

        private ComboboxItem _FilterAntragstellerSelected;
        public ComboboxItem FilterAntragstellerSelected
        {
            get => _FilterAntragstellerSelected;
            set
            {
                SetProperty(ref _FilterAntragstellerSelected, value);
                if (_FilterAntragstellerSelected != null) FilterList();
            }
        }
        private ComboboxItem _FilterMeldeartSelected;
        public ComboboxItem FilterMeldeartSelected
        {
            get => _FilterMeldeartSelected;
            set
            {
                SetProperty(ref _FilterMeldeartSelected, value);
                if (_FilterMeldeartSelected != null) FilterList();
            }
        }
        private ComboboxItem _FilterTaetigkeitsartSelected;
        public ComboboxItem FilterTaetigkeitsartSelected
        {
            get => _FilterTaetigkeitsartSelected;
            set
            {
                SetProperty(ref _FilterTaetigkeitsartSelected, value);
                if (_FilterTaetigkeitsartSelected != null) FilterList();
            }
        }

        private ComboboxItem _FilterStatusSelected;
        public ComboboxItem FilterStatusSelected
        {
            get => _FilterStatusSelected;
            set
            {
                SetProperty(ref _FilterStatusSelected, value);
                if (_FilterStatusSelected != null) FilterList();
            }
        }
        private void FilterList()
        {
            if (pageIsLoaded == true)
            {
                ActivityRequestsView.Clear();
                int setActivityArt = (ActivityRequestManager.ListArt == 1) ? 1 : 2;
                SetActivityRequestsView(setActivityArt);
            }
        }

        #endregion

        public NebentaetigkeitenListViewModel()
        {
            SetExecutes();
            SetSettings();
            FillComboboxes();
            SetShowInitial();
            pageIsLoaded = true;
        }

        private void SetShowInitial()
        {
            ShowBin = true;
            ShowSend = true;
            ShowWord = false;
            ShowReject = false;
        }

        private void SetSettings()
        {
            if (ActivityRequestManager.ListArt == 1)
            {
                SetActivityRequestsView(1);
                SetViewSettings(1, "Übersicht Anzeigen/Anträge", (UserManager.RegistratedUser.PositionId == 2) ? "Anzeige/Antrag bei der Vorsitzenden/dem Vorsitzenden einreichen" : "Anzeige/Antrag beim Präsidialbereich einreichen");
                ShowSign = false;
            }
            else if (ActivityRequestManager.ListArt == 2)
            {
                if (UserManager.AdminstatusCheck(UserEnums.EnumAdminStatus.Präsidialrichter.ToString()))
                    SetViewSettings(2, "Übersicht aller offenen Anträge", "Anzeige/Antrag an Präsidentin weiterleiten");
                else if (UserManager.AdminstatusCheck(UserEnums.EnumAdminStatus.Präsidentin.ToString()))
                    SetViewSettings(3, "Übersicht aller offenen Anträge", "Anzeige/Antrag billigen/ genehmigen");
                else if (UserManager.AdminstatusCheck(UserEnums.EnumAdminStatus.Vorzimmer.ToString()))
                    SetViewSettings(4, "Übersicht aller zu bearbeitenden Anträge", "Anzeige/Antrag in das Archiv geben");
                else if (UserManager.RegistratedUser.IsVorsitzenderRichter)
                    SetViewSettings(6, "Übersicht aller zu bearbeitenden Anträge", "Anzeige/Antrag an Präsidalrichter weiterleiten");
                SetActivityRequestsView(2);
                ShowSign = true;
            }
            else if (ActivityRequestManager.ListArt == 3)
            {
                ActivityRequestManager.AblageArt = 5;
                SetActivityRequestsView(2);
                ShowSign = true;
            }
        }

        private void SetViewSettings(int ablageArt, string titel, string toolTipSend)
        {
            ActivityRequestManager.AblageArt = ablageArt;
            Titel = titel;
            ToolTipSend = toolTipSend;
        }

        private void SetActivityRequestsView(int art)
        {
            ActivityRequestsView.Clear();
            switch (art)
            {
                case 1:
                    ShowUser = false;
                    try
                    {
                        var query = dBContext.ActivityRequests
                                        .Where(u => u.ARUserID == UserManager.RegistratedUser.UserId)
                                        .Include(a => a.ARVerguetungAdventages)
                                        .Include(x => x.ActivityRequestDataFiles)
                                        .Include(c => c.ActivityRequestChangeHistories)
                                        .Include(u => u.ActivityRequestStatusHistories)
                                        .OrderBy(x => x.ARDatum);
                        foreach (var activityRequest in query)
                        {
                            bool anzeige = true;
                            if (pageIsLoaded)
                            {
                                if (FilterMeldeartSelected.Item != keinFilterBezeichnung) anzeige = activityRequest.ActivityRequestMeldeArt.ActivityRequestMeldeArtText == FilterMeldeartSelected.Item;
                                if (anzeige && FilterTaetigkeitsartSelected.Item != keinFilterBezeichnung) anzeige = activityRequest.ActivityRequestTyp.ActivityRequestTypText == FilterTaetigkeitsartSelected.Item;
                                if (anzeige && FilterStatusSelected.Item != keinFilterBezeichnung) anzeige = activityRequest.Status == FilterStatusSelected.Item;
                            }
                            if (anzeige) ActivityRequestsView.Add(activityRequest);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog($"Es konnte keine Verbindung zur Nebentätigkeitendatenbank hergestellt werden. Es ist folgender Fehler aufgetreten: {ex.Message}; {ex.InnerException}");
                        ViewManager.ShowMainInfoFlyout("Es konnte keine Verbindung zur Nebentätigkeitendatenbank hergestellt werden. Bitte wenden Sie sich an den Administrator.", false);
                        ViewManager.ShowPageOnMainView<StartView>();
                        return;
                    }
                    SetWidthUser(0);
                    break;
                case 2:
                    ShowUser = true;
                    var query2 = dBContext.ActivityRequests
                                    .Where(u => u.ARZustaendigkeitsbereich == ActivityRequestManager.AblageArt)
                                    .Include(a => a.ARVerguetungAdventages)
                                    .Include(x => x.ActivityRequestDataFiles)
                                    .Include(c => c.ActivityRequestChangeHistories)
                                    .Include(u => u.ActivityRequestStatusHistories)
                                    .OrderBy(x => x.ARDatum);
                    try
                    {
                        foreach (var activityRequest in query2)
                        {
                            bool anzeige = true;
                            if (pageIsLoaded)
                            {
                                if (FilterAntragstellerSelected.Item != keinFilterBezeichnung)
                                {
                                    UserDBContext context = new UserDBContext();
                                    User ARuser = context.Users.FirstOrDefault(u => u.UserId == FilterAntragstellerSelected.Id);
                                    if (ARuser != null) anzeige = activityRequest.ARUserID == ARuser.UserId;
                                }
                                if (anzeige && FilterMeldeartSelected.Item != keinFilterBezeichnung) anzeige = activityRequest.ActivityRequestMeldeArt.ActivityRequestMeldeArtText == FilterMeldeartSelected.Item;
                                if (anzeige && FilterTaetigkeitsartSelected.Item != keinFilterBezeichnung) anzeige = activityRequest.ActivityRequestTyp.ActivityRequestTypText == FilterTaetigkeitsartSelected.Item;
                                if (anzeige && FilterStatusSelected.Item != keinFilterBezeichnung) anzeige = activityRequest.Status == FilterStatusSelected.Item;
                            }
                            if (ActivityRequestManager.AblageArt == 6)
                            {
                                if (UserManager.RegistratedUser.Senate == null) anzeige = false;
                                else
                                {
                                    if (activityRequest.ARUserID < 1) anzeige = false;
                                    else
                                    {
                                        List<Senat> senateVorsitzender = UserManager.RegistratedUser.Senate.ToList();
                                        UserDBContext context = new UserDBContext();
                                        User ARuser = context.Users.Include(x => x.Senate).FirstOrDefault(u => u.UserId == activityRequest.ARUserID);
                                        if (ARuser == null) anzeige = false;
                                        else
                                        {
                                            List<Senat> senateARUser = ARuser.Senate.ToList();
                                            anzeige = false;
                                            foreach (Senat senat in senateVorsitzender) if (senateARUser.FirstOrDefault(x => x.SenatID == senat.SenatID) != null) anzeige = true;
                                        }
                                    }
                                }
                            }
                            if (anzeige)
                            {
                                ActivityRequestsView.Add(activityRequest);
                                if (activityRequest.ARUser != null) 
                                    if (activityRequest.ARUser.Fullname.Length > _maxUsernameLength) 
                                        _maxUsernameLength = activityRequest.ARUser.Fullname.Length;
                            }
                        }
                        //Falls keine Userlength ermittelt werden soll, soll mindestens die Mindestbreite angezeigt werden; daher wird der Wert auf 1 gesetzt
                        SetWidthUser(_maxUsernameLength == 0 ? 1 : _maxUsernameLength);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog($"Es ist beim Einlesen der Daten für die Nebentätigkeiten folgender Fehler aufgetreten: {ex.Message}; {ex.InnerException}");
                    }
                    break;
            }
        }

        private void SetExecutes()
        {
            EditCommand = new RelayCommand(EditExecute);
            DeleteCommand = new RelayCommand(DeleteExecute);
            SendCommand = new RelayCommand(SendExecute);
            RejectCommand = new RelayCommand(RejectExecute);
            NewRequestCommand = new RelayCommand(NewRequestExecute);
            SortingCommand = new RelayCommand(SortingExecute);
            ReSetFilterCommand = new RelayCommand(ReSetFilterExecute, ReSetFilterCanExecute);
            DuplicateCommand = new RelayCommand(DuplicateExecute);
        }

        private void FillComboboxes()
        {
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        SetComboboxSource(FilterMeldeartList, 1);
                        break;
                    case 1:
                        SetComboboxSource(FilterTaetigkeitsartList, 2);
                        break;
                    case 2:
                        SetComboboxSource(FilterStatusList, 3);
                        break;
                    case 3:
                        SetComboboxSource(FilterAntragstellerList, 4);
                        break;
                }
            }
            FilterMeldeartSelected = FilterMeldeartList[0];
            FilterTaetigkeitsartSelected = FilterTaetigkeitsartList[0];
            FilterStatusSelected = FilterStatusList[0];
            FilterAntragstellerSelected = FilterAntragstellerList[0];
        }

        private void SetComboboxSource(ObservableCollection<ComboboxItem> list, int art)
        {
            try
            {
                List<string> stringList = new List<string>();
                list.Add(new ComboboxItem(keinFilterBezeichnung));
                if (art == 4)
                {
                    List<User> sortUserList = new List<User>();
                    foreach (ActivityRequest item in ActivityRequestsView)
                    {
                        if (item.ARUser != null) sortUserList.Add(item.ARUser);
                    }
                    sortUserList = sortUserList.OrderBy(x => x.NachName).ThenBy(y => y.VorName).ToList();
                    foreach (User user in sortUserList) stringList.Add($"{user.Fullname};{user.UserId}");

                }
                else
                {
                    foreach (ActivityRequest item in ActivityRequestsView)
                    {
                        switch (art)
                        {
                            case 1:
                                stringList.Add(item.ActivityRequestMeldeArt.ActivityRequestMeldeArtText);
                                break;
                            case 2:
                                stringList.Add(item.ActivityRequestTyp.ActivityRequestTypText);
                                break;
                            case 3:
                                stringList.Add(item.Status.ToString());
                                break;
                            case 4:
                                stringList.Add($"{item.ARUser.Fullname};{item.ARUserID}");
                                break;
                        }
                    }
                }
                stringList = stringList.Distinct().ToList();
                foreach (string cbArt in stringList)
                {
                    if (art == 4)
                    {
                        string[] user = cbArt.Split(';');
                        list.Add(new ComboboxItem(user[0], Int32.Parse(user[1])));
                    }
                    else
                    {
                        list.Add(new ComboboxItem(cbArt));
                    }
                }
            }
            catch (Exception ex) { ErrorMessage.CreateExceptionWithoutMessage("Fehler beim Befüllen der Filtercomboboxen", ex); }
        }

        private void SetWidthUser(int maxLength)
        {
            if (maxLength == 0) WidthUser = 0;
            else if (maxLength < 20) WidthUser = 130;
            else if (maxLength < 40) WidthUser = 200;
            else WidthUser = 250;
        }
        #region Executes
        private void SendExecute(object obj)
        {

            ARSendProperties aRSendProperties = new ARSendProperties();
            switch (ActivityRequestManager.AblageArt)
            {
                case 1: //Eigene Liste
                    aRSendProperties.MessageText1 = $"Soll der Eintrag {((UserManager.RegistratedUser.PositionId == 1) ? "beim Präsidialbereich" : "bei der Vorsitzenden / dem Vorsitzenden")} eingereicht werden?";
                    if (UserManager.RegistratedUser.PositionId > 1) aRSendProperties.MessageText1 += $"\nEs wird automatisch eine Benachrichtigungsemail erstellt.";
                    aRSendProperties.MessageText2 = "Der Eintrag wurde erfolgreich eingereicht.";
                    ShowBin = true;
                    ShowReject = false;
                    ShowWord = false;       
                    aRSendProperties.AblageArtExport = UserManager.RegistratedUser.PositionId == 1 ? 2 : 6;
                    aRSendProperties.ActivityRequestStatusID = 1;
                    break;
                case 2: //Präsidialbereich
                    aRSendProperties.MessageText1 = "Soll der Eintrag zur Präsidentin weitergeleitet werden?";
                    aRSendProperties.MessageText2 = "Der Eintrag wurde an die Präsidentin weitergeleitet.";
                    ShowBin = true;
                    ShowReject = true;
                    ShowWord = false;
                    aRSendProperties.AblageArtExport = 3;
                    aRSendProperties.ActivityRequestStatusID = 2;
                    break;
                case 3: //Präsidentinnenbereich
                    aRSendProperties.MessageText1 = "Soll der Eintrag genehmigt werden?";
                    aRSendProperties.MessageText2 = "Der Eintrag wurde genehmigt.";
                    ShowBin = false;
                    ShowReject = true;
                    ShowWord = false;
                    aRSendProperties.AblageArtExport = 4;
                    break;
                case 4: //Vorzimmer
                    aRSendProperties.MessageText1 = "Soll der Eintrag in das Archiv gelegt werden?";
                    aRSendProperties.MessageText2 = "Der Eintrag wurde in das Archiv abgelegt.";
                    ShowBin = false;
                    ShowReject = false;
                    ShowWord = true;
                    aRSendProperties.AblageArtExport = 5;
                    break;

            }

            bool Antwort = ViewManager.ShowQuestionWindow(aRSendProperties.MessageText1, "Ja");
            if (Antwort == true)
            {
                SendAcitivityRequest(aRSendProperties);
            }
        }

        private void SendAcitivityRequest(ARSendProperties aRSendProperties)
        {
            try
            {
                SelectedActivityRequest.ARZustaendigkeitsbereich = aRSendProperties.AblageArtExport;
                //SelectedActivityRequest.ActivityRequestStatusID = aRSendProperties.ActivityRequestStatusID;

                SelectedActivityRequest.ActivityRequestStatusHistories.Add(new ActivityRequestStatusHistory
                {
                    ActivityRequestID= SelectedActivityRequest.ActivityRequestId,
                    ActivityRequestStatusID = aRSendProperties.ActivityRequestStatusID,
                    Date= DateTime.Now
                });

                ActivityRequest sendRequest = SelectedActivityRequest;
                dBContext.ActivityRequests.AddOrUpdate(SelectedActivityRequest);
                dBContext.SaveChanges();
                //Liste neu füllen
                switch (ActivityRequestManager.AblageArt)
                {
                    case 1:
                        SetActivityRequestsView(1);
                        if (aRSendProperties.AblageArtExport == 6)
                        {
                            DBResponse responseEMails = SendEMail(sendRequest);
                            if (!responseEMails.Success)
                            {
                                Logger.WriteLog(responseEMails.Message);
                                ViewManager.ShowMainInfoFlyout(responseEMails.Message,false);
                            }
                        }
                        break;
                    case 2:
                    case 3:
                        SetActivityRequestsView(2);
                        break;
                }

                if (ActivityRequestManager.LoginType == 2) ActivityRequestsView.Remove(sendRequest);
                ViewManager.ShowMainInfoFlyout(aRSendProperties.MessageText2, false);
            }
            catch (Exception ex) { ErrorMessage.CreateExceptionWithFlyOutMessage("SendAcitivityRequest", ex); }
        }


        private DBResponse SendEMail(ActivityRequest sendRequest)
        {
            DBResponse eMailResponse = new DBResponse();
            try
            {
                List<User> EMailRecipientList = new List<User>();
                UserDBContext userDBContext = new UserDBContext();
                User HiWi = userDBContext.Users.Include(x => x.Senate).FirstOrDefault(x => x.UserId == sendRequest.ARUserID);
                if (HiWi != null && HiWi.Senate != null)
                {

                    List<Senat> HiWiSenatslist = HiWi.Senate.ToList();
                    foreach (Senat senat in HiWiSenatslist)
                    {
                        Senat searchSenat = userDBContext.Senate.Include(x => x.Users).FirstOrDefault(x => x.SenatID == senat.SenatID);
                        if (searchSenat != null)
                        {
                            foreach (User Member in searchSenat.Users)
                            {
                                if (Member.DienstbezeichnungId == 3 || Member.DienstbezeichnungId == 4) EMailRecipientList.Add(Member);
                            }
                        }
                    }
                    if (EMailRecipientList.Count > 0)
                    {
                        foreach(User member in EMailRecipientList)
                        {
                            if(member.EMail != string.Empty)
                            {        
                                string subjectEMail = $"{(sendRequest.ActivityRequestMeldeArtID == 1 ? "Neue Anzeige einer Nebentätigkeit " : "Neuer Antrag auf Genehmigung einer Nebentätigkeit")}";
                                string textBody = $"{(member.GeschlechtID == 1 ? "Sehr geehrter Herr " : "Sehr geehrte Frau ")} {member.FullSurname},<Br><Br>" +
                                                    $"ich habe {(sendRequest.ActivityRequestMeldeArtID == 1 ? "eine neue Anzeige einer Nebentätigkeit " : "einen neuen Antrag auf Genehmigung einer Nebentätigkeit")} eingereicht." +
                                                    $"<BR> Sie können {(sendRequest.ActivityRequestMeldeArtID == 1 ? "díe Anzeige " : "den Antrag ")} in BGHKompakt unter dem Menüpunkt Nebentätigkeiten einsehen.<BR><BR>" +
                                                    $"Mit freundlichen Grüßen<Br>{sendRequest.ARUser.Fullname}";
                                EMailVersand eMailVersand = new EMailVersand();
                                DBResponse dBResponse = eMailVersand.Send_Email(
                                    emailTo: member.EMail,
                                    subject: subjectEMail,
                                    mailBody: textBody);
                                if (!dBResponse.Success)
                                {
                                    Logger.WriteLog(dBResponse.Message);
                                    ViewManager.ShowMainInfoFlyout(dBResponse.Message, false);
                                }
                                
                            }
                            else
                            {
                                eMailResponse.Message += $"Die Benachrichtigungsemail konnte nicht an {member.FullSurname} versandt werden, da keine E-Mail-Adresse eingetragen war. ";
                            }
                        }
                        if (eMailResponse.Message == string.Empty) eMailResponse.Success = true;
                    }
                    else
                    {
                        eMailResponse.Success = false;
                        eMailResponse.Message = $"Die Benachrichtigungsemail konnte nicht versandt werden, da kein Vorsitzender zugeordnet werden konnte.";
                    }
                }
                else
                {
                    eMailResponse.Success = false;
                    eMailResponse.Message = $"Die Benachrichtigungsemail konnte nicht versandt werden, da kein Antragsteller gefunden wurde.";
                }
            }
            catch (Exception ex)
            {
                eMailResponse.Success = false;
                eMailResponse.Message = $"Beim E-Mail-Versand ist folgender Fehler aufgetreten: {ex.Message}";
            }
            return eMailResponse;
        }

        private void EditExecute(object obj)
        {
            ActivityRequestManager.SelectedActivityRequest = SelectedActivityRequest;
            ActivityRequestManager.ActionType = 2;
            //int art = ActivityRequestMangager.BereichsArt == 3 || ActivityRequestMangager.BereichsArt == 4 ? 2 : 1;
            ActivityRequestManager.DirectJump = true;
            ViewManager.NebentaetigkeitenView.RequestOwn.IsSelected = true;
            //ViewManager.ShowUnderPageOn<NebentaetigkeitenAnzeigeView>(ViewManager.NebentaetigkeitenView.AnimatedContentControl);
            //MessageBox.Show("Edit");
        }

        private void DuplicateExecute(object obj)
        {
            ActivityRequestManager.SelectedActivityRequest = SelectedActivityRequest;
            ActivityRequestManager.ActionType = 3;
            //int art = ActivityRequestMangager.BereichsArt == 3 || ActivityRequestMangager.BereichsArt == 4 ? 2 : 1;
            ActivityRequestManager.DirectJump = true;
            ViewManager.NebentaetigkeitenView.RequestOwn.IsSelected = true;
            //ViewManager.ShowUnderPageOn<NebentaetigkeitenAnzeigeView>(ViewManager.NebentaetigkeitenView.AnimatedContentControl);
        }


        private void DeleteExecute(object obj)
        {
            bool Antwort = ViewManager.ShowQuestionWindow("Soll der Eintrag gelöscht werden?", "Ja");
            if (Antwort == true)
            {
                try
                {
                    var deleteRequest = dBContext.ActivityRequests.Where(v => v.ActivityRequestId == SelectedActivityRequest.ActivityRequestId).FirstOrDefault();
                    if (deleteRequest != null)
                    {
                        ActivityRequestsView.Remove(deleteRequest);
                        dBContext.ActivityRequests.Remove(deleteRequest);
                        dBContext.SaveChanges();
                    }
                    else
                    {
                        ViewManager.ShowMainInfoFlyout("Der Eintrag konnte nicht gelöscht werden, da das Verfahren in der Datenbank nicht gefunden wurde", false);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ViewManager.ShowMainInfoFlyout($"Der Eintrag konnte nicht gelöscht werden. Es ist folgender Fehler aufgetreten: {ex.Message}", false);
                    return;
                }

            }
        }

        private void RejectExecute(object obj)
        {

            MessageBox.Show("RejectCommand");
        }
        private void NewRequestExecute(object obj)
        {
            ActivityRequestManager.LoginType = 1;
            ActivityRequestManager.ActionType = 1;
            ViewManager.ShowUnderPageOn<NebentaetigkeitenAnzeigeView>(ViewManager.NebentaetigkeitenView.AnimatedContentControl);
            ViewManager.SettingView.Grid_Settings.Visibility = Visibility.Visible;
        }
        private void SortingExecute(object obj)
        {
            if (obj != null)
            {
                switch ((string)obj)
                {
                    case "DatumAbsteigend":
                        SortingAscanding = true;
                        SetSorting(date: true);
                        SortActivityRequestList(nameof(AREnums.EnumARSorting.Eingangsdatum), SortingAscanding);
                        break;
                    case "DatumAufsteigend":
                        SortingAscanding = false;
                        SetSorting(date: true);
                        SortActivityRequestList(nameof(AREnums.EnumARSorting.Eingangsdatum), SortingAscanding);
                        break;
                    case "TitelAbsteigend":
                        SortingAscanding = true;
                        SetSorting(title: true);
                        SortActivityRequestList(nameof(AREnums.EnumARSorting.Titel), SortingAscanding);
                        break;
                    case "TitelAufsteigend":
                        SortingAscanding = false;
                        SetSorting(title: true);
                        SortActivityRequestList(nameof(AREnums.EnumARSorting.Titel), SortingAscanding);
                        break;
                    case "MeldeartAbsteigend":
                        SortingAscanding = true;
                        SetSorting(title: true);
                        SortActivityRequestList(nameof(AREnums.EnumARSorting.Meldeart), SortingAscanding);
                        break;
                    case "MeldeartAufsteigend":
                        SortingAscanding = false;
                        SetSorting(meldeart: true);
                        SortActivityRequestList(nameof(AREnums.EnumARSorting.Meldeart), SortingAscanding);
                        break;
                    case "TaetigkeitsartAbsteigend":
                        SortingAscanding = true;
                        SetSorting(taetigkeitsart: true);
                        SortActivityRequestList(nameof(AREnums.EnumARSorting.Tätigkeitsart), SortingAscanding);
                        break;
                    case "TaetigkeitsartAufsteigend":
                        SortingAscanding = false;
                        SetSorting(taetigkeitsart: true);
                        SortActivityRequestList(nameof(AREnums.EnumARSorting.Tätigkeitsart), SortingAscanding);
                        break;
                    case "VerguetungAbsteigend":
                        SortingAscanding = true;
                        SetSorting(verguetung: true);
                        SortActivityRequestList(nameof(AREnums.EnumARSorting.Vergütung), SortingAscanding);
                        break;
                    case "VerguetungAufsteigend":
                        SortingAscanding = false;
                        SetSorting(verguetung: true);
                        SortActivityRequestList(nameof(AREnums.EnumARSorting.Vergütung), SortingAscanding);
                        break;
                    case "StatusAbsteigend":
                        SortingAscanding = true;
                        SetSorting(status: true);
                        SortActivityRequestList(nameof(AREnums.EnumARSorting.Status), SortingAscanding);
                        break;
                    case "StatusAufsteigend":
                        SortingAscanding = false;
                        SetSorting(status: true);
                        SortActivityRequestList(nameof(AREnums.EnumARSorting.Status), SortingAscanding);
                        break;
                }
            }
        }
        private void ReSetFilterExecute(object obj)
        {
            ComboboxItem resetFilter = FilterMeldeartList.FirstOrDefault(f => f.Item == keinFilterBezeichnung);
            if (resetFilter != null) FilterMeldeartSelected = resetFilter;
            resetFilter = FilterAntragstellerList.FirstOrDefault(f => f.Item == keinFilterBezeichnung);
            if (resetFilter != null) FilterAntragstellerSelected= resetFilter;
            resetFilter = FilterTaetigkeitsartList.FirstOrDefault(f => f.Item == keinFilterBezeichnung);
            if (resetFilter != null) FilterTaetigkeitsartSelected= resetFilter;
            resetFilter = FilterStatusList.FirstOrDefault(f => f.Item == keinFilterBezeichnung);
            if (resetFilter != null) FilterStatusSelected= resetFilter;
                
        }

        private bool ReSetFilterCanExecute(object obj)
        {
            ComboboxItem resetFilter = new ComboboxItem(keinFilterBezeichnung);
            if (FilterAntragstellerSelected.Item != resetFilter.Item) return true;
            if (FilterMeldeartSelected.Item != resetFilter.Item) return true;
            if (FilterTaetigkeitsartSelected.Item != resetFilter.Item) return true;
            if (FilterStatusSelected.Item != resetFilter.Item) return true;
            return false;
        }


        #endregion
        private ActivityRequest _SelectedActivityRequest;
        public ActivityRequest SelectedActivityRequest
        {
            get => _SelectedActivityRequest;
            set
            {
                SetProperty<ActivityRequest>(ref _SelectedActivityRequest, value);

                if (SelectedActivityRequest != null)
                {
                    switch (ActivityRequestManager.ListArt)
                    {
                        case 1: //Eigene Übersicht
                            ShowBin = SelectedActivityRequest.ARZustaendigkeitsbereich == 1;
                            ShowReject = false;
                            ShowSend = SelectedActivityRequest.ARZustaendigkeitsbereich == 1;
                            ShowDuplicate = true;
                            ShowWord = false;
                            break;
                        case 2: //Präsdialrichter/in; Präsident/in; Vorzimmer
                            ShowBin = SelectedActivityRequest.ARZustaendigkeitsbereich == 2;
                            ShowReject = SelectedActivityRequest.ARZustaendigkeitsbereich == 2 || SelectedActivityRequest.ARZustaendigkeitsbereich == 3;
                            ShowSend = false;
                            ShowDuplicate = false;
                            ShowWord = SelectedActivityRequest.ARZustaendigkeitsbereich == 4;
                            break;
                        case 3: //Archiv
                            ShowBin = false;
                            ShowReject = false;
                            ShowSend = false;
                            ShowDuplicate = false;
                            ShowWord = false;
                            break;

                    }
                }
            }
        }

        #region SortingFunctions
        public void SetSorting(bool date = false, bool title = false, bool meldeart = false, bool taetigkeitsart = false, bool verguetung = false, bool zeitaufwand = false, bool status = false)
        {
            SortingDate = date;
            SortingTitle = title;
            SortingMeldeart = meldeart;
            SortingTaetigkeitsart = taetigkeitsart;
            SortingVerguetung = verguetung;
            SortingZeitaufwand = zeitaufwand;
            SortingStatus = status;
        }
        private void SortActivityRequestList(string bereich, bool sortingAsc)
        {
            if (ActivityRequestsView.Count > 0)
            {
                switch (bereich)
                {
                    case nameof(AREnums.EnumARSorting.Eingangsdatum):
                        var query1 = sortingAsc ? ActivityRequestsView.OrderBy(x => x.ARDatum).ToArray() : ActivityRequestsView.OrderByDescending(x => x.ARDatum).ToArray();
                        ActivityRequestsView.Clear();
                        foreach (ActivityRequest item in query1) ActivityRequestsView.Add(item);
                        break;
                    case nameof(AREnums.EnumARSorting.Titel):
                        var query2 = sortingAsc ? ActivityRequestsView.OrderBy(x => x.ARTitel).ToArray() : ActivityRequestsView.OrderByDescending(x => x.ARTitel).ToArray();
                        ActivityRequestsView.Clear();
                        foreach (ActivityRequest item in query2) ActivityRequestsView.Add(item);
                        break;
                    case nameof(AREnums.EnumARSorting.Meldeart):
                        var query3 = sortingAsc ? ActivityRequestsView.OrderBy(x => x.ActivityRequestMeldeArtID).ToArray() : ActivityRequestsView.OrderByDescending(x => x.ActivityRequestMeldeArtID).ToArray();
                        ActivityRequestsView.Clear();
                        foreach (ActivityRequest item in query3) ActivityRequestsView.Add(item);
                        break;
                    case nameof(AREnums.EnumARSorting.Tätigkeitsart):
                        var query4 = sortingAsc ? ActivityRequestsView.OrderBy(x => x.ActivityRequestTyp.ActivityRequestTypText).ToArray() : ActivityRequestsView.OrderByDescending(x => x.ActivityRequestTyp.ActivityRequestTypText).ToArray();
                        ActivityRequestsView.Clear();
                        foreach (ActivityRequest item in query4) ActivityRequestsView.Add(item);
                        break;
                    case nameof(AREnums.EnumARSorting.Vergütung):
                        var query5 = sortingAsc ? ActivityRequestsView.OrderBy(x => x.ARVerguetung).ToArray() : ActivityRequestsView.OrderByDescending(x => x.ARVerguetung).ToArray();
                        ActivityRequestsView.Clear();
                        foreach (ActivityRequest item in query5) ActivityRequestsView.Add(item);
                        break;
                    case nameof(AREnums.EnumARSorting.Zeitaufwand):
                        var query6 = sortingAsc ? ActivityRequestsView.OrderBy(x => x.ARZeitaufwandMain).ToArray() : ActivityRequestsView.OrderByDescending(x => x.ARZeitaufwandMain).ToArray();
                        ActivityRequestsView.Clear();
                        foreach (ActivityRequest item in query6) ActivityRequestsView.Add(item);
                        break;
                    case nameof(AREnums.EnumARSorting.Status):
                        var query7 = sortingAsc ? ActivityRequestsView.OrderBy(x => x.Status).ToArray() : ActivityRequestsView.OrderByDescending(x => x.Status).ToArray();
                        ActivityRequestsView.Clear();
                        foreach (ActivityRequest item in query7) ActivityRequestsView.Add(item);
                        break;
                }
            }
        }
        #endregion

    }
}
